import { execFileSync } from 'node:child_process';
import { fileURLToPath } from 'node:url';

export const run = (command, args) => execFileSync(command, args, { encoding: 'utf8', stdio: ['ignore', 'pipe', 'inherit'] }).trim();

export function normalizeTag(input) {
  const tag = input.trim();
  if (/^(?:sha-)?[0-9a-fA-F]{7,40}$/.test(tag)) return `sha-${tag.replace(/^sha-/, '').toLowerCase()}`;
  if (/^(?:pr-[1-9][0-9]*|rc_[0-9]+\.[0-9]+\.[0-9]+|v[0-9]+\.[0-9]+\.[0-9]+)$/.test(tag)) return tag;
  throw new Error('Expected a commit SHA, sha-<SHA>, pr-<number>, rc_x.y.z or vX.Y.Z image tag');
}

export function expandTag(input, execute = run) {
  const tag = normalizeTag(input);
  if (tag.startsWith('sha-') && tag.length < 44) {
    const revision = execute('gh', ['api', `repos/bcgov/hets/commits/${tag.slice(4)}`, '--jq', '.sha']);
    if (!/^[a-f0-9]{40}$/.test(revision)) throw new Error('Could not resolve a unique full commit SHA');
    return `sha-${revision}`;
  }
  return tag;
}

export function resolveImages(input, execute = run) {
  const tag = normalizeTag(input);
  const images = {};
  let revision;
  for (const component of ['api', 'client']) {
    const repository = `ghcr.io/bcgov/hets/${component}`;
    const ref = `${repository}:${tag}`;
    const descriptor = JSON.parse(execute('docker', ['buildx', 'imagetools', 'inspect', ref, '--format', '{{json .Manifest}}']));
    const digest = descriptor.digest;
    if (!/^sha256:[a-f0-9]{64}$/.test(digest ?? '')) throw new Error(`Invalid manifest digest for ${ref}`);
    // Read metadata through the immutable digest, so changing a PR alias
    // cannot make us validate one manifest and deploy another.
    const raw = execute('docker', ['buildx', 'imagetools', 'inspect', `${repository}@${digest}`, '--raw']);
    const manifest = JSON.parse(raw);
    const annotations = manifest.annotations ?? {};
    const source = annotations['org.opencontainers.image.source'];
    const sourceRevision = annotations['org.opencontainers.image.revision'];
    if (source !== 'https://github.com/bcgov/hets' || !/^[a-f0-9]{40}$/.test(sourceRevision ?? '')) {
      throw new Error(`${ref} is missing HETS source/revision annotations`);
    }
    if (!manifest.manifests?.some((m) => m.platform?.os === 'linux' && m.platform?.architecture === 'amd64')) {
      throw new Error(`${ref} does not contain a linux/amd64 image`);
    }
    if (revision && revision !== sourceRevision) throw new Error('API and client images were built from different commits');
    if (tag.startsWith('sha-') && !sourceRevision.startsWith(tag.slice(4))) throw new Error('Image revision does not match the requested SHA');
    revision = sourceRevision;
    images[component] = `${repository}@${digest}`;
  }
  return { tag, revision, images };
}

export function verifyMasterRevision(revision, execute = run) {
  const comparison = JSON.parse(execute('gh', ['api', `repos/bcgov/hets/compare/master...${revision}`]));
  if (!['behind', 'identical'].includes(comparison.status)) throw new Error('Only commits already merged into master can be promoted beyond dev');
}

export function main() {
  const [command, tag, environment] = process.argv.slice(2);
  if (command !== 'resolve' || !['dev', 'test', 'uat', 'prod'].includes(environment)) {
    throw new Error('Usage: node .github/scripts/images.mjs resolve <tag> dev|test|uat|prod');
  }
  const release = resolveImages(expandTag(tag));
  if (environment !== 'dev') verifyMasterRevision(release.revision);
  process.stdout.write(`${JSON.stringify(release, null, 2)}\n`);
}

if (process.argv[1] === fileURLToPath(import.meta.url)) main();
