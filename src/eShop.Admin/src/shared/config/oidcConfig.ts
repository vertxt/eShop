export const oidcConfig = {
    authority: 'https://localhost:5005',
    client_id: 'admin_site',
    client_secret: 'admin_secret',
    redirect_uri: 'https://localhost:5002/signin-callback',
    post_logout_redirect_uri: 'https://localhost:5002/signout-callback',
    response_type: 'code',
    scope: 'openid profile email roles api offline_access',
    automaticSilentRenew: true,
    loadUserInfo: true,
}