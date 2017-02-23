// basic test for the pdf engine.

var pdf = require('./pdf.js');

pdf(function (error, buffer){console.log (buffer);}
);