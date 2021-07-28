[![CircleCI](https://circleci.com/gh/BCDevOps/pipeline-cli.svg?style=svg)](https://circleci.com/gh/BCDevOps/pipeline-cli)

# pipeline-cli

The `pipeline-cli` is an opinionated utility wrapper around [oc](https://docs.okd.io/3.11/cli_reference/index.html) CLI.
It enables developer to run stages of their continuous delivery pipeline from their own workstation or form within a CI/CD server.

The utitility support and promote the following Continuous Delivery patterns:
- Single Command Environment: "run a single command to build and deploy the application to any accessible environment, including the local development."
- Single Path to Production: "Any change can be tied back to a single revision in a version-control system."
- Short-Lived Branches: "Branches must be short lived – ideally less than a few days and never more than an iteration."

As well as support a [Github Flow](https://guides.github.com/introduction/flow/).

## How we got here

### Version 1: OpenShift Pipeline
This is OpenShift built-in [pipeline](https://docs.okd.io/3.11/dev_guide/openshift_pipeline.html) build strategy.

Pro:
- Easy to setup
- Small footprint

Con:
- It does not managed itself as it requires that the BuildConfig already exists and it is manually managed/maintained
- Issues related to managed Jenkins memory/CPU
- Not very reusable (aside from copy-and-paste)
- High Dependency to Jenkins and Jenkinsfile
- Long Jenkisfile.
- Can't build/deploy from workstation (Without Jenkins)
- No support for PR-based build/deployment.

### Version 2: [Jenkins Shared Library](https://jenkins.io/doc/book/pipeline/shared-libraries/)
Jenkins Shared Library addresses the reusablity aspect, and make it easier to share common pipeline tasks/stages.

Pro:
- Reusable
- Brings consistency accross multiple instances of Jenkins
- Smaller Jenkinsfile
- Opinionated/Configurable

Con:
- Hard to develop/evold the pipeline: Debugging/Troubleshooting requires adding a binch of `echo` to try to identify issues.
- High Dependency to Jenkins and Jekninsfile
- Can't build/deploy from workstation (Without Jenkins)

### Version 3: Groovy Script Library

Pro:
- Reusable
- Single command line build/deployment from Workstation
- Low dependency to Jenkins

Con:
- Java with Gradle + Groovy has a considerably large footprint and long statup. It takes about 5min to actually start build/deployment.
- Complex configuration file

### Version 4: JavaScript Node/NPM Module

Pro:
- Reusable
- Single command line build/deployment from Workstation
- Low dependency to Jenkins
- Easy to extend
- Less opnionated

Con:
- Bigger footprint/boilerplate

## Why Not ...?

### Helm
[Helm](https://helm.sh/) helps you manage Kubernetes applications — Helm Charts help you define, install, and upgrade even the most complex Kubernetes application.

### Operator Framework
The [Operator Framework](https://github.com/operator-framework) is an open source toolkit to manage Kubernetes native applications, called Operators, in an effective, automated, and scalable way.

### OpenShift Do (Odo)
[Odo](https://openshiftdo.org/) utilizes [Source-to-Image](https://github.com/openshift/source-to-image) to create reproducible Docker images from source code. Odo handles the complex task of building, pushing and deploying your source code.



# Coverage of fast tests:
```
DEBUG=info:* npm run coverage
```

# Reset dependencies
```
rm -rf node_modules; npm install
```

# Debug
```
npm link
```
