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

const path = require('path');
const debug = require('debug');
const isPlainObject = require('lodash.isplainobject');
const isString = require('lodash.isstring');
const { spawn, spawnSync } = require('child_process');
const OpenShiftResourceSelector = require('./OpenShiftResourceSelector');
const OpenShiftStaticSelector = require('./OpenShiftStaticSelector');
const util = require('./util');

const { isArray } = Array;

const logger = {
  info: debug('info:OpenShiftClient'),
  trace: debug('trace:OpenShiftClient'),
};

function appendCommandArg(prefix, item, result) {
  if (item instanceof Array) {
    item.forEach(subitem => {
      return appendCommandArg(prefix, subitem, result);
    });
  } else if (!(item instanceof String || typeof item === 'string') && item instanceof Object) {
    Object.keys(item).forEach(prop => {
      appendCommandArg(`${prefix}=${prop}`, item[prop], result);
    });
  } else {
    result.push(`${prefix}=${item}`);
  }
}

/**
 * https://github.com/openshift/jenkins-client-plugin/blob/master/src/main/resources/com/openshift/jenkins/plugins/OpenShiftDSL.groovy
 */

module.exports = class OpenShiftClient {
  constructor(options) {
    this.globalArgs = {};
    this.options = {};

    if (options) {
      this.options = Object.assign(this.options, options);
      if (options.namespace) this.globalArgs.namespace = options.namespace;
      if (options.cwd) this._cwd = options.cwd;
      // TODO:read/override from process.arguments
    }
    this.git = this.options.git;
  }

  namespace(ns) {
    if (typeof ns !== 'undefined' && ns != null) {
      this.globalArgs.namespace = ns;
    }
    return this.globalArgs.namespace;
  }

  cwd() {
    return this._cwd;
  }

  /**
   * @param {string} verb
   * @param {string|Object} verbArgs
   * @param {Object} userArgs
   * @param {Object} overrideArgs
   */
  buildCommonArgs(verb, verbArgs, userArgs, overrideArgs) {
    if (userArgs != null && !isPlainObject(userArgs)) {
      throw new Error('Expected "userArgs" to be plain object');
    }

    if (overrideArgs != null && !isPlainObject(overrideArgs)) {
      throw new Error('Expected "userArgs" to be plain object');
    }

    const _args = {};
    Object.assign(_args, this.globalArgs);
    if (userArgs != null) {
      Object.assign(_args, userArgs);
    }
    if (isPlainObject(verbArgs) && verbArgs.namespace) {
      _args.namespace = verbArgs.namespace;
      delete verbArgs.namespace; // eslint-disable-line no-param-reassign
    }

    if (overrideArgs != null) {
      Object.assign(_args, overrideArgs);
    }

    const args = [];
    if (_args.namespace) {
      args.push(`--namespace=${_args.namespace}`);
      delete _args.namespace;
    }
    args.push(verb);
    if (isArray(verbArgs)) {
      args.push(...verbArgs);
    } else if (isPlainObject(verbArgs)) {
      args.push(...this.toCommandArgsArray(verbArgs));
    } else if (isString(verbArgs)) {
      args.push(verbArgs);
    }

    args.push(...this.toCommandArgsArray(_args));
    return args;
  }

  _actionAsync(args, input) {
    const self = this;
    // console.log(`> ${JSON.stringify(args)}`)
    logger.trace('>', ['oc'].concat(args).join(' '));
    // logger.trace('ocSpawn', ['oc'].concat(cmdArgs).join(' '))
    const _options = { encoding: 'utf-8' };
    if (self.cwd()) {
      _options.cwd = self.cwd();
    }
    // const startTime = process.hrtime();
    if (input != null) {
      _options.input = input;
    }
    const proc = spawn('oc', args, _options);

    return proc;
  }

  _action(args, input) {
    const proc = this._rawAction(args, input);
    if (proc.status !== 0) {
      throw new Error(`command: ${['oc'].concat(args).join(' ')}\nstderr:${proc.stderr}`);
    }
    return proc;
  }

  _rawAction(args, input) {
    const self = this;
    // console.log(`> ${JSON.stringify(args)}`)
    logger.trace('>', ['oc'].concat(args).join(' '));
    // logger.trace('ocSpawn', ['oc'].concat(cmdArgs).join(' '))
    const _options = { encoding: 'utf-8' };
    if (self.cwd()) {
      _options.cwd = self.cwd();
    }
    const startTime = process.hrtime();
    if (input != null) {
      _options.input = input;
    }
    const proc = spawnSync('oc', args, _options);
    const duration = process.hrtime(startTime);
    logger.info(['oc'].concat(args).join(' '), ` # (${proc.status}) [${duration[0]}s]`);

    return proc;
  }

  splitNamesUsingArgs(string, args) {
    const namespace = args
      .find(item => {
        return item.startsWith('--namespace=');
      })
      .substr('--namespace='.length);
    return this.splitNames(string, namespace);
  }

  // eslint-disable-next-line class-methods-use-this
  splitNames(string, namespace) {
    const trimmed = string.trim();
    if (trimmed.length > 0) {
      const names = trimmed.split(/\n/);
      if (names.length > 0 && namespace != null) {
        for (let i = 0; i < names.length; i += 1) {
          names[i] = `${namespace}/${names[i]}`;
        }
      }
      return names;
    }
    return [];
  }

  _actionReturningName(args) {
    const proc = this._action(args);
    const names = this.splitNamesUsingArgs(proc.stdout, args);
    return new OpenShiftStaticSelector(this, names);
  }

  _actionReturningName2(args) {
    const proc = this._rawAction(args);
    const names = this.splitNamesUsingArgs(proc.stdout, args);
    return new OpenShiftStaticSelector(this, names);
  }

  get(object, args) {
    return this.objectDefAction('get', object, Object.assign({ output: 'json' }, args || {}));
  }

  /**
   *
   * @param {string[]} args
   */
  raw(verb, verbArgs, userArgs) {
    const args = this.buildCommonArgs(verb, verbArgs, userArgs);
    return this._action(args);
  }

  rawAsync(verb, verbArgs, userArgs) {
    const args = this.buildCommonArgs(verb, verbArgs, userArgs);
    return this._actionAsync(args);
  }

  /**
   * Given a list of objects, return their names (namespace/name)
   * @param {} objects
   */
  // eslint-disable-next-line class-methods-use-this, no-unused-vars
  names(objects) {
    throw new Error('Not Implemented');
  }

  object(name, args) {
    return this.objects([name], args)[0];
  }

  objectOrNull(name, args) {
    const items = this.objects([name], args);
    if (items.length > 0) {
      return items[0];
    }
    return null;
  }

  objects(names, args) {
    const result = [];
    const namespaces = {};
    names.forEach(name => {
      const parsed = util.parseName(name);
      const namespace = parsed.namespace || this.namespace();
      namespaces[namespace] = namespaces[namespace] || [];
      namespaces[namespace].push(util.name(parsed));
    });

    Object.keys(namespaces).forEach(namespace => {
      const names2 = namespaces[namespace];
      const items = this.objectDefAction(
        'get',
        names2,
        Object.assign({ output: 'json', namespace }, args || {}), // eslint-disable-line comma-dangle
      );
      result.push(...items);
    });

    return result;
  }

  /**
   * returns (array)
   */
  // eslint-disable-next-line class-methods-use-this
  unwrapOpenShiftList(object) {
    const result = [];
    if (isPlainObject(object)) {
      if (object.kind !== 'List') {
        result.push(object);
      } else if (object.items) {
        result.push(...object.items);
      }
    } else {
      throw new Error('Not Implemented');
    }
    return result;
  }

  wrapOpenShiftList(object) {
    const list = this._emptyListModel();
    if (isArray(object)) {
      list.items.push(...object);
    } else {
      list.items.push(object);
    }
    return list;
  }

  // eslint-disable-next-line class-methods-use-this
  serializableMap(jsonString) {
    return JSON.parse(jsonString);
  }

  toNamesList(objectOrList) {
    if (isArray(objectOrList)) {
      const names = [];
      for (let i = 0; i < objectOrList.length; i += 1) {
        const item = objectOrList[i];
        names.push(`${item.kind}/${item.metadata.name}`);
      }
      return names;
    }
    if (isPlainObject(objectOrList)) {
      if (objectOrList.kind === 'List') {
        if (objectOrList.items != null) {
          return this.toNamesList(objectOrList.items);
        }
        return [];
      }
      return [`${objectOrList.kind}/${objectOrList.metadata.name}`];
    }
    throw new Error('Not Implemented');
  }
  // TODO: toSingleObject(){}

  /**
   * @returns {OpenShiftResourceSelector}
   * @param {String|String[]} kind
   * @param {String|Object} qualifier
   */
  selector(kind, qualifier) {
    return new OpenShiftResourceSelector(this, 'selector', kind, qualifier);
  }

  /**
   *
   * @param {string} template URL (http, https, or file), or template name
   * @param {Object} args
   * @returns {OpenShiftResourceSelector}
   *
   */
  process(template, args) {
    if (typeof template !== 'string') throw new Error('Expected string');
    if (util.isUrl(template)) {
      const proc = this._action(
        this.buildCommonArgs('process', ['-f', this.toFilePath(template)], args, {
          output: 'json',
        }), // eslint-disable-line comma-dangle
      );
      return this.unwrapOpenShiftList(this.serializableMap(proc.stdout));
    }
    throw new Error('Not Implemented');
  }

  objectDefAction(verb, object, userArgs) {
    if (!isString(object) && !isPlainObject(object) && !isArray(object)) {
      throw new Error('Expected string, plain object, or array');
    }
    if (verb === 'get' && userArgs != null && userArgs.output === 'json') {
      const list = this._emptyListModel();
      list.items = object;
      const args = this.buildCommonArgs(verb, object, userArgs, {});
      const proc = this._action(args);
      proc.stdout = proc.stdout.trim();
      if (proc.stdout == null || proc.stdout.length === 0) {
        return this.unwrapOpenShiftList(this._emptyListModel());
      }
      return this.unwrapOpenShiftList(JSON.parse(proc.stdout));
    } else if ( // eslint-disable-line no-else-return,prettier/prettier
      verb === 'get' && userArgs != null && userArgs.output != null && userArgs.output.startsWith('jsonpath') // eslint-disable-line prettier/prettier
    ) {
      const args = this.buildCommonArgs(verb, object, userArgs, {});
      const proc = this._action(args);
      return proc.stdout.trim().split('\n');
    } else if (verb === 'start-build') { // eslint-disable-line no-else-return,prettier/prettier
      const args = this.buildCommonArgs(verb, object, userArgs, { output: 'name' });
      logger.info(`Starting new build: ${args.join(' ')}`);
      return this._actionReturningName(args);
    } else if (verb === 'get' || verb === 'delete' || verb === 'start-build' || verb === 'process') { // eslint-disable-line no-else-return,prettier/prettier
      return this._actionReturningName(
        this.buildCommonArgs(verb, object, userArgs, { output: 'name' }), // eslint-disable-line comma-dangle
      );
    } else if ((verb === 'apply' || verb === 'create') && isArray(object)) {
      const list = this._emptyListModel();
      list.items = object;
      let ignoreExitStatus = false;
      if (userArgs && userArgs['ignore-exit-status'] != null) {
        ignoreExitStatus = userArgs['ignore-exit-status'];
        delete userArgs['ignore-exit-status']; // eslint-disable-line no-param-reassign
      }
      const args = this.buildCommonArgs(verb, ['-f', '-'], userArgs, { output: 'name' });
      let proc = null;
      if (ignoreExitStatus) {
        proc = this._rawAction(args, JSON.stringify(list));
      } else {
        proc = this._action(args, JSON.stringify(list));
      }
      const names = this.splitNamesUsingArgs(proc.stdout, args);
      return new OpenShiftStaticSelector(this, names);
    } else if (verb === 'tag' && isArray(object)) {
      // [0] is the source
      // [1+] is the targets
      const args = this.buildCommonArgs(verb, object, userArgs, {});
      this._action(args);
      return null;
    } else if ((verb === 'create' || verb === 'replace') && isString(object) && util.isUrl(object)) { // eslint-disable-line prettier/prettier
      if (userArgs['ignore-exit-status'] === true) {
        delete userArgs['ignore-exit-status']; // eslint-disable-line no-param-reassign
        return this._actionReturningName2(
          this.buildCommonArgs(verb, { filename: this.toFilePath(object) }, userArgs, { output: 'name' }) // eslint-disable-line prettier/prettier, comma-dangle
        );
      }
      return this._actionReturningName(this.buildCommonArgs(verb, { filename: this.toFilePath(object) }, userArgs, { output: 'name' })); // eslint-disable-line prettier/prettier
    } else if (verb === 'cancel-build') {
      return this._actionReturningName(this.buildCommonArgs(verb, object, userArgs));
    } else {
      throw new Error('Not Implemented');
    }
  }

  async startBuild(object, args) {
    if (isArray(object)) {
      const promises = [];
      for (let i = 0; i < object.length; i += 1) {
        const item = object[i];
        promises.push(
          Promise.resolve(item).then(result => {
            return this.startBuild(result, args);
          }), // eslint-disable-line comma-dangle
        );
      }
      const results = await Promise.all(promises);
      return results;
    } else if (isPlainObject(object)) { // eslint-disable-line no-else-return,prettier/prettier
      const _args = Object.assign({ namespace: object.metadata.namespace }, args);
      return this.objectDefAction('start-build', util.name(object), _args);
    } else if (isString(object)) {
      const parsed = util.parseName(object);
      return this.objectDefAction(
        'start-build',
        util.name(parsed),
        Object.assign({ namespace: parsed.namespace || this.namespace() }, args), // eslint-disable-line comma-dangle, max-len
      );
    }
    return null;
  }

  cancelBuild(object, args) {
    return this.objectDefAction('cancel-build', object, args);
  }

  // TODO: watch(){}
  create(object, args) {
    return this.objectDefAction('create', object, args);
  }

  createIfMissing(object, args) {
    return this.objectDefAction(
      'create',
      object,
      Object.assign({ 'ignore-exit-status': true }, args),
    );
  }

  waitForImageStreamTag(tag) {
    let istag = {};
    const start = process.hrtime();

    while (((istag.image || {}).metadata || {}).name == null) {
      const istags = this.objects([`ImageStreamTag/${tag}`], { 'ignore-not-found': 'true' });
      if (istags.length > 0) {
        istag = istags[0]; // eslint-disable-line prefer-destructuring
      }
      if (process.hrtime(start)[0] > 60) {
        throw new Error(`Timeout waiting for ImageStreamTag/${tag} to become available`);
      }
    }
  }

  apply(object, args) {
    const result = this.objectDefAction('apply', object, args);
    object.forEach(item => {
      if (item.kind === 'ImageStream') {
        ((item.spec || {}).tags || []).forEach(tag => {
          this.waitForImageStreamTag(`${item.metadata.name}:${tag.name}`);
        });
      }
    });
    return result;
  }

  replace(object, args) {
    return this.objectDefAction('replace', object, args);
  }

  delete(object, args) {
    return this.objectDefAction('delete', object, args);
  }

  // patch(){}

  /**
   *
   * @param {*} verb
   * @param {*} args
   */
  // eslint-disable-next-line class-methods-use-this
  simplePassthrough() {
    throw new Error('Not Implemented');
  }

  /**
   * Create and run a particular image, possibly replicated.
   * @param {*} args
   */
  run(args) {
    return this.simplePassthrough('run', args);
  }

  /**
   * Execute a command in a container.
   * @param {*} args
   */
  exec(args) {
    return this.simplePassthrough('exec', args);
  }

  rsh(args) {
    return this.simplePassthrough('rsh', args);
  }

  rsync(args) {
    return this.simplePassthrough('rsync', args);
  }

  /**
   *
   * @param {*} objects  An array where the first one ([0]) is the source tag.
   * @param {*} args
   */
  tag(objects, args) {
    return this.objectDefAction('tag', objects, args);
  }

  // Utilities
  // eslint-disable-next-line class-methods-use-this
  toFileUrl(str) {
    if (typeof str !== 'string') {
      throw new Error('Expected a string');
    }

    const pathName = path.resolve(str).replace(/\\/g, '/');

    return encodeURI(`file://${pathName}`);
  }

  // eslint-disable-next-line class-methods-use-this
  toFilePath(string) {
    if (string.startsWith('file://')) {
      return string.substr('file://'.length);
    }
    return string;
  }

  // eslint-disable-next-line class-methods-use-this
  toCommandArgsArray(args) {
    if (isArray(args)) return args;
    const result = [];
    Object.keys(args).forEach(prop => {
      const value = args[prop];
      if (value !== undefined) {
        appendCommandArg(`--${prop}`, value, result);
      }
    });
    return result;
  }

  // eslint-disable-next-line class-methods-use-this
  _emptyListModel() {
    return {
      apiVersion: 'v1',
      kind: 'List',
      metadata: {},
      items: [],
    };
  }
};
