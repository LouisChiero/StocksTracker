(function() {
    'use strict';

    angular.module("services")
        .factory("httpInterceptorService", [
        '$injector',
        'common',
        httpInterceptorService]);

    function httpInterceptorService($injector, common) {

        var $q = common.$q;

        var service = {
            request: request,
            requestError: requestError,
            response: response,
            responseError: responseError
        };

        return service;

        function request(httpConfig) {

            httpConfig.headers = httpConfig.headers || {};

            var authService = $injector.get('authenticationService'),
                accessToken = authService.user().accessToken();
            
            if (accessToken !== null && accessToken !== undefined)
                httpConfig.headers.Authorization = 'Bearer ' + accessToken;

            console.log(httpConfig);
            return httpConfig;
        }

        // here for info only. probably not useful to intercept this call.
        function requestError(httpRejection) {
            //console.log(httpRejection);
            return $q.reject(httpRejection);
        }

        // here for info only. probably not useful to intercept this call.
        function response(httpResponse) {
            //console.log(httpResponse);
            return httpResponse;
        }

        function responseError(httpRejection) {

            var authService = $injector.get('authenticationService');

            if (httpRejection.status === 401 || httpRejection.status === 500) {
                authService.logout();
            }

            console.log(httpRejection);
            return $q.reject(httpRejection);
        }
    }
})();