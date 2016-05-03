(function() {
    'use strict';
    var controllerId = 'login';

    angular.module('stocksTrackerApp').controller(controllerId, [
        'common',
        'commonConfig',
        'authenticationService',
        '$location',
        '$http',
        'ngStocksTrackerApiSettings',
        login]);

    function login(common, commonConfig, authenticationService, $location, $http, ngStocksTrackerApiSettings) {
        
        var vm = this;
        vm.loginData = {
            userName: "",
            password: ""
        };

        vm.message = "";        
        vm.submit = userLogin;
        vm.cancel = function (loginForm) {
            loginForm.$setPristine();
            vm.loginData = { userName: "", password: "" };
            vm.message = "";
        };

        vm.externalLogin = externalLogin;

        activate();

        function activate() {
            common.activateController([], controllerId)
                .then(function () { console.log(controllerId + " loaded"); });
        }

        function userLogin() {
            
            var success = false;
            common.eventRaiser(commonConfig.config.userLoginStartedEvent);

            authenticationService.login(vm.loginData)
                .then(function (response) {
                  // success
                  success = true;
                }
                , function (err) {
                  // error
                  vm.message = err.error_description;
                })
            .finally(function () {
                common.eventRaiser(commonConfig.config.userLoginCompletedEvent, { success: success });
            });            
        }

        function externalLogin(provider) {
            console.log(provider);
            var url = ngStocksTrackerApiSettings.apiServiceBaseUri + "/" + ngStocksTrackerApiSettings.accountApiPrefix + "/ExternalLogin/";
      
            //$http({ method: 'POST', url: url, headers: { 'Content-Type': "application/x-www-form-urlencoded" }, params: { 'provider': provider }, responseType: 'json' })
            //    .then(function (data) { return data.data; });

            window.open(url + "?provider=" + provider, "Authenticate Account", "location=0,status=0,width=750,height=750");
        }
    }
})();