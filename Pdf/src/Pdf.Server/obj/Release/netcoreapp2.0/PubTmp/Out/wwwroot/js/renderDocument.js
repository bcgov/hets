module.exports = function (callback, template, data) {

    console.log("renderDocument.js: starting mustache html generation");

    try {
        console.log("renderDocument.js: get mustache module");
        var mustache = require("mustache");

        console.log("renderDocument.js: calling mustache.render");
        var output = mustache.render(template, data);

        console.log("renderDocument.js: Done!");
        callback(null, output);    
    }
    catch (err) {
        console.log("renderDocument.js: error");
        callback(err, null);
    }    
};