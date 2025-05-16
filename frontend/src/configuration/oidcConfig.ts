const oidcConfig = {
  authority: 'http://localhost:8080/realms/split-costs',
  client_id: 'split-costs-client',
  redirect_uri: 'http://localhost:5173/', // Ou um path específico, ex: '/callback'
  scope: 'openid profile email', // Escopos
  response_type: 'code',
  onSigninCallback: () => {
    window.history.replaceState({}, document.title, window.location.pathname);
  },
};

export default oidcConfig;