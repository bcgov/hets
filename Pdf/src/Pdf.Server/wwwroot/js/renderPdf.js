module.exports = function (callback, data) {

    console.log("renderPdf.js: starting pdf generation");
    console.log("renderPdf.js: data: " + data);

    var jsreport = require('jsreport-core')({
        allowLocalFilesAccess: true,
        templatingEngines: { strategy: 'in-process' },
        store: { provider: 'fs' },
        phantom: {
            strategy: 'dedicated-process',
            numberOfWorkers: 2,
            timeout: 180000,
            allowLocalFilesAccess: true,
            defaultPhantomjsVersion: '1.9.8'
        }
    });

    jsreport.use(require('jsreport-phantom-pdf')());
    jsreport.init();

    jsreport.init().then(function() {
        return jsreport.render({
            template: {
                content: data,
                engine: 'none',
                recipe: 'phantom-pdf'
            },
            data: { name: "jsreport" }
        }).then(function(resp) {
            callback(null, resp.content.toJSON().data);
        });
    }).catch(function(e) {
        callback(e, null);
    });
};