"use strict";
const { OpenShiftClientX } = require("@bcgov/pipeline-cli");
const path = require("path");

const util = require("./utils");

module.exports = (settings) => {
  const phases = settings.phases;
  const options = settings.options;
  const phase = options.env;
  const changeId = phases[phase].changeId;
  const oc = new OpenShiftClientX(
    Object.assign({ namespace: phases[phase].namespace }, options)
  );

  const templatesLocalBaseUrl = oc.toFileUrl(
    path.resolve(__dirname, "../../openshift")
  );
  var objects = [];

  const dbSecret = util.getSecret(
    oc,
    phases[phase].namespace,
    `${phases[phase].name}-db-${phases[phase].phase}`
  );

  if (!dbSecret) {
    console.log("Adding Db postgresql secret");

    objects.push(
      ...oc.processDeploymentTemplate(
        `${templatesLocalBaseUrl}/secrets/db-postgresql-secrets.yaml`,
        {
          param: {
            PROJECT_NAME: `${phases[phase].name}`,
            NAME: `${phases[phase].name}-db`,
            SUFFIX: phases[phase].suffix,
            POSTGRESQL_USER: phases[phase].dbUser,
            ENV: phases[phase].phase,
          },
        }
      )
    );
  }

  // The deployment of your cool app goes here ▼▼▼
  objects.push(
    ...oc.processDeploymentTemplate(
      `${templatesLocalBaseUrl}/postgresql-deploy-config.yaml`,
      {
        param: {
          PROJECT_NAME: `${phases[phase].name}`,
          NAME: `${phases[phase].name}-db`,
          SUFFIX: phases[phase].suffix,
          VERSION: phases[phase].tag,
          ENV: phases[phase].phase,
          PERSISTENT_VOLUME_SIZE: phases[phase].dbSize,
          CPU: phases[phase].db_cpu,
          MEMORY: phases[phase].db_memory,
        },
      }
    )
  );

  objects.push(
    ...oc.processDeploymentTemplate(
      `${templatesLocalBaseUrl}/backup-deploy-config.yaml`,
      {
        param: {
          PROJECT_NAME: `${phases[phase].name}`,
          NAME: `${phases[phase].name}-backup`,
          SUFFIX: phases[phase].suffix,
          VERSION: phases[phase].tag,
          ENV: phases[phase].phase,
          BACKUP_VOLUME_NAME: phases[phase].backupVolume,
          BACKUP_VOLUME_SIZE: phases[phase].backupVolumeSize,
          VERIFICATION_VOLUME_SIZE: phases[phase].verificationVolumeSize,
        },
      }
    )
  );

  objects.push(
    ...oc.processDeploymentTemplate(
      `${templatesLocalBaseUrl}/api-deploy-config.yaml`,
      {
        param: {
          PROJECT_NAME: `${phases[phase].name}`,
          NAME: `${phases[phase].name}-api`,
          SUFFIX: phases[phase].suffix,
          VERSION: phases[phase].tag,
          ENV: phases[phase].phase,
          ASPNETCORE_ENVIRONMENT: phases[phase].dotnet_env,
          CPU: phases[phase].api_cpu,
          MEMORY: phases[phase].api_memory,
        },
      }
    )
  );

  objects.push(
    ...oc.processDeploymentTemplate(
      `${templatesLocalBaseUrl}/client-deploy-config.yaml`,
      {
        param: {
          PROJECT_NAME: `${phases[phase].name}`,
          NAME: `${phases[phase].name}-client`,
          SUFFIX: phases[phase].suffix,
          VERSION: phases[phase].tag,
          ENV: phases[phase].phase,
          HOST: phases[phase].host,
          CPU: phases[phase].client_cpu,
          MEMORY: phases[phase].client_memory,
        },
      }
    )
  );

  oc.applyRecommendedLabels(
    objects,
    phases[phase].name,
    phase,
    `${changeId}`,
    phases[phase].instance
  );
  oc.importImageStreams(
    objects,
    phases[phase].tag,
    phases.build.namespace,
    phases.build.tag
  );
  oc.applyAndDeploy(objects, phases[phase].instance);
};
