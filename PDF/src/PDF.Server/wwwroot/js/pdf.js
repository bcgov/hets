module.exports = function (callback, rawdata, pdfOptions) {

    console.log("pdf.js: starting pdf generation");

    const defaultPdfOptions = {
        format: 'letter',
        orientation: 'landscape' // portrait or landscape
    }

    try {
        // setup pdf module
        console.log("pdf.js: get pdf module");
        var pdf = require('html-pdf');

        console.log("pdf.js: setup options");
        var options = Object.assign({}, defaultPdfOptions, pdfOptions);

        // export as PDF
        console.log("pdf.js: calling pdf.create");
        pdf.create(rawdata, options).toBuffer(function (err, buffer) {
            if (err) {
                console.log("pdf.create: error");
                callback(err, null);
            }
            else {
                console.log("pdf.js: Done!");
                callback(null, buffer.toJSON());
            }
        });
    }
    catch (err) {
        console.log("pdf.js: error");
        callback(err, null);
    }         
};