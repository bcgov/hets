module.exports = function (callback, template, data) {

    var mustache = require("mustache");
    var output = mustache.render(template, data);

    callback(null, output);    
};