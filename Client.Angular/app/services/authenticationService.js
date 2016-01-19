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
                accessToken = 'n2V1JQYrf5RTFiswsaEIvM7ZHI3f6ZZ2Jismuep2M-KM4tn_goemJOtaCDQCiEIE-rHsYwqi1RB8nyV-uCWExWvpLih0c1zSSk5xE6K9qPr-eKhMR8lX0SdlhDk5Z8hk9I0Szwtq6fIhxJHurLPRrN3y-aT8DteIJcHBmPvghiSbt46nb_-bGYaNNDyHK1KszVSUsl23a_TkMeQpjB1cIcNPdR7q3MRzDYp-pYGkN2Ahevn4gCMMceqgLN-gC-e2sSVcHp1U6g9iXmU6euODgQNu7f3jcuPHr9OpmiOmd49H3nrskz3KsdEjT-bdTgW-GwB4R-ubW5HpvWLaML-9aBGTh0YBoyEVSTdPbDEstl1FO7CI80qmg2sTdZ7SE-2M6AdRUkuHlpXkqOX1_jJxSeeGPI2aIaeMshPsaiznqEgYdALm2uFhYssZ8WosbMJNlXD7R7u9k181YnlorPRLkU-D5XCsWQxrCVx47lf409YAn3pk8wyp2nHRmiWe9gb-lUAHhcEH0xU4eQsNywqt_w';
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