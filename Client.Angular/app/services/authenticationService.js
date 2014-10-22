(function() {
    'use strict';

    angular.module("services")
        .factory("authenticationService", [
            '$http',
            '$window', // TODO: replace with storage service
            'common',
            'commonConfig',
            'ngStocksTrackerApiSettings',
            authenticationService]);

    function authenticationService($http, $window, common, commonConfig, ngStocksTrackerApiSettings) {

        // constant value for querying auth data from storage
        var authenticationStorageKey = "AUTHENTICATION_DATA",
            authenticated = false,
            userName = null,
            accessToken = null,
            // JS object represents the application user
            applicationUser = {
                // keep these read-only
                authenticated: function () { return authenticated; },
                userName: function () { return userName; },
                accessToken: function () { return accessToken; }
            };

        var service = {
            bootstrapAuthentication: bootstrapAuthentication,
            user: getApplicationUser,
            logout: logout,
            register: register,
            login: login
        };

        return service;

        function bootstrapAuthentication() {
            //console.log(commonConfig.config.userAuthenticationSucceededEvent);
            // check for "cached" authentication data
            // broadcast state of application user to listeners
            if (true) { // TODO: check storage service for auth data
                authenticated = true;
                userName = 'Louis Chiero';
                accessToken = '_nP-_9ybjhVMXOHKjX-Uz9SdPQprH7dJoGIOelkZ1umiAbgXIfQ-HNXiuxCFGINs9ax0YQ7eV2RxcOUvtXpkv08OfMvEWK9LtYOLkmYfA0eh8SpiHenK0n1lrxedd019k1fdBgrzM6kp0AwHh__73nl1pIct_BJV6BH1h0UbNmkK8A_OVR-HPiYJiO1ZhAIVXHDJz8pUHDvE8aP0sM-IwKZM0O48UU1NXtTN9kJ2Co1i5e6JmuYA2EygD8nO15y86ViKAu7-s2mteIBgrNjZCjlowMsemKYgWktg48baIPybkqRKRCMxFfFBr9PeOWJyaUj_LlHEexn8050SNJnfLpwJmTPhyEaKCdrCOARumZQtc2P6cChsuXAO7gzXmuuRvjAEHfNiLcERXvR3xvJnyF8Z1ETFZpcbsGfv8-DimMKjWaHXHhOl9ys8oWuvL_ocncCjq5yC-fO5txCOVIglkeqXxDLfekrEd5md72CEq-w';
                //console.log(applicationUser);
                //common.eventRaiser(commonConfig.config.userAuthenticationSucceededEvent, { userName: applicationUser.userName() });
            } else {
                authenticated = false;
                userName = null;
                accessToken = null;
                //common.eventRaiser(commonConfig.config.userAuthenticationFailedEvent);
            }
        }

        function getApplicationUser() {
            return applicationUser;
        }

        function logout() {

            // clear auth
            revokeAuthorization();

            // raise event
            common.eventRaiser(commonConfig.config.userLoggedOutEvent);
            
            //var url = ngStocksTrackerApiSettings.apiServiceBaseUri + "/" + ngStocksTrackerApiSettings.accountApiPrefix + "/Logout";
            //return $http.post(url)
            //    .then(function () {
            //        // raise event
            //        common.eventRaiser(commonConfig.config.userLoggedOutEvent);
            //});
        }

        function register(registration) {

            // clear auth
            revokeAuthorization();

            var url = ngStocksTrackerApiSettings.apiServiceBaseUri + "/" + ngStocksTrackerApiSettings.accountApiPrefix + "/Register";
            return $http.post(url, { UserName: registration.userName, Password: registration.password })
                .then(function(response) { return response; });
        }

        function login(loginData) {

            var url = ngStocksTrackerApiSettings.apiServiceBaseUri + "/Token";
            var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

            //if (loginData.useRefreshTokens) {
            //    data = data + "&client_id=" + ngAuthSettings.clientId;
            //}

            var deferred = $q.defer();

            $http.post(url, data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
                .success(function (response) {

                //if (loginData.useRefreshTokens) {
                //    localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName, refreshToken: response.refresh_token, useRefreshTokens: true });
                //}
                //else {
                //    localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName, refreshToken: "", useRefreshTokens: false });
                //}
                //_authentication.isAuth = true;
                //_authentication.userName = loginData.userName;
                    //_authentication.useRefreshTokens = loginData.useRefreshTokens;
                    authenticated = true;
                    userName = loginData.userName;
                    accessToken = response.access_token;

                    // raise event
                    common.eventRaiser(commonConfig.config.userLoggedInEvent, { userName: userName });

                deferred.resolve(response);

            }).error(function (err, status) {
                revokeAuthorization();
                deferred.reject(err);
            });

            return deferred.promise;
        }

        function revokeAuthorization() {
            // TODO: remove from local storage
            authenticated = false;
            userName = null;
            accessToken = null;
        }
    }

})();