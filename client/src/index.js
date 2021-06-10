import React from 'react';
import ReactDOM from 'react-dom';
import App from './js/App';
import reportWebVitals from './reportWebVitals';

import * as Keycloak from './js/Keycloak';

import 'bootstrap/dist/css/bootstrap.css';
import './sass/main.scss';

Keycloak.init(() => {
  ReactDOM.render(
    <React.StrictMode>
      <App />
    </React.StrictMode>,
    document.getElementById('root')
  );
});

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
