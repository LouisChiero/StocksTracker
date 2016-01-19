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
                accessToken = 'GC2_MqiuSryNoUVY7h7I2g_U7IsfeGf9KRKkw45uWs16d5vjieIjB4HGO_YI4mEzBQSbJgGAeHys7L74GaKqTxwvc4qvjJIDFEiGC1V24EcWA8QMWK6cx5P8vupdh4BNppvIjs9CkrOr-RmOTjNLpeidvshXCnd4vxx7D5L6p06pKNdnRUFHm0KhVhU8-oV7wJ3UB3CF6nx529n6GSPPF9j9F2Ga1r-YLSze4QbwuiOwXCY-R75YyZue-ACXq9B9FbUnGF8cftNFC_q2TQnp4F7WKbOk5rz721DoTNrOuRrrFCZ2velRxI-Q1a_gW_4N4tcWHDB1MUiKCdBe5uoC-ZlYUkPhlFuB4KP4B3uw9AT2nGOsaoNs3pAUWMKwH0RhEZU9PRUKa6Y3y8_0XseNFKmKBLI-Z7cBaCP5EO0m6NCh7hJVClz3ejepUBRoo1mAlIqgraAOi6GGsjm8XUm0vzMDbjCnW34MHPhJcPeBqz8DMspOLTrGdWGjaznwWsEOGzp3IqzPteW_k5xpMRy8vg';
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