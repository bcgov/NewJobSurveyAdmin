import 'reflect-metadata'
import React from 'react'
import ReactDOM from 'react-dom/client'

import { HashRouter } from 'react-router'
import { routerBase as getRouterBasename } from './helpers/envHelper'
import { unregister } from './registerServiceWorker'
import App from './components/App'
import KeycloakService from './components/Login/KeycloakService'

import './components/App.scss'

const root = ReactDOM.createRoot(document.getElementById('root') as HTMLElement);

KeycloakService.initKeycloak(() => {
  root.render(
    <HashRouter basename={getRouterBasename()}>
      <App />
    </HashRouter>
  );
});

// registerServiceWorker()
unregister()
