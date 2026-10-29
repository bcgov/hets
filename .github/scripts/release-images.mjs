import { spawnSync } from 'node:child_process';
import { fileURLToPath } from 'node:url';
import { expandTag, normalizeTag, resolveImages, run, verifyMasterRevision } from './images.mjs';

export function releaseTag(source, destination) {
  const tag = normalizeTag(destination);
  if (!/^(rc_|v)[0-9]+\.[0-9]+\.[0-9]+$/.test(tag)) throw new Error('Release tag must be rc_x.y.z or vX.Y.Z');
  if (tag.startsWith('rc_') && !source.startsWith('sha-')) throw new Error('An RC release must select a built commit SHA');
  if (tag.startsWith('v') && source !== 'rc_' + tag.slice(1)) throw new Error('A production version must promote the corresponding RC version');
  return tag;
}

function lookup(command, args, missingPattern) {
  const result = spawnSync(command, args, { encoding: 'utf8' });
  if (result.error) throw result.error;
  if (result.status === 0) return JSON.parse(result.stdout);
  if (missingPattern.test(result.stderr)) return null;
  throw new Error(result.stderr || command + ' failed');
}

export function publishRelease(sourceTag, destinationTag, execute = run, probe = lookup) {
  const source = expandTag(sourceTag, execute);
  const destination = releaseTag(source, destinationTag);
  const release = resolveImages(source, execute);
  verifyMasterRevision(release.revision, execute);
  const ref = probe('gh', ['api', 'repos/bcgov/hets/git/ref/tags/' + destination], /HTTP 404/);
  if (ref && (ref.object.type !== 'commit' || ref.object.sha !== release.revision)) throw new Error('Existing Git tag points to a different commit');

  // Validate every destination before creating any alias. A retry may finish a
  // partial release, but it must never overwrite an alias with different bytes.
  const pending = [];
  for (const component of ['api', 'client']) {
    const target = 'ghcr.io/bcgov/hets/' + component + ':' + destination;
    const existing = probe('docker', ['buildx', 'imagetools', 'inspect', target, '--format', '{{json .Manifest}}'], /manifest unknown|not found|no such manifest/i);
    const expected = release.images[component].split('@')[1];
    if (existing && existing.digest !== expected) throw new Error(target + ' already points to a different image');
    if (!existing) pending.push([target, release.images[component]]);
  }
  for (const [target, image] of pending) {
    execute('docker', ['buildx', 'imagetools', 'create', '--prefer-index=false', '--tag', target, image]);
  }
  const verified = resolveImages(destination, execute);
  if (JSON.stringify(verified.images) !== JSON.stringify(release.images)) throw new Error('Release aliases do not match the verified source digests');

  if (!ref) {
    execute('gh', ['api', '--method', 'POST', 'repos/bcgov/hets/git/refs', '-f', 'ref=refs/tags/' + destination, '-f', 'sha=' + release.revision]);
  }
  const published = probe('gh', ['api', 'repos/bcgov/hets/releases/tags/' + destination], /HTTP 404/);
  if (!published) {
    execute('gh', ['release', 'create', destination, '--repo', 'bcgov/hets', '--target', release.revision, '--verify-tag', '--generate-notes', ...(destination.startsWith('rc_') ? ['--prerelease'] : [])]);
  }
  return verified;
}

if (process.argv[1] === fileURLToPath(import.meta.url)) {
  const [source, destination] = process.argv.slice(2);
  if (!source || !destination) throw new Error('Usage: release-images.mjs <source image tag> <release tag>');
  process.stdout.write(JSON.stringify(publishRelease(source, destination), null, 2) + '\n');
}
