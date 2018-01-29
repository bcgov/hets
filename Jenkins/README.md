# Creating Jenkins Pipelines

## Before you begin

This project uses the scripts found in [openshift-project-tools](https://github.com/BCDevOps/openshift-project-tools) to setup and maintain OpenShift environments (both local and hosted).  Refer to the [OpenShift Scripts](https://github.com/BCDevOps/openshift-project-tools/blob/master/bin/README.md) documentation for details.

**These scripts are designed to be run on the command line (using Git Bash for example) in the root `openshift` directory of your project's source code.**

## Steps

1. Create your Jenkins file following the established folder structure or naming convention.  Either place the `Jenkinsfile` in an appropriately named directory, or name your Jenkinsfile `<name/>-Jenkinsfile`.
1. Write your scripted pipeline process.
1. Open a Git Bash (or other such cmd line) to the project's root `openshift` directory.
1. Run `genParams.sh` to generate the `.param` files for the new pipelines.
1. Run `genBuilds.sh` to create the new pipelines in the *tools* project of your OpenShift project set.  You will likely see errors/warnings indicating some resources already exist.  You can ignore these errors and run the script through to completion to ensure the new resources get created.
1. Optionally wire up a GitHub webhook to the newly created pipeline.
1. Test your new pipeline.