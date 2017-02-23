
module.exports = function (callback) {

	// https://www.npmjs.com/package/mustache
	var mustache = require ('mustache');
	// https://www.npmjs.com/package/html-pdf
	var pdf = require('html-pdf');
	
	// setup mustache template
	
	var template = "{{title}}";
	var view = {
		title: "Hello World"
	};
	
	// render
	
	var html = mustache.render( template, view )
	
	// PDF options
	var options = { format: 'Letter' };
	
	// export as PDF
	pdf.create(html).toBuffer(function(err, buffer){
		callback (null, buffer.toJSON());
	});	    
};