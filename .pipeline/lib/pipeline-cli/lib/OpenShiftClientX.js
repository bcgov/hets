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

const { isArray } = Array;
const info = require('debug')('info:OpenShiftClient');
const trace = require('debug')('trace:OpenShiftClient');

const path = require('path');
const isPlainObject = require('lodash.isplainobject');
const fs = require('fs');
const OpenShiftClient = require('./OpenShiftClient');
const OpenShiftClientResult = require('./OpenShiftClientResult');
const OpenShiftStaticSelector = require('./OpenShiftStaticSelector');
const Transformers = require('./transformers');
const isOpenShiftList = require('./isOpenShiftList');
const util = require('./util');
const CONSTANTS = require('./constants');

const logger = { info, trace };

class OpenShiftClientX extends OpenShiftClient {
  constructor(options) {
    super(options);
    this.cache = new Map();
  }

  applyBestPractices(resources) {
    if (resources != null && isArray(resources)) {
      return this.applyBestPractices(this.wrapOpenShiftList(resources));
    }

    if (resources != null && !isOpenShiftList(resources)) {
      throw new Error('"resources" argument must be an array');
    }
    const transformers = new Transformers(this);
    resources.items.forEach(resource => {
      transformers.ENSURE_METADATA(resource);
      transformers.ADD_CHECKSUM_LABEL(resource);
      transformers.ENSURE_METADATA_NAMESPACE(resource, resources);
      transformers.REMOVE_BUILD_CONFIG_TRIGGERS(resource);
      transformers.ADD_SOURCE_HASH(resource);
    });
    return resources;
  }

  // eslint-disable-next-line class-methods-use-this
  getLabel(resource, name) {
    resource.metadata = resource.metadata || {}; // eslint-disable-line no-param-reassign
    resource.metadata.labels = resource.metadata.labels || {}; // eslint-disable-line no-param-reassign,max-len
    return resource.metadata.labels[name];
  }

  // eslint-disable-next-line class-methods-use-this
  setLabel(resource, name, value) {
    resource.metadata = resource.metadata || {}; // eslint-disable-line no-param-reassign
    resource.metadata.labels = resource.metadata.labels || {}; // eslint-disable-line no-param-reassign,max-len
    if (isPlainObject(name)) {
      Object.assign(resource.metadata.labels, name);
    } else {
      resource.metadata.labels[name] = value; // eslint-disable-line no-param-reassign
    }
    return resource;
  }

  // eslint-disable-next-line class-methods-use-this
  getAnnotation(resource, name) {
    resource.metadata = resource.metadata || {}; // eslint-disable-line no-param-reassign
    resource.metadata.annotations = resource.metadata.annotations || {}; // eslint-disable-line no-param-reassign,max-len
    return resource.metadata.annotations[name];
  }
  // TODO:

  // eslint-disable-next-line class-methods-use-this
  setAnnotation(resource, name, value) {
    resource.metadata = resource.metadata || {}; // eslint-disable-line no-param-reassign
    resource.metadata.annotations = resource.metadata.annotations || {}; // eslint-disable-line no-param-reassign, max-len
    if (isPlainObject(name)) {
      Object.assign(resource.metadata.annotations, name);
    } else {
      resource.metadata.annotations[name] = value; // eslint-disable-line no-param-reassign
    }
  }

