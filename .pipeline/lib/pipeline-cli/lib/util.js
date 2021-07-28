//
// pipeline-cli
//
// Copyright © 2019 Province of British Columbia
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

'use strict';

const crypto = require('crypto');
const isString = require('lodash.isstring');
const isPlainObject = require('lodash.isplainobject');
const fs = require('fs');
const path = require('path');
const { spawnSync } = require('child_process');
const debug = require('debug');

const { isArray } = Array;

const nameRegexPattern = '^(?:([^/]+?)/)?(([^/]+?)/(.*?))$';

const logger = {
  info: debug('info:OpenShiftClient'),
  trace: debug('trace:OpenShiftClient'),
};

function hashString(itemAsString) {
  const shasum = crypto.createHash('sha1');
  // var itemAsString = JSON.stringify(resource)
  shasum.update(`blob ${itemAsString.length + 1}\0${itemAsString}\n`);

  return shasum.digest('hex');
}

const hashObject = resource => {
  // var shasum = crypto.createHash('sha1');
  const itemAsString = JSON.stringify(resource);
  // shasum.update(`blob ${itemAsString.length + 1}\0${itemAsString}\n`);
  return hashString(itemAsString);
};

const isUrl = string => {
  const protocolAndDomainRE = /^(?:\w+)+:\/\/(\S+)$/;
  if (!isString(string)) return false;
  const match = string.match(protocolAndDomainRE);
  if (!match) return false;
  return true;
};

// const fullName = (resource) => `${resource.kind}/${resource.metadata.name}`;
function getBuildConfigStrategy(bc) {
  return bc.spec.strategy.sourceStrategy || bc.spec.strategy.dockerStrategy;
}

const getBuildConfigInputImages = bc => {
  const result = [];
  const buildStrategy = getBuildConfigStrategy(bc);

  if (buildStrategy.from) {
    result.push(buildStrategy.from);
  }

  if ((bc.spec.source || {}).images) {
    const sourceImages = bc.spec.source.images;
    sourceImages.forEach(sourceImage => {
      result.push(sourceImage.from);
    });
  }

  return result;
};

const normalizeKind = kind => {
  if (kind === 'ImageStream') {
    return 'imagestream.image.openshift.io';
  }
  if (kind === 'BuildConfig') {
    return 'buildconfig.build.openshift.io';
  }
  return kind;
};

/* eslint no-underscore-dangle: 0 */
const _hashDirectory = dir => {
  const result = [];
  const items = fs.readdirSync(dir).sort();

  items.forEach(item => {
    const fullpath = path.join(dir, item);
    const stat = fs.statSync(fullpath);
    if (stat.isDirectory()) {
      result.push(..._hashDirectory(fullpath));
    } else {
      result.push(hashString(fs.readFileSync(fullpath)));
    }
  });
  return result;
};

const hashDirectory = dir => {
  const items = _hashDirectory(dir);
  return hashObject(items);
};

function unsafeExecSync(...args) {
  const ret = spawnSync(...args);
  logger.trace([args[0]].concat(args[1]).join(' '), ' - ', args[2], ' > ', ret.status);
  return ret;
}

function execSync(...args) {
  const ret = unsafeExecSync(...args);
  if (ret.status !== 0) {
    throw new Error(
      `Failed running '${args[0]} ${args[1].join(' ')}' as it returned ${ret.status}`,
    );
  }
  return ret;
}
function parseArgumentsFromArray(...argv) {
  const git = {};
  const options = { git };

  argv.forEach(value => {
    if (value.startsWith('--')) {
      // eslint-disable-next-line no-param-reassign
      value = value.substr(2);
      const sep = value.indexOf('=');
      const argName = value.substring(0, sep);
      const argValue = value.substring(sep + 1);
      if (argName.startsWith('git.')) {
        const ctxPath = argName.substr(4).split('.');
        let ctx = git;
        ctxPath.forEach((key, index) => {
          if (index === ctxPath.length - 1) {
            ctx[key] = argValue;
          } else {
            ctx[key] = ctx[key] || {};
            ctx = ctx[key];
          }
        });
      } else {
        options[argName] = argValue;
      }
    }
  });
  return applyArgumentsDefaults(options);
}

