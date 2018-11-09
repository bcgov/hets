module.exports = function (callback, html) {

    console.log("createPdf.js: starting jsreport pdf generation");

    try {
        console.log("createPdf.js: get jsreport module");
        var jsreport = require('jsreport-core')({
            "tasks": {
                "strategy": "dedicated-process",
                "timeout": 120000,
                "allowedModules": []
            },
            "scripts": {
                "allowedModules": [],
                "timeout": 120000
            }
        });

        jsreport.init().then(function() {
            return jsreport.render({                
                template: {
                    content: html,
                    engine: 'jsrender',
                    recipe: 'phantom-pdf'
                }
            }).then(function(resp) {
                callback(null, resp.content.toJSON().data);
            });
        });
    }
    catch (err) {
        console.log("createPdf.js: error");
        callback(err, null);
    }    
};