  /**
   *
   * @param {array<Object>} resources The resources to be modified
   * @param {*} appName The name of the app.
   * @param {*} envName The name of the environment (e.g.: dev, test, prod)
   * @param {*} envId    The unique name for this environment (e.g.: dev-1, dev-2, test, prod)
   * @param {*} instance  The name of the instance of the app
   *                      (defaults to `{appName}-${envName}-${envId})
   */
  applyRecommendedLabels(resources, appName, envName, envId, instance) {
    if (resources != null && !isArray(resources)) {
      throw new Error('"resources" argument must be an array');
    }

    const commonLabels = { 'app-name': appName };
    const envLabels = {
      'env-name': envName,
      'env-id': envId,
      'github-repo': this.git.repository,
      'github-owner': this.git.owner,
    };
    const allLabels = {
      app:
        instance || `${commonLabels['app-name']}-${envLabels['env-name']}-${envLabels['env-id']}`,
    };

    Object.assign(allLabels, commonLabels, envLabels);
    const mostLabels = Object.assign({}, allLabels);
    delete mostLabels['env-id'];
    // Apply labels to the list itself
    // client.util.label(resources, allLabels)

    resources.forEach(item => {
      if (this.getLabel(item, 'shared') === 'true') {
        this.setLabel(item, commonLabels);
      } else {
        this.setLabel(item, allLabels);
      }
      if (item.kind === 'StatefulSet') {
        // Normalize labels to StatefulSet Selector, so that it only needs the statefulset name
        // eslint-disable-next-line no-param-reassign
        item.spec.selector = { matchLabels: { statefulset: item.metadata.name } };
        logger.info(
          `Setting StatefulSet/${item.metadata.name}.spec.selector to be ${JSON.stringify(
            item.spec.selector,
          )}`,
        );
        this.setLabel(item.spec.template, { statefulset: item.metadata.name });
        if (item.spec.volumeClaimTemplates) {
          item.spec.volumeClaimTemplates.forEach(pvc => {
            // eslint-disable-next-line no-param-reassign
            pvc.metadata.labels = Object.assign({ statefulset: item.metadata.name }, mostLabels);
          });
        }
      } else if (item.kind === 'DeploymentConfig') {
        // eslint-disable-next-line max-len
        // Normalize labels to DeploymentConfig Selector, so that it only needs the deploymentConfig name
        // eslint-disable-next-line no-param-reassign
        item.spec.selector = { deploymentConfig: item.metadata.name };
        logger.info(
          `Setting DeploymentConfig/${item.metadata.name}.spec.selector to be ${JSON.stringify(
            item.spec.selector,
          )}`,
        );
        this.setLabel(item.spec.template, { deploymentConfig: item.metadata.name });
      }
    });

    return resources;
  }

  // eslint-disable-next-line class-methods-use-this
  copyRecommendedLabels(source, target) {
    ['app', 'app-name', 'env-name', 'env-id', 'github-repo', 'github-owner'].forEach(labelName => {
      if (source[labelName] != null) {
        target[labelName] = source[labelName]; // eslint-disable-line no-param-reassign
      }
    });
  }

  fetchSecretsAndConfigMaps(resources) {
    if (resources != null && !isArray(resources)) {
      throw new Error('"resources" argument must be an array');
    }

    for (let i = 0; i < resources.length; i += 1) {
      const resource = resources[i];
      if (resource.kind === 'Secret' || resource.kind === 'ConfigMap') {
        const refName = this.getAnnotation(resource, 'as-copy-of');
        if (refName != null) {
          const refResource = this.object(`${resource.kind}/${refName}`);
          resource.data = refResource.data;
          const tmpStringData = resource.stringData || {};
          resource.stringData = {};
          if (resource.kind === 'Secret' && tmpStringData['metadata.name'] != null) {
            resource.stringData['metadata.name'] = resource.metadata.name;
          }
          const preserveFields = this.getAnnotation(resource, 'as-copy-of/preserve');
          if (resource.kind === 'Secret' && preserveFields) {
            const existingResource = this.object(
              `${resource.kind}/${resource.metadata.name}`,
              { 'ignore-not-found': 'true' } // eslint-disable-line prettier/prettier
            );
            if (existingResource != null) {
              resource.data[preserveFields] = existingResource.data[preserveFields];
            }
          }
        }
      } else if (resource.kind === 'Route') {
        const refName = this.getAnnotation(resource, 'tls/secretName');
        if (refName != null) {
          const refResource = this.object(`${resource.kind}/${refName}`);
          const refData = refResource.data;
          Object.keys(refData).forEach(prop => {
            refData[prop] = Buffer.from(refData[prop], 'base64').toString('ascii');
          });
          resource.spec.tls = resource.spec.tls || {};
          Object.assign(resource.spec.tls, refData);
        }
      }
    }
    return resources;
  }

