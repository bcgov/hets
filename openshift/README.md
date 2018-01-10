Hired Equipment Tracking System
======================

## Running in OpenShift

This project uses the scripts found in [openshift-project-tools](https://github.com/BCDevOps/openshift-project-tools) to setup and maintain OpenShift environments (both local and hosted).  Refer to the [OpenShift Scripts](https://github.com/BCDevOps/openshift-project-tools/blob/master/bin/README.md) documentation for details.

**These scripts are designed to be run on the command line in the root `openshift` directory of your project's source code.**

## Running in a Local OpenShift Cluster

At times running in a local cluster is a little different than running in the production cluster.

Differences can include:
* Resource settings.
* Available image runtimes.
* Source repositories (such as your development repo).
* Etc.

To target a different repo and branch, create a `setting.local.sh` file in your project's local `openshift` directory and override the GIT parameters, for example;
```
export GIT_URI="https://github.com/WadeBarnes/hets.git"
export GIT_REF="openshift-updates"
```

Then run the following command from the project's local `openshift` directory:
```
genParams.sh -l
```

This will generate local settings files for all of the builds, deployments, and Jenkins pipelines.
The settings in these files will be specific to your local configuration and will be applied when you run the `genBuilds.sh` or `genDepls.sh` scripts with the `-l` switch.

### Important Local Configuration for the HETS project

Before you deploy your local build configurations ...

The client, server, and pdf components of the project use .Net 2.0 s2i images for their builds.  In the pathfinder environment these components utilize the `dotnet-20-rhel7` image which is available at registry.access.redhat.com/dotnet/dotnet-20-rhel7.  For local builds this image can still be downloaded, however you will receive errors during any builds (Docker builds) that try to use `yum` to install any additional packages.

To resolve this issue the project defined builds for `dotnet-20-runtime-centos7` and `dotnet-20-centos7`; which at the time of writing were not available in image form.  The `dotnet-20-centos7` s2i image is the CentOS equivalent of the `dotnet-20-rhel7` s2i image that can be used for local development.  These two images are not used in the Pathfinder environment and exist only to be used in a local environment.

To switch to the `dotnet-20-centos7` image for local deployment, open your `client-build.local.param` file an update the following 2 lines;

```
# SOURCE_IMAGE_KIND=DockerImage
# SOURCE_IMAGE_NAME=registry.access.redhat.com/dotnet/dotnet-20-rhel7
```
with
```
SOURCE_IMAGE_KIND=ImageStreamTag
SOURCE_IMAGE_NAME=dotnet-20-centos7
```

### Preparing for local deployment

1. Install the oc cli.  If you are working on a Windows 10 machine you get can up and running quickly using the scripts found here; [Chocolatey Scripts](https://github.com/WadeBarnes/dev-tools/tree/master/chocolatey).
1. Start an OpenShift cluster.  If you are using Docker you can use the `oc-cluster-up.sh` scripts from [openshift-project-tools](https://github.com/BCDevOps/openshift-project-tools).
1. Run `generateLocalProjects.sh` to create the projects in your local cluster.
1. Run `initOSProjects.sh` to update the deployment permissions on the projects.
1. To fix the routes in your local deployment environments use the `updateRoutes.sh` script.

From here on out the commands used to deploy and management the application configurations are basically the same for a local cluster as they are for the Pathfinder environment.

## Deploying your project

All of the commands listed in the following sections must be run from the root `openshift` directory of your project's source code.

### Before you begin ...

If you are updating an existing environment you will need to be conscious of retaining access to the existing data in the given environment.  User accounts, database names, and database credentials can all be affected.  The processes affecting them should be reviewed and understood before proceeding.

For example, the process of deploying and managing database credentials has changed.  The process has moved to using shared secretes that are mounted as environment variables, where previously they were provisioned as discrete environment variables in each component's environment.  Further the new deployment process, by default, will create a random set of credentials for each deployment or update (a new set every time you run `genDepls.sh`).  Being that the credentials are shared, there is a single source and place that needs to be updated.  You simply need to ensure the credentials are updated to the values expected by the pre-configured environment if needed.

### Initialization

If you are working with a new set of OpenShift projects, or you have run a `oc delete all --all` to start over, run the `initOSProjects.sh` script, this will repair the cluster file system services (in the Pathfinder environment), and ensure the deployment environments have the correct permissions to deploy images from the tools project.

### Generating the Builds, Images and Pipelines in the Tools Project

Run;
```
genBuilds.sh
```
, and follow the instructions.

All of the builds should start automatically as their dependencies are available, starting with builds with only docker image and source dependencies.

The process of deploying the Jenkins pipelines will automatically provision a Jenkins instance if one does not already exist.  This makes it easy to start fresh; you can simply delete the existing instance along with it's associated PVC, and fresh instances will be provisioned.

### Deploying the Secrets

The application relies on a number of secrets used during initialization that must be loaded into the environment before components such as the server will function properly.  There is a script and a template to make deployment easy, but you have to provide the appropriate content.

Secrets should NEVER be checked into source control, and they should be managed securely.

The secrets are used to initialize the user accounts for a given environment and must contain JSON in the following form.

**users.json**
`[
{
"active": true,
"email": ""email address,
"givenName": "Given Name",
"id": "Id field",
"initials": "Initials",
"smUserID": "Siteminder User ID",
"surname": "Surname",
"groupMemberships": [{"Group" : {"name": "Group Name"}}],
"userRoles": [{"Role": {"Name": "Role Name"}}]
 },
<other users>
]`

**districts.json**

`[
{
"endDate": null,
"id": "1",
"ministryDistrict": "1",
"name": "Lower Mainland",
"region": {
"id": "1",
"ministryRegionID": "1"
},
"startDate": "1/1/2009"
},
<other districts>`
]`

**regions.json**

`[
{
"Name": "South Coast",
"endDate": null,
"id": "1",
"ministryRegionID": "1",
"startDate": "1/1/2009"
},
<other regions>
]`

Create the following files in your project directory:

`.../hets/openshift/secrets/server-secret/districts.json`

`.../hets/openshift/secrets/server-secret/regions.json`

`.../hets/openshift/secrets/server-secret/users.json`

Populate the files with the json content appropriate for the intended environment (the content will likely be different for some environments such as prod).

Run;
```
genSecrets.sh -e <environmentName/>
```
, to deploy the secrets to the desired environments.

Review the results in the OpenShift web interface to ensure the secrets were deployed as expected.

### Generate the Deployment Configurations and Deploy the Components

Run;
```
genDepls.sh -e <environmentName/>
```
, and follow the instructions to deploy the application components to the desired environments.

### Wire up your Jenkins Pipelines

When the Jenkins Pipelines were provisioned when your ran `genBuilds.sh` web-hook URLs and secrets were generated automatically.  To trigger the pipelines via GIT commits, register the URL(s) for the pipelines with GIT.

Copy and paste the pipeline's web-hook url into the Payload URL of the GIT web-hook (it comes complete with the secret).

Set the content type to **application/json**

Select **Just the push event**

Check **Active**

Click **Add webhook**

## Environment Variables

The following environment variables are used by the application:

| Environment Variable | Purpose | Example | Notes |
| ---------------------| ------- | ------- | ----- |
| DATABASE_SERVICE_NAME | Database service | postgresql | set to localhost for local development |
| POSTGRESQL_USER | PGSQL User | test | Must have enough access to create tables |
| POSTGRESQL_PASSWORD | PGSQL User's Password | test | |
| POSTGRESQL_DATABASE | Database name | hets | |
| UserInitializationFile | Location of user seed data | /secrets/users.json | Json format following the User model definition |
| DistrictInitializationFile | Location of District seed data | /secrets/districts.json | Json format following the District model definition |
| RegionInitializationFile | Location of Region seed data | /secrets/regions.json | Json format following the Region model definition |
| PDF_SERVICE_NAME | PDF microservice location | http://pdf:8080 | |
| ASPNETCORE_ENVIRONMENT | Type of deployment | Development | Set to other Production (or anything other than Development) to disable development features such as user visible stack traces. |

Credit:  [George Walker](https://github.com/GeorgeWalker), [Wade Barnes](https://github.com/WadeBarnes) contributed to this page.
