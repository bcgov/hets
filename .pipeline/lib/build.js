"use strict";
const { OpenShiftClientX } = require("@bcgov/pipeline-cli");
const path = require("path");

module.exports = async (settings) => {
  const phases = settings.phases;
  const options = settings.options;
  const oc = new OpenShiftClientX(
    Object.assign({ namespace: phases.build.namespace }, options)
  );
  const phase = "build";
  let objects = [];
  const templatesLocalBaseUrl = oc.toFileUrl(
    path.resolve(__dirname, "../../openshift")
  );

  objects.push(
    ...oc.processDeploymentTemplate(
      `${templatesLocalBaseUrl}/api-build-config.yaml`,
      {
        param: {
          PROJECT_NAME: `${phases[phase].name}`,
          NAME: `${settings.phases[phase].name}-api`,
          SUFFIX: settings.phases[phase].suffix,
          VERSION: settings.phases[phase].tag,
          SOURCE_REPOSITORY_URL: `${oc.git.uri}`,
          SOURCE_REPOSITORY_REF: `${oc.git.branch_ref}`,
        },
      }
    )
  );

  objects.push(
    ...oc.processDeploymentTemplate(
      `${templatesLocalBaseUrl}/client-build-config.yaml`,
      {
        param: {
          PROJECT_NAME: `${phases[phase].name}`,
          NAME: `${settings.phases[phase].name}-client`,
          SUFFIX: settings.phases[phase].suffix,
          VERSION: settings.phases[phase].tag,
          SOURCE_REPOSITORY_URL: `${oc.git.uri}`,
          SOURCE_REPOSITORY_REF: `${oc.git.branch_ref}`,
        },
      }
    )
  );

  oc.applyRecommendedLabels(
    objects,
    phases[phase].name,
    phase,
    phases[phase].changeId,
    phases[phase].instance
  );

  // Apply the build resources once, then start only the application builds.
  // OpenShiftClientX.applyAndBuild() scans every historical image in each
  // output image stream before deciding whether to reuse an image. The HETS
  // streams contain years of history, making that scan slow and vulnerable to
  // transient API failures. The build configurations already use no-cache
  // strategies, so direct builds are deterministic and avoid that scan.
  oc.apply(objects);

  const buildConfigs = [
    `${phases[phase].name}-api${phases[phase].suffix}`,
    `${phases[phase].name}-client${phases[phase].suffix}`,
  ];

  for (const buildConfig of buildConfigs) {
    console.log(`Starting and waiting for ${buildConfig}`);
    await oc.startBuild(`BuildConfig/${buildConfig}`, { wait: "true" });
  }
};
