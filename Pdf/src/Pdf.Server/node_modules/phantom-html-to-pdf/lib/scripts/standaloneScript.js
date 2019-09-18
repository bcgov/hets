/*!
 * Copyright(c) 2015 Jan Blaha
 */
/* globals phantom */

var maxLogEntrySize = require("system").env['PHANTON_MAX_LOG_ENTRY_SIZE']
var messages = [];

function trimMessage(pars) {
    var message = Array.prototype.join.call(pars, ' ')

    // this is special case, because phantom logs base64 images content completely into the output
    if (message.indexOf('Request data:image') === 0 && message.length > 100) {
        return message.substring(0, 100) + '...';
    }

    if (message.length > maxLogEntrySize) {
        return message.substring(0, maxLogEntrySize) + '...';
    }

    return message;
};

console.log = function(m) {
    messages.push({ timestamp: new Date().getTime(), message: trimMessage(arguments), level: 'debug'});
};

console.error = function(m) {
    messages.push({ timestamp: new Date().getTime(), message: trimMessage(arguments), level: 'error'});
};
console.warn = function(m) {
    messages.push({ timestamp: new Date().getTime(), message: trimMessage(arguments), level: 'warn'});
};

try {
    console.log('Converting in dedicated phantomjs ' + phantom.version.major + '.' + phantom.version.minor + '.' + phantom.version.patch);
    var webpage = require('webpage');
    var system = require('system');
    var fs = require('fs');
    var page = webpage.create();

    var settingsFile = system.args[system.args.length - 1];
    var stream = fs.open(settingsFile, "r");
    var body = JSON.parse(stream.read());
    stream.close();

    function respond(page, body){
        system.stdout.write(JSON.stringify({
            logs: messages,
            numberOfPages: body.numberOfPages
        }));

        // Work-around to avoid "Unsafe JavaScript attempt to access frame" warning in PhantomJS 1.9.8.
        // See: https://github.com/ariya/phantomjs/issues/12697
        // since we rely on stdout for the dedicated-process strategy this work-around
        // ensures the phantom process don't log anything we don't want
        page.close();

        setTimeout(function (){
            phantom.exit(0);
        }, 0);
    }

    var pageJSisDone = body.waitForJS ? false : true;

page.viewportSize = {
    width: body.viewportSize.width || 600,
    height: body.viewportSize.height || 600
};

page.settings.javascriptEnabled = body.settings.javascriptEnabled !== false;
page.settings.resourceTimeout = body.settings.resourceTimeout || 10000;


if(body.cookies.length > 0) {
    for(var i in body.cookies) {
        page.addCookie(body.cookies[i]);
    }
}

page.onResourceRequested = function (request, networkRequest) {
    console.log('Request ' + request.url);
    if (request.url.lastIndexOf(body.url, 0) === 0) {
        return;
    }

    //potentially dangerous request
    if (request.url.lastIndexOf("file:///", 0) === 0 && !body.allowLocalFilesAccess) {
        networkRequest.abort();
        return;
    }

    //to support cdn like format //cdn.jquery...
    if (request.url.lastIndexOf("file://", 0) === 0 && request.url.lastIndexOf("file:///", 0) !== 0) {
        networkRequest.changeUrl(request.url.replace("file://", "http://"));
    }

    if (body.waitForJS && request.url.lastIndexOf("http://intruct-javascript-ending", 0) === 0) {
        pageJSisDone = true;
    }
};

page.onConsoleMessage = function(msg, line, source) {
    console.log(msg, line, source);
};

page.onResourceError = function(resourceError) {
    console.warn('Unable to load resource (#' + resourceError.id + 'URL:' + resourceError.url + ')');
    console.warn('Error code: ' + resourceError.errorCode + '. Description: ' + resourceError.errorString);
};

page.onError = function (msg, trace) {
    console.warn(msg);
    trace.forEach(function(item) {
        console.warn('  ', item.file, ':', item.line);
    });
};

page.onInitialized = function() {
    if (body.injectJs && body.injectJs.length > 0) {
        body.injectJs.forEach(function(script) {
            console.log('Injecting ' + script);
            page.injectJs(script);
        });
    }
   
    // inject function to the page in order to the client can instruct the ending of its JS
    if (body.waitForJS) {
        page.evaluate(function(varName) {
            if (typeof Object.defineProperty === 'function') {
                Object.defineProperty(window, varName, {
                    set: function(val) {
                        if (!val)
                            return;

                        if (val === true) {
                            var scriptNode = document.createElement("script");
                            scriptNode.src = 'http://intruct-javascript-ending';
                            document.body.appendChild(scriptNode);
                        }
                    }
                });
            }
        }, body.waitForJSVarName);
    }
};

page.open(body.url, function () {
    
    var phantomHeader = page.evaluate(function (s) {
        return document.querySelector(s) ? document.querySelector(s).innerHTML : null;
    }, '#phantomHeader');

    var phantomFooter = page.evaluate(function (s) {
        return document.querySelector(s) ? document.querySelector(s).innerHTML : null;
    }, '#phantomFooter');

    body.numberOfPages = 0;

    page.paperSize = {
        format: body.paperSize.format || "",
        orientation: body.paperSize.orientation,
        margin: body.paperSize.margin || "1cm",
        width: body.paperSize.width || undefined,
        height: body.paperSize.height || undefined,
        header: {
            height: body.paperSize.headerHeight || ((phantomHeader || body.headerFile) ? "1cm" : "1mm"),
            contents: phantom.callback(function (pageNum, numPages) {
                body.numberOfPages = numPages;

                if (!phantomHeader && !body.headerFile)
                    return "<span></span>";

                if (!phantomHeader) {
                    var stream = fs.open(body.headerFile, "r");
                    phantomHeader = stream.read();
                    stream.close();
                }

                return phantomHeader.replace(/{#pageNum}/g, pageNum).replace(/{#numPages}/g, numPages);
            })
        },
        footer: (body.footerFile || phantomFooter) ? {
            height: body.paperSize.footerHeight || "1cm",
            contents: phantom.callback(function (pageNum, numPages) {
                if (!phantomFooter) {
                    var stream = fs.open(body.footerFile, "r");
                    phantomFooter = stream.read();
                    stream.close();
                }

                return phantomFooter.replace(/{#pageNum}/g, pageNum).replace(/{#numPages}/g, numPages);
            })
        } : undefined
    };

    if (body.customHeaders) {
        page.customHeaders = body.customHeaders;
    }

    page.zoomFactor = 1;
    if(body.fitToPage) {
        var widths = page.evaluate(function() {
            return {
                scrollWidth : document.body.scrollWidth,
                offsetWidth : document.body.offsetWidth
            };
        });

        if(widths.scrollWidth > widths.offsetWidth) {
            page.zoomFactor =(widths.offsetWidth / widths.scrollWidth);
        }

    }

    setTimeout(function () {
        resolvePage()
    }, body.printDelay || 0);

    function resolvePage() {
        if (body.waitForJS && !pageJSisDone) {
            setTimeout(function() {
                console.log('PhantomJS printing done')
                resolvePage();
            }, 100);
            return;
        }

        page.render(body.output, body.format);
        respond(page, body);
    }
});

}
catch(e) {
    console.error(e.message);
    e.message += '; log: ' + JSON.stringify(messages);
    system.stderr.write(JSON.stringify(e));
    phantom.exit(1);
}



