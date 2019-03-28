#!/usr/bin/env node

/*eslint-env node*/

'use strict';

var toxy = require('../node_modules/toxy');
var argv = require('minimist')(process.argv.slice(2));

var port = argv.port || argv.p || 8000;
var proxy = toxy();
// var rules = proxy.rules;
var poisons = proxy.poisons;

proxy.forward('http://localhost:' + port);

// proxy.all('/api/users/search')
//   .poison(toxy.poisons.inject({
//     code: 503,
//     body: '{"error": "toxy injected error"}',
//     headers: {'Content-Type': 'application/json'},
//   }));

// proxy.all('/api/roles')
//   .redirect('https://logontest7.gov.bc.ca/clp-cgi/int/logon.cgi');

// proxy.all('/api/business/owner/*')
//   .redirect('https://logontest7.gov.bc.ca/clp-cgi/int/logon.cgi');

// make initial app load fast
proxy.get('/api/authentication/dev/token/*').disableAll();
proxy.get('/api/users/current').disableAll();
proxy.get('/api/districts').disableAll();
proxy.get('/api/regions').disableAll();
proxy.get('/api/serviceareas').disableAll();
proxy.get('/api/districts/*/localAreas').disableAll();
proxy.get('/api/districts/*/fiscalYears').disableAll();
proxy.get('/api/permissions').disableAll();
proxy.get('/api/userdistricts').disableAll();

proxy.all('/api/*')
  .poison(poisons.latency({ min: 500, max: 2000 }))
  .poison(poisons.slowRead({ bps: 100 }));

proxy.all('/*');

proxy.listen(3000);
console.log('Server listening on port:', 3000);
console.log('Proxying to API on port:', port);
