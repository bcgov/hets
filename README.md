![img](https://img.shields.io/badge/Lifecycle-Stable-97ca00)

# Hired Equipment Tracking System (HETS)

## Introduction

The BC Ministry of Transportation's [Hired Equipment Program](http://www2.gov.bc.ca/gov/content/industry/construction-industry/transportation-infrastructure/hired-equipment-program) is for owners/operators who have a dump truck, bulldozer, back hoe or other piece of equipment they want to hire out to the Transportation Ministry for day labour and emergency projects. The Hired Equipment Program distributes available work to local equipment owners. The program is based on seniority and is designed to deliver work to registered users fairly and efficiently through the development of local area call-out lists.

The Hired Equipment Tracking System (HETS) is currently a part of [BC Bid](http://www.bcbid.gov.bc.ca/). However, the current version of BC Bid is being replaced by a new version at the end of Fiscal 16/17 and the new version will not include support for the HETS program. As such, a Project is underway to replace the current HETS with a new implementation.

The application is being developed as an open source solution.

## Repository Map

- **client**: The javascript source for the user interface
- **Common**: A library of common methods used by various components
- **FrontEnd**: The Front End server that hosts static content and proxies the API
- **Server**: The API Server

## Installation

This application is meant to be deployed to RedHat OpenShift version 4. The full application will require sufficient Persistent Volume storage for the database and configuration secrets.

## Developer Prerequisites

**Client**

- Node.js
- Text editor such as VSCode

**Server**

- .Net Core SDK (.NET Core App 2.0 is used for all components)
- Node.js
- .NET Core IDE such as Visual Studio
- PostgreSQL 10 or newer

**DevOps**

Refer to [this document](openshift/README.md) for OpenShift Deployment and Pipeline related topics

## Development

**Client Code**
The client code is tested using a Node.js application. Node.js is also used to build the client code into the deployable JavaScript application.

Run npm install from the client directory to configure the client build environment

The frameworks used for this application are React/Redux.

**API Services**

- Create a local postgres database that you will use for development purposes
- Edit the appsetting.json and set the ConnectionStrings for your HETS DB
- Run the code in Development mode, which will allow you to get a Developer Token allowing the application to run outside of a BC Government SiteMinder authenticated environment.
  - SiteMinder - a centralized web access management system that enables user authentication and single sign-on, policy-based authorization, identity federation, and auditing of access to Web applications and portals.
- A developer token is obtained by going to the following url, where <UserId> is a valid SiteMinder UserId field in the database.
  - `/api/authentication/dev/token?userid=<UserId>`

**SonarQube**

SonarQube is a code quality service that helps identify problem areas in code through static analysis.

A batch file is provided in the Server code directory that can be used to run SonarQube code analysis on the API Server code.

Before running this batch file, `sonar.bat`, ensure that you have a valid SonarQube account (can be your GitHub account once registered with SonarQube.com) and that your SonarQube token is installed properly on your computer. You will also need the SonarQube Scanner for C# to be installed on your local computer.

The file sonar.bat will start the SonarQube scanner, execute a build, and stop the scanner. You may then go to the SonarQube.com website to view the results of the scan.

**Viewing the Database**

This application utilizes the [SchemaSpy](https://github.com/bcgov/SchemaSpy) OpenShift image to provide an easy way for stakeholders to view the database schema. The SchemaSpy component is a self contained schema viewer application that can be rapidly deployed to analyze the database structure and provide a website to review details of the database.

## Contribution

Please report any [issues](https://github.com/bcgov/hets/issues).

[Pull requests](https://github.com/bcgov/hets/pulls) are always welcome.

If you would like to contribute, please see our [contributing](CONTRIBUTING.md) guidelines.

Please note that this project is released with a [Contributor Code of Conduct](CODE_OF_CONDUCT.md). By participating in this project you agree to abide by its terms.

## License

    Copyright 2017 Province of British Columbia

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.

## Maintenance

This repository is maintained by [BC Ministry of Transportation](http://www.th.gov.bc.ca/).
Click [here](https://github.com/orgs/bcgov/teams/tran/repositories) for a complete list of our repositories on GitHub.
