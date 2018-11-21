module.exports = function (callback, data) {

    console.log("renderPdf.js: starting pdf generation");
   
    var jsreport = require('jsreport-core')({
        allowLocalFilesAccess: false,
        tasks: { 
            strategy: 'in-process',  
            allowedModules: '*',
            timeout: 180000
        }
    });
    
    jsreport.use(require('jsreport-phantom-pdf')({
        strategy: 'dedicated-process',
        numberOfWorkers: 2,
        timeout: 180000,
        allowLocalFilesAccess: false,
        defaultPhantomjsVersion: '1.9.8'
    }));

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
    }).catch(function(err) {
        console.log("renderPdf.js: error");
        callback(err, null);
    });      
};