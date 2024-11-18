import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './js/App';
import reportWebVitals from './reportWebVitals';

import * as Keycloak from './js/Keycloak';
import { Provider } from 'react-redux';
import { store } from './js/store';

import 'bootstrap/dist/css/bootstrap.css';
import './sass/main.scss';

Keycloak.init(() => {
  const root = ReactDOM.createRoot(document.getElementById('root'));

  root.render(
    <Provider store={store}>
      <React.StrictMode>
        <App />
      </React.StrictMode>
    </Provider>
  );
});


// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
