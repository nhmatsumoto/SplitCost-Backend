const oidcConfig = {
  authority: 'http://localhost:8080/realms/split-costs',
  client_id: 'split-costs-client',
  redirect_uri: 'http://localhost:5173/',
  scope: 'openid profile email',
  response_type: 'code',
  silent_redirect_uri: window.location.origin + "/silent-redirect.html",
  automaticSilentRenew: true,
  loadUserInfo: true,

  onSigninCallback: () => {
    window.history.replaceState({}, document.title, window.location.pathname);
  },
};

export default oidcConfig;