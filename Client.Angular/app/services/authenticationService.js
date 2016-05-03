(function() {
    'use strict';

    angular.module("services")
        .factory("authenticationService", [
            '$http',
            'common',
            'commonConfig',
            'ngStocksTrackerApiSettings',
            authenticationService]);

    function authenticationService($http, common, commonConfig, ngStocksTrackerApiSettings) {
        console.log("authenticationService");
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
            
            var cachedUser = common.localStorage.applicationUserValues;
            if (cachedUser !== undefined) {
                var values = angular.fromJson(cachedUser);
                authenticated = values.authenticated;
                userName = values.userName;
                accessToken = values.accessToken;

                // raise event
                common.eventRaiser(commonConfig.config.userLoggedInEvent, { userName: userName });
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
        }

        function register(registration) {

            // clear auth
            revokeAuthorization();          

            var url = ngStocksTrackerApiSettings.apiServiceBaseUri + "/" + ngStocksTrackerApiSettings.accountApiPrefix + "/Register";
            var deferred = common.$q.defer();

            return $http.post(url, { UserName: registration.userName, Password: registration.password, ConfirmPassword: registration.confirmPassword })
                .success(function (response) { deferred.resolve(response); })
                .error(function (err, status) { deferred.reject(err); });
        }

        function login(loginData) {

            var url = ngStocksTrackerApiSettings.apiTokenUri;
            var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

            //if (loginData.useRefreshTokens) {
            //    data = data + "&client_id=" + ngAuthSettings.clientId;
            //}

            var deferred = common.$q.defer();

            $http.post(url, data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' }, withCredentials: true })
                .success(function (response) {

                    authenticated = true;
                    userName = loginData.userName;
                    accessToken = response.access_token;

                    common.localStorage.applicationUserValues = angular.toJson({ authenticated: true, userName: userName, accessToken: accessToken });

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
            delete common.localStorage.applicationUserValues;
            authenticated = false;
            userName = null;
            accessToken = null;
        }
    }

})();