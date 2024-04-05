# Hired Equipment Tracking System Web Application

The Hired Equipment Tracking System (a.k.a. HETS) is a web application to track and hire equipment
in British Columbia. The client/ directory is the home of of the front-end source code.

## Getting Started

These instructions will get you web application and running on your local machine for development
and testing purposes.

### Prerequisites

The following software requirements are required:

1. [Node](https://nodejs.org/en/download/) (at least version 8)
2. [git](https://git-scm.com/downloads)

### Installing

1. Git clone the [repository](https://github.com/bcgov/hets) onto your local machine.

   ```
   git clone git@github.com:bcgov/hets.git
   ```

2. Install the Node module dependencies. You will need to be in the hets/client/ directory.

   ```
   npm install
   ```

   **Note**: The installation of [node-sass](https://www.npmjs.com/package/node-sass) might fail
   depending on the target platform because of a missing C compiler.

3. Run the local dev server

   ```
   npm start
   ```

Now you will be able to access the web app when you go to
[http:://localhost:3000](http:://localhost:3000)

## Development

The web application requires the [C# API Server](https://github.com/bcgov/hets/tree/master/Server)
to be runing on localhost which can connect to a local PostgreSQL database. Check the
[Server project’s documentation](https://github.com/bcgov/hets/blob/master/Server/README.md) on how
to run the API server. Look for documentation in the
[DB Scripts](https://github.com/bcgov/hets/tree/master/Db%20Scripts) directory on how to init a
database.

[Redux](https://redux.js.org/) is used to store most of the application’s state. The application’s
[store.js](https://github.com/bcgov/hets/tree/master/client/src/js/store.js) contains the response
data from any API request to be used by the React components as well as UI data.

[React Router v3](https://github.com/ReactTraining/react-router/tree/v3/docs) is used to control the
browser’s URL history and routing. Routes are defined in
[app.jsx](https://github.com/bcgov/hets/tree/master/client/src/js/app.jsx) file. If a user is a
business owner they can only access the routes at the path `/business`. All other users can access
the rest of the application’s routes.

API requests follow an [action creator pattern](https://redux.js.org/basics/actions#action-creators)
and they are all defined in a [api](./src/js/api.js) file.

[SASS](https://sass-lang.com/) is a CSS preprocessor. The SCSS flavour of SASS is used for all the
files in the project.

The web application uses [Webpack](https://webpack.js.org/)’s
[Hot Module Replacement](https://webpack.js.org/concepts/hot-module-replacement/) which will update
the UI whenever a [React Component](https://reactjs.org/docs/react-component.html) is saved on disk.

[Create React App](https://www.npmjs.com/package/create-react-app) is the tool used for the front-end build system. Inside the

## Coding style

Coding style for JS is enforced by [ESLint](https://eslint.org/). ESLint is run whenever the
JavaScript is built. Warnings turn into errors when the project is built. Any ESLint errors will
fail to build the JS.

## Deployment

Please refer to Openshift ReadMe
https://github.com/bcgov/hets/tree/1.9.3/openshift#readme 

## Built With

- [React](https://reactjs.org/) - JS library for building UIs
- [React Bootstrap](https://react-bootstrap.github.io/) - UI components and styles

## License

This project is licensed under the [Apache v2 License](https://www.apache.org/licenses/LICENSE-2.0).