  _setCache(resource) {
    if (isArray(resource)) {
      const entries = [];
      for (let i = 0; i < resource.length; i += 1) {
        entries.push(this._setCache(resource[i]));
      }
      return entries;
    }
    const resourceFullName = util.fullName(resource);
    const entry = { item: resource, fullName: resourceFullName, name: util.name(resource) };
    this.cache.set(resourceFullName, entry);
    return entry;
  }

  _getCache(name) {
    const _names = []; // eslint-disable-line no-underscore-dangle
    const entries = [];
    const missing = [];

    if (isArray(name)) {
      _names.push(...name);
    } else {
      _names.push(name);
    }

    // look for missing resources from cache
    for (let i = 0; i < _names.length; i += 1) {
      const _name = _names[i]; // eslint-disable-line no-underscore-dangle
      const _parsed = util.parseName(_name, this.namespace()); // eslint-disable-line no-underscore-dangle,max-len
      const _full = util.fullName(_parsed); // eslint-disable-line no-underscore-dangle
      const entry = this.cache.get(_full);
      if (entry == null) {
        missing.push(_full);
      }
    }

    // fetch missing resources
    if (missing.length > 0) {
      const objects = this.objects(missing);
      this._setCache(objects);
    }

    // populate entries
    for (let i = 0; i < _names.length; i += 1) {
      const _name = _names[i]; // eslint-disable-line no-underscore-dangle
      const _parsed = util.parseName(_name, this.namespace()); // eslint-disable-line no-underscore-dangle,max-len
      const _full = util.fullName(_parsed); // eslint-disable-line no-underscore-dangle
      const entry = this.cache.get(_full);
      if (entry == null) throw new Error(`Missing object:${_name}`);
      entries.push(entry);
    }
    return entries;
  }

  getBuildStatus(buildCacheEntry) {
    if (!buildCacheEntry || !buildCacheEntry.item) {
      return undefined;
    }
    return this.cache.get(util.fullName(buildCacheEntry.item));
  }

  // Deprecated in favour of processBuidTemplate()
  /*
  processForBuild(template, args, name, envId) {
    const objects = this.process(template, args);
    this.applyBestPractices(objects);
    this.applyRecommendedLabels(objects, name, 'build', envId);

    return objects;
  }
  */

