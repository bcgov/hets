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

module.exports = Object.freeze({
  KINDS: {
    LIST: 'List',
    BUILD: 'Build',
    BUILD_CONFIG: 'BuildConfig',
    IMAGE_STREAM: 'ImageStream',
    IMAGE_STREAM_TAG: 'ImageStreamTag',
    IMAGE_STREAM_IMAGE: 'ImageStreamImage',
    DEPLOYMENT_CONFIG: 'DeploymentConfig',
  },
  ENV: {
    BUILD_HASH: '_BUILD_HASH',
  },
  LABELS: {
    TEMPLATE_HASH: 'template-hash',
    SOURCE_HASH: 'source-hash',
  },
  ANNOTATIONS: {
    TEMPLATE_HASH: 'template-hash',
    SOURCE_HASH: 'source-hash',
  },
  POD_PHASES: {
    PENDING: 'Pending',
    RUNNING: 'Running',
    SUCCEEDED: 'Succeeded',
    FAILED: 'Failed',
    UNKNOWN: 'Unknown',
  },
});
