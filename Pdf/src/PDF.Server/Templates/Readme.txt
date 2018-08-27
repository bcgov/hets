A typical Mustache template:
Hello {{name}}
You have just won {{value}} dollars!
{{#in_ca}}
Well, {{taxed_value}} dollars, after taxes.
{{/in_ca}}


Given the following hash:
{
  "name": "Chris",
  "value": 10000,
  "taxed_value": 10000 - (10000 * 0.4),
  "in_ca": true
}


Will produce the following:
Hello Chris
You have just won 10000 dollars!
Well, 6000.0 dollars, after taxes.
