import test from 'node:test';
import assert from 'node:assert/strict';
import { expandTag, normalizeTag, resolveImages, verifyMasterRevision } from './images.mjs';

const revision = 'a'.repeat(40);
const digest = `sha256:${'b'.repeat(64)}`;
const manifest = (rev = revision) => ({ annotations: { 'org.opencontainers.image.source': 'https://github.com/bcgov/hets', 'org.opencontainers.image.revision': rev }, manifests: [{ platform: { os: 'linux', architecture: 'amd64' } }] });
const registry = (clientRevision = revision) => (_command, args) => {
  if (args.includes('--format')) return JSON.stringify({ digest });
  return JSON.stringify(manifest(args.some((a) => a.includes('/client')) ? clientRevision : revision));
};

test('expands short commit references to the full SHA used by CI', () => {
  assert.equal(expandTag('aaaaaaa', () => revision), `sha-${revision}`);
  assert.equal(expandTag('pr-56', () => { throw new Error('unexpected lookup'); }), 'pr-56');
  assert.throws(() => expandTag('aaaaaaa', () => 'ambiguous'), /unique full commit/);
});

test('normalizes accepted tags and rejects shell/path input', () => {
  assert.equal(normalizeTag('ABCDEF1'), 'sha-abcdef1');
  for (const tag of ['pr-56', 'rc_1.2.3', 'v1.2.3']) assert.equal(normalizeTag(tag), tag);
  for (const tag of ['latest', '../prod', 'x;whoami', '$(id)', 'pr-0', '']) assert.throws(() => normalizeTag(tag));
});
test('pins both components to digests from one source revision', () => {
  const release = resolveImages(`sha-${revision}`, registry());
  assert.equal(release.revision, revision);
  assert.equal(release.images.api, `ghcr.io/bcgov/hets/api@${digest}`);
  assert.equal(release.images.client, `ghcr.io/bcgov/hets/client@${digest}`);
});
test('rejects partially updated PR image pairs', () => {
  assert.throws(() => resolveImages('pr-56', registry('c'.repeat(40))), /different commits/);
});
test('rejects mismatched SHA aliases', () => {
  assert.throws(() => resolveImages('sha-ccccccc', registry()), /requested SHA/);
});
test('reads source metadata through the resolved digest rather than the mutable alias', () => {
  const execute = (command, args) => {
    if (args.includes('--raw')) assert.ok(args.some((a) => a.endsWith(`@${digest}`)));
    return registry()(command, args);
  };
  resolveImages('pr-56', execute);
});
test('requires complete source metadata, digest and amd64 platform', () => {
  for (const broken of [{}, { ...manifest(), manifests: [] }, { ...manifest(), annotations: {} }]) {
    assert.throws(() => resolveImages('pr-56', () => JSON.stringify(broken)));
  }
  assert.throws(() => resolveImages('pr-56', (_c, a) => JSON.stringify(a.includes('--format') ? { digest: 'invalid' } : manifest())), /Invalid manifest digest/);
});
test('accepts merged revisions and rejects unmerged revisions', () => {
  for (const status of ['behind', 'identical']) verifyMasterRevision(revision, () => JSON.stringify({ status }));
  for (const status of ['ahead', 'diverged']) assert.throws(() => verifyMasterRevision(revision, () => JSON.stringify({ status })), /merged/);
});