  /**
   * @param {*} buildConfig
   * @returns {string}  the name of the 'Build' object
   */
  startBuildIfNeeded(buildConfig) {
    const tmpfile = `/tmp/${util.hashObject(buildConfig)}.tar`;
    const args = { wait: 'true' };
    const hashData = {
      source: buildConfig.metadata.labels[CONSTANTS.LABELS.SOURCE_HASH],
      images: [],
      buildConfig: buildConfig.metadata.labels[CONSTANTS.LABELS.TEMPLATE_HASH],
    };
    const contextDir = buildConfig.spec.source.contextDir || '';

    if (buildConfig.spec.source.type === 'Binary') {
      if (fs.existsSync(tmpfile)) {
        fs.unlinkSync(tmpfile);
      }
      const procArgs = ['-chf', tmpfile, buildConfig.spec.source.contextDir];
      const procOptions = { cwd: this.cwd(), encoding: 'utf-8' };
      util.execSync('tar', procArgs, procOptions);
      Object.assign(args, { 'from-archive': tmpfile });
      hashData.source = util.hashDirectory(path.join(this.cwd(), contextDir));
    } else if (
      // eslint-disable-next-line max-len,prettier/prettier
      buildConfig.spec.source.type === 'Dockerfile' && buildConfig.spec.strategy.type === 'Docker'
    ) {
      hashData.source = util.hashObject(buildConfig.spec.source);
    } else {
      hashData.source = util
        .execSync('git', ['rev-parse', `HEAD:${contextDir}`], {
          cwd: this.cwd(),
          encoding: 'utf-8',
        })
        .stdout.trim();
      if (this.options['dev-mode'] === 'true') {
        Object.assign(args, { 'from-dir': this.cwd() });
      }
    }

    util.getBuildConfigInputImages(buildConfig).forEach(sourceImage => {
      if (sourceImage.kind === CONSTANTS.KINDS.IMAGE_STREAM_TAG) {
        const imageName = this.object(util.name(sourceImage), {
          namespace: sourceImage.namespace || this.namespace(),
          output: 'jsonpath={.image.metadata.name}',
        });
        const imageStreamImageName = `${sourceImage.name.split(':')[0]}@${imageName}`;
        logger.info(`Rewriting reference from '${sourceImage.kind}/${sourceImage.name}' to '${CONSTANTS.KINDS.IMAGE_STREAM_IMAGE}/${imageStreamImageName}'`) // eslint-disable-line
        sourceImage.kind = CONSTANTS.KINDS.IMAGE_STREAM_IMAGE; // eslint-disable-line no-param-reassign,max-len
        sourceImage.name = imageStreamImageName; // eslint-disable-line no-param-reassign
      }
      hashData.images.push(sourceImage);
    });

    const env = {};
    const buildHash = util.hashObject(hashData);
    env[CONSTANTS.ENV.BUILD_HASH] = buildHash;
    logger.trace(`${util.fullName(buildConfig)} > hashData: ${hashData}`);

    const outputTo = buildConfig.spec.output.to;
    if (outputTo.kind !== CONSTANTS.KINDS.IMAGE_STREAM_TAG) {
      throw new Error(`Expected kind=${CONSTANTS.KINDS.IMAGE_STREAM_TAG}, but found kind=${outputTo.kind} for ${util.fullName(buildConfig)}.spec.output.to`); // eslint-disable-line prettier/prettier
    }
    const outputImageStream = this.object(`${CONSTANTS.KINDS.IMAGE_STREAM}/${outputTo.name.split(':')[0]}`); // eslint-disable-line prettier/prettier
    const tags = (outputImageStream.status || {}).tags || [];
    let foundImageStreamImage = null;

    while (tags.length > 0) {
      const tag = tags.shift();
      if (!foundImageStreamImage) {
        const resources = tag.items.map(image => {
          return `${CONSTANTS.KINDS.IMAGE_STREAM_IMAGE}/${outputTo.name.split(':')[0]}@${image.image}`; // eslint-disable-line prettier/prettier
        });
        const images = this.objects(resources);
        images.forEach((ocImageStreamImage) => { // eslint-disable-line no-loop-func,prettier/prettier,max-len
          const sourceBuild = { kind: CONSTANTS.KINDS.BUILD, metadata: {} };
          ocImageStreamImage.image.dockerImageMetadata.Config.Env.forEach(envLine => {
            if (envLine === `${CONSTANTS.ENV.BUILD_HASH}=${buildHash}`) {
              foundImageStreamImage = ocImageStreamImage;
            } else if (envLine.startsWith('OPENSHIFT_BUILD_NAME=')) {
              sourceBuild.metadata.name = envLine.split('=')[1]; // eslint-disable-line prefer-destructuring
            } else if (envLine.startsWith('OPENSHIFT_BUILD_NAMESPACE=')) {
              sourceBuild.metadata.namespace = envLine.split('=')[1]; // eslint-disable-line prefer-destructuring
            }
          });
        });
      }
    }

    if (!foundImageStreamImage) {
      // eslint-disable-next-line no-console, prettier/prettier,max-len
      console.log('Starting new build for ', util.name(buildConfig));
      this._action(
        this.buildCommonArgs(
          'set',
          ['env', util.name(buildConfig)],
          { env, overwrite: 'true' },
          { namespace: buildConfig.metadata.namespace || this.namespace() },
        ),
      );
      return super.startBuild(`${util.fullName(buildConfig)}`, args);
    }
    // If image already exists, reuse it
    // eslint-disable-next-line no-console, prettier/prettier,max-len
    console.log('Re-using image ', util.fullName(foundImageStreamImage), 'for build ', util.name(buildConfig));
    this.tag([foundImageStreamImage.metadata.name, buildConfig.spec.output.to.name]);
    return new OpenShiftStaticSelector(this, [`${util.fullName(foundImageStreamImage)}`]);
  }