function applyArgumentsDefaults(options) {
  options.git = options.git || {};
  const git = options.git;

  if (git.dir == null) {
    // eslint-disable-next-line prettier/prettier
    git.dir = execSync('git', ['rev-parse', '--show-toplevel'], { encoding: 'utf-8' }).stdout.trim();
  }

  if (options.cwd == null) {
    options.cwd = git.dir;
  }

  git.branch = git.branch || {};

  if (git.branch.name == null) {
    // eslint-disable-next-line prettier/prettier
    git.branch.name = execSync('git', ['rev-parse', '--abbrev-ref', 'HEAD'], { encoding: 'utf-8' }).stdout.trim();
  }

  if (git.branch.remote == null) {
    // eslint-disable-next-line prettier/prettier
    const gitConfigBranchRemote = unsafeExecSync('git', ['config', `branch.${git.branch.name}.remote`], { encoding: 'utf-8' });
    if (gitConfigBranchRemote.status !== 0) {
      // Default to "origin"
      git.branch.remote = 'origin';
    } else {
      git.branch.remote = gitConfigBranchRemote.stdout.trim();
    }
  }

  if (git.url == null) {
    // eslint-disable-next-line prettier/prettier
    git.url = execSync('git', ['config', '--get', `remote.${git.branch.remote}.url`], { encoding: 'utf-8' }).stdout.trim();
  }

  git.uri = git.url;
  if (git.http_url == null) {
    git.http_url = git.url.replace(
      /((https:\/\/github\.com\/)|(git@github.com:))([^/]+)\/(.*)/,
      'https://github.com/$4/$5', // eslint-disable-line comma-dangle
    );
  }

  if (git.http_url.startsWith('https://github.com') && !git.branch.merge) {
    git.branch.merge = `refs/pull/${git.pull_request}/head`;
  }

  if (git.branch.merge == null) {
    // eslint-disable-next-line prettier/prettier
    git.branch.merge = execSync('git', ['config', `branch.${git.branch.name}.merge`], { encoding: 'utf-8' }).stdout.trim();
  }

  if (git.owner == null) {
    git.owner = git.url.replace(
      /((https:\/\/github\.com\/)|(git@github.com:))([^/]+)\/(.*)/,
      '$4', // eslint-disable-line comma-dangle
    );
  }
  if (git.repository == null) {
    git.repository = git.url.replace(
      /((https:\/\/github\.com\/)|(git@github.com:))([^/]+)\/([^\.]+)\.git/, // eslint-disable-line no-useless-escape
      '$5', // eslint-disable-line comma-dangle
    );
  }

  if (options.pr) {
    git.pull_request = options.pr;
  }
  // when --ref flag is used
  if (options.ref) {
    git.ref = options.ref;
  }

  if (!git.ref) {
    if (git.pull_request) {
      git.ref = `refs/pull/${git.pull_request}/head`;
    } else {
      git.ref = git.branch.merge;
    }
  }
  git.branch_ref = git.ref;
  return options;
}

module.exports = {
  hashString,
  hashObject,
  isUrl,
  isString,
  isArray,
  isPlainObject,
  // TODO: shortName: (resource) => { return resource.metadata.name },
  parseName: (name, defaultNamespace) => {
    const result = new RegExp(nameRegexPattern, 'g').exec(name);
    return {
      namespace: result[1] || defaultNamespace,
      kind: result[3],
      name: result[4],
    };
  },
  name: resource => {
    if (resource.kind && resource.name) return `${normalizeKind(resource.kind)}/${resource.name}`;
    return `${normalizeKind(resource.kind)}/${resource.metadata.name}`;
  },
  fullName: resource => {
    if (resource.namespace && resource.kind && resource.name) {
      return `${resource.namespace}/${normalizeKind(resource.kind)}/${resource.name}`;
    }
    return `${resource.metadata.namespace}/${normalizeKind(resource.kind)}/${
      resource.metadata.name
    }`;
  },
  normalizeKind,
  normalizeName: name => {
    if (name.startsWith('ImageStream/')) {
      return `imagestream.image.openshift.io/${name.substr('ImageStream/'.length)}`;
    }
    if (name.startsWith('BuildConfig/')) {
      return `buildconfig.build.openshift.io/${name.substr('BuildConfig/'.length)}`;
    }
    return name;
  },
  getBuildConfigInputImages,
  getBuildConfigStrategy,
  hashDirectory,
  parseArguments: () => {
    return parseArgumentsFromArray(...process.argv.slice(2));
  },
  parseArgumentsFromArray,
  applyArgumentsDefaults,
  execSync,
  unsafeExecSync,
};
