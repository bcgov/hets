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

// const OpenShiftClient = require('./OpenShiftClient');
const isEmpty = require('lodash.isempty');

module.exports = class OpenShiftClientResult {
  constructor(client) {
    this.client = client;
  }

  /**
   * parses the string output from an oc get object --watch command
   * this function in particular returns the field names as identified from the initial
   * output from ocp
   */
  static parseGetObjectFields(stdOut) {
    const [fieldNames] = stdOut.split('\n');

    const names = fieldNames.replace(/ {2,}/g, '|{}|').split('|{}|');

    return names.map(n => {
      return n.toLowerCase().replace(/ +/g, '_');
    });
  }

  /**
   * parses the string output from an oc get object --watch command
   * this function in particular returns the data values from subsequent returns from ocp
   */
  static parseGetObjectValues(stdOut) {
    if (!stdOut) return [];

    const entries = stdOut
      .trim()
      .replace(/ {2,}/g, '|{}|')
      .split('|{}|');
    return entries;
  }

  /**
   * waits for a deployment from the oc get dc --wait command
   * it will kill the process once the desired replicas and current replicas are equal
   * @param {Process} proc the process command that was spawned for oc get blah
   *
   * usage:
   * const proc = self.rawAsync('get', 'dc', {
      selector: `app=${appName}`,
      watch: 'true'
     });

     OpenshiftClientResult.waitForDeployment(proc);
     // proc will kill when replicas match desired amount

     proc.on('exit', () => do something here)
   */
  static waitForDeployment(proc) {
    let deployment = {};
    // eslint-disable-next-line no-console
    console.log(`WAITING FOR DEPLOYMENT:
      if a deployment fails, it will not throw. 
      Ensure you are setting appropriate timeouts while using this implementation!
    `);

    proc.stdout.on('data', data => {
      let stdOut = data.toString();
      if (isEmpty(deployment)) {
        const keys = OpenShiftClientResult.parseGetObjectFields(stdOut);
        deployment = keys.reduce((obj, key) => {
          // eslint-disable-next-line no-param-reassign
          obj[key] = '';
          return obj;
        }, {});

        // we need reference to keys to assign deployment values later
        // eslint-disable-next-line no-underscore-dangle
        deployment._keys = keys;

        // the first stdout instance contains fields and values and so we remove field names now
        // eslint-disable-next-line no-unused-vars
        const [names, entries] = stdOut.split('\n');
        stdOut = entries;
      }

      const deployData = OpenShiftClientResult.parseGetObjectValues(stdOut);

      console.log(deployData);

      deployData.forEach((d, index) => {
        // eslint-disable-next-line no-underscore-dangle
        deployment[deployment._keys[index]] = d;
      });

      if (deployment.desired === deployment.current && deployment.desired !== '') {
        proc.kill('SIGTERM');
      }
    });
  }
};