  importImageStreams(objects, targetImageTag, sourceNamespace, sourceImageTag) {
    for (let i = 0; i < objects.length; i += 1) {
      const item = objects[i];
      if (util.normalizeKind(item.kind) === 'imagestream.image.openshift.io') {
        this.raw(
          'tag',
          [
            `${sourceNamespace}/${item.metadata.name}:${sourceImageTag}`,
            `${item.metadata.name}:${targetImageTag}`,
          ],
          { 'reference-policy': 'local', namespace: item.metadata.namespace },
        );
        this.waitForImageStreamTag(`${item.metadata.name}:${targetImageTag}`);
      }
    }
    return objects;
  }

  async pickNextBuilds(builds, buildConfigs) {
    // var buildConfigs = _buildConfigs.slice()
    // const maxLoopCount = buildConfigs.length * 2
    let currentBuildConfigEntry = null;
    // var currentLoopCount = 0
    const promises = [];
    let head = undefined; // eslint-disable-line no-undef-init
    logger.trace(`>pickNextBuilds from ${buildConfigs.length} buildConfigs`);
    while ((currentBuildConfigEntry = buildConfigs.shift()) !== undefined) { // eslint-disable-line no-cond-assign, max-len, prettier/prettier
      if (head === undefined) {
        head = currentBuildConfigEntry;
      } else if (head === currentBuildConfigEntry) {
        buildConfigs.push(currentBuildConfigEntry);
        break;
      }
      const currentBuildConfig = currentBuildConfigEntry.item;
      const buildConfigFullName = util.fullName(currentBuildConfig);
      const dependencies = currentBuildConfigEntry.dependencies; // eslint-disable-line prefer-destructuring,max-len
      let resolved = true;
      // logger.trace(`Trying to queue ${buildConfigFullName}`)
      for (let i = 0; i < dependencies.length; i += 1) {
        const parentBuildConfigEntry = dependencies[i].buildConfigEntry;
        logger.trace(`${buildConfigFullName}  needs ${util.fullName(dependencies[i].item)}`);
        if (parentBuildConfigEntry) {
          logger.trace(`${buildConfigFullName}  needs ${util.fullName(parentBuildConfigEntry.item)}`); // eslint-disable-line prettier/prettier
          // var parentBuildConfig = parentBuildConfigEntry.item
          if (!parentBuildConfigEntry.imageStreamImageEntry) {
            const parentBuildEntry = parentBuildConfigEntry.buildEntry;
            const buildStatus = this.getBuildStatus(parentBuildEntry);
            if (buildStatus === undefined) {
              resolved = false;
              break;
            }
          }
        }
      }
      // dependencies have been resolved/satisfied
      if (resolved) {
        logger.trace(`Queuing ${buildConfigFullName}`);
        const self = this;
        const _startBuild = this.startBuildIfNeeded.bind(self);
        const _bcCacheEntry = currentBuildConfigEntry;

        promises.push(
          Promise.resolve(currentBuildConfig)
            .then(() => {
              return _startBuild(currentBuildConfig);
            })
            .then(build => {
              const _names = build.identifiers();
              // eslint-disable-next-line prefer-destructuring
              _bcCacheEntry.buildEntry = self._setCache(self.objects(_names))[0];
              if (build != null) {
                builds.push(..._names);
              }
            }),
        );
        if (head === currentBuildConfigEntry) {
          head = undefined;
        }
      } else {
        buildConfigs.push(currentBuildConfigEntry);
        logger.trace(`Delaying ${buildConfigFullName}`);
        // logger.trace(`buildConfigs.length =  ${buildConfigs.length}`)
      }
    } // end while

    let p = Promise.all(promises);
    // logger.trace(`buildConfigs.length =  ${buildConfigs.length}`)
    if (buildConfigs.length > 0) {
      const pickNextBuilds = this.pickNextBuilds.bind(this);
      p = p.then(() => {
        return pickNextBuilds(builds, buildConfigs);
      });
    }
    return p.catch(error => {
      throw error;
    });
  }

