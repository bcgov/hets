import test from 'node:test';
import assert from 'node:assert/strict';
import { publishRelease, releaseTag } from './release-images.mjs';

const revision = 'a'.repeat(40);
const digest = 'sha256:' + 'b'.repeat(64);
const manifest = { annotations: { 'org.opencontainers.image.source': 'https://github.com/bcgov/hets', 'org.opencontainers.image.revision': revision }, manifests: [{ platform: { os: 'linux', architecture: 'amd64' } }] };
function harness(probe = () => null) {
  const mutations = [];
  const execute = (command, args) => {
    if (command === 'gh' && args.some((a) => a.includes('/compare/'))) return JSON.stringify({ status: 'identical' });
    if (command === 'docker' && args.includes('inspect')) return JSON.stringify(args.includes('--raw') ? manifest : { digest });
    mutations.push([command, args]);
    return '{}';
  };
  return { mutations, execute, probe };
}

test('production releases must promote the same RC version', () => {
  assert.equal(releaseTag('rc_1.2.3', 'v1.2.3'), 'v1.2.3');
  assert.throws(() => releaseTag('rc_1.2.3', 'v1.2.4'), /corresponding RC/);
  assert.throws(() => releaseTag('rc_1.2.3', 'rc_1.2.4'), /built commit SHA/);
  assert.throws(() => releaseTag('pr-56', 'latest'));
});
test('publishes image aliases before the Git tag and release', () => {
  const h = harness();
  publishRelease('sha-' + revision, 'rc_1.2.3', h.execute, h.probe);
  assert.deepEqual(h.mutations.map(([command]) => command), ['docker', 'docker', 'gh', 'gh']);
  assert.ok(h.mutations[0][1].includes('ghcr.io/bcgov/hets/api@' + digest));
});
test('rejects a conflicting existing image before any writes', () => {
  const h = harness((command) => command === 'docker' ? { digest: 'sha256:' + 'c'.repeat(64) } : null);
  assert.throws(() => publishRelease('sha-' + revision, 'rc_1.2.3', h.execute, h.probe), /different image/);
  assert.equal(h.mutations.length, 0);
});
test('rejects a conflicting Git tag before any writes', () => {
  const h = harness(() => ({ object: { type: 'commit', sha: 'c'.repeat(40) } }));
  assert.throws(() => publishRelease('sha-' + revision, 'rc_1.2.3', h.execute, h.probe), /different commit/);
  assert.equal(h.mutations.length, 0);
});
test('retries an already completed release without rewriting aliases or tags', () => {
  const h = harness((command, args) => {
    if (command === 'docker') return { digest };
    if (args.some((a) => a.includes('/git/ref/'))) return { object: { type: 'commit', sha: revision } };
    return { tag_name: 'rc_1.2.3' };
  });
  publishRelease('sha-' + revision, 'rc_1.2.3', h.execute, h.probe);
  assert.equal(h.mutations.length, 0);
});
