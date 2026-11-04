import { readFileSync, writeFileSync } from 'node:fs';

// Preserve the buildinfo.html contract previously supplied by the S2I image.
const { version } = JSON.parse(readFileSync(new URL('../package.json', import.meta.url), 'utf8'));
const revision = process.env.SOURCE_REVISION || 'unknown';
const escape = (value) => String(value).replace(/[&<>"']/g, (character) => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' })[character]);
const timestamp = new Date().toISOString();
writeFileSync(new URL('../build/buildinfo.html', import.meta.url), `<!doctype html>
<html lang="en"><head><title>HETS build information</title></head><body>
<time id="buildtime" data-buildtime="${timestamp}">${timestamp}</time>
<span id="version">${escape(version)}</span>
<span id="commit">${escape(revision)}</span>
</body></html>
`);