  async startBuild(resources) {
    logger.info('>startBuilds');
    // var cache = new Map()
    const buildConfigs = this._setCache(this.objects(resources));

    // try{
    buildConfigs.forEach(entry => {
      const bc = entry.item;
      const buildConfigFullName = util.fullName(bc);
      logger.trace(`Analyzing ${buildConfigFullName} - ${bc.metadata.namespace}`);
      const outputTo = bc.spec.output.to;
      if (outputTo) {
        if (outputTo.kind === CONSTANTS.KINDS.IMAGE_STREAM_TAG) {
          const name = outputTo.name.split(':');
          // eslint-disable-next-line prettier/prettier
          const imageStreamFullName = `${outputTo.namespace || bc.metadata.namespace}/${CONSTANTS.KINDS.IMAGE_STREAM}/${name[0]}`;
          const imageStreamCacheEntry = this._getCache(imageStreamFullName)[0];
          imageStreamCacheEntry.buildConfigEntry = entry;
        } else {
          // eslint-disable-next-line prettier/prettier
          throw new Error(`Expected '${CONSTANTS.KINDS.IMAGE_STREAM_TAG}' but found '${outputTo.kind}' in ${buildConfigFullName}.spec.output.to`);
        }
      }

      const dependencies = [];

      util.getBuildConfigInputImages(bc).forEach(sourceImage => {
        if (sourceImage.kind === CONSTANTS.KINDS.IMAGE_STREAM_TAG) {
          const name = sourceImage.name.split(':');
          // eslint-disable-next-line prettier/prettier
          const imageStreamFullName = `${sourceImage.namespace || bc.metadata.namespace}/${CONSTANTS.KINDS.IMAGE_STREAM}/${name[0]}`;
          dependencies.push(this._getCache(imageStreamFullName)[0]);
        } else {
          // eslint-disable-next-line prettier/prettier
          throw new Error(`Expected '${CONSTANTS.KINDS.IMAGE_STREAM_TAG}' but found '${sourceImage.kind}' in  ${bc.metadata.kind}/${bc.metadata.name} - ${JSON.stringify(sourceImage)}`);
        }
      });
      entry.dependencies = dependencies; // eslint-disable-line no-param-reassign
    });

    const builds = [];
    return this.pickNextBuilds(builds, buildConfigs).then(() => {
      return builds;
    });
  }

  processDeploymentTemplate(template, templateArgs) {
    const objects = this.process(template, templateArgs);
    this.applyBestPractices(objects);
    return objects;
  }

  processBuidTemplate(template, templateArgs) {
    const objects = this.process(template, templateArgs);
    this.applyBestPractices(objects);
    return objects;
  }

  async applyAndBuild(objects) {
    this.fetchSecretsAndConfigMaps(objects);
    const applyResult = this.apply(objects);

    return applyResult
      .narrow('bc')
      .startBuild()
      .catch(e => {
        console.log(e.stack); // eslint-disable-line no-console
        process.exit(1);
      });
  }

  async applyAndDeploy(resources, appName) {
    this.fetchSecretsAndConfigMaps(resources);
    // TODO: When there is an HorizontalPodAutoscaler,
    //   we need to update the dc.replicas to match hpa.status.desiredReplicas (existing)
    //   or hpa.spec.minReplicas (new)
    const existingDC = this.raw('get', ['dc'], {
      selector: `app=${appName}`,
      output: 'template={{range .items}}{{.metadata.name}}{{"\\t"}}{{.spec.replicas}}{{"\\t"}}{{.status.latestVersion}}{{"\\n"}}{{end}}' // eslint-disable-line prettier/prettier
    });
    //
    this.apply(resources);

    const newDCs = this.raw('get', ['dc'], {
      selector: `app=${appName}`,
      output: 'template={{range .items}}{{.metadata.name}}{{"\\t"}}{{.spec.replicas}}{{"\\t"}}{{.status.latestVersion}}{{"\\n"}}{{end}}' // eslint-disable-line prettier/prettier
    });

    const proc = this.rawAsync('get', 'dc', {
      selector: `app=${appName}`,
      watch: 'true',
    });

    return new Promise(resolve => {
      if (existingDC.stdout !== newDCs.stdout) {
        OpenShiftClientResult.waitForDeployment(proc);
      } else {
        proc.kill('SIGTERM');
      }
      proc.on('exit', () => {
        return resolve();
      });
    });
  }
}

module.exports = OpenShiftClientX;
