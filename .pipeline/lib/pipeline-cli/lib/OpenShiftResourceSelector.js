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

const isPlainObject = require('lodash.isplainobject');
const isString = require('lodash.isstring');
const OpenShiftClientResult = require('./OpenShiftClientResult');

const { isArray } = Array;

// const OpenShiftStaticSelector = require('./OpenShiftStaticSelector')

module.exports = class OpenShiftResourceSelector extends OpenShiftClientResult {
  /**
   * @param {string} type 'selector', 'narrow', 'freeze'
   * @param {*} kind
   * @param {*} qualifier
   *
   * () selects all
   * (string) selects all of name
   * ("dc", "jenkins") selects a particular instance dc/jenkins
   * ("dc/jenkins") selects a particular instance dc/jenkins
   * (["dc/jenkins", "svc/jenkins"]) selects a particular list of resources
   * ("dc", { alabel: 'avalue' }) // Selects using label values
   */
  constructor(client, type, kindOrList, qualifier = undefined) {
    super(client);
    this.ids = undefined;
    this.kind = undefined;
    this.qualifier = undefined;
    this.labels = undefined;

    // if it is a list of qualified names
    if (isArray(kindOrList)) {
      this.ids = [];
      this.ids.push(...kindOrList);
    } else if (kindOrList.indexOf('/') >= 0) {
      this.ids = [];
      this.ids.push(kindOrList);
    } else {
      this.kind = kindOrList;
      if (isPlainObject(qualifier)) {
        this.labels = qualifier;
      } else if (isString(qualifier)) {
        this.qualifier = qualifier;
      }
    }
  }

  _isEmptyStatic() {
    return this.ids != null && this.ids.length === 0;
  }

  // TODO: asJson(){}
  // TODO: asYaml(){}

  // TODO: object(){}
  // TODO: objects(){}

  queryIdentifiers() {
    if (this._isEmptyStatic()) {
      return [];
    }
    const args = [this.kind];
    if (this.qualifier != null) {
      args.push(this.qualifier);
    }
    const selectors = this.client.toCommandArgsArray({ selector: this.labels });
    return this.client.objectDefAction('get', args.concat(selectors), null).identifiers();
  }

  identifiers() {
    if (this.ids) {
      return [].concat(this.ids);
    }
    return this.queryIdentifiers();
  }

  /**
   * return {String[]}
   */
  names() {
    const _identifiers = this.identifiers();
    const names = [];
    for (let i = 0; i < _identifiers.length; i += 1) {
      const name = _identifiers[i];
      names.push(name.substr(name.indexOf('/') + 1));
    }
    return names;
  }

  // TODO: selectionArgs(){}
  // TODO: queryNames(){ }

  /**
   * return {String}
   */
  // // eslint-disable-next-line class-methods-use-this
  /*
  name() {
    throw new Error('Not Implemented');
  }
  */
  delete(args) {
    return this.client.delete(this.names(), args);
  }

  // TODO: label(){}
  // TODO: annotate() {}
  // TODO: watch(){}
  async startBuild(args) {
    const _names = this.identifiers();
    return this.client.startBuild(_names, args);
  }

  cancelBuild(args) {
    return this.client.cancelBuild(this.names(), args);
  }

  narrow(kind) {
    const result = [];
    if (kind === 'bc') {
      const names = this.identifiers();
      for (let i = 0; i < names.length; i += 1) {
        const name = names[i];
        const kind2 = name.split('/')[1];
        // eslint-disable-next-line prettier/prettier
        if (kind2 === 'bc' || kind2 === 'buildconfig' || kind2 === 'buildconfig.build.openshift.io') {
          result.push(name);
        }
      }
    }
    return new OpenShiftResourceSelector(this.client, 'static', result);
  }
  // TODO: deploy(){}
  // TODO: freeze(){}
  // TODO: related(){}
  // TODO: union(){}
};
