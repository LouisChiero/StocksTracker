(function() {
    'use strict';
    var controllerId = 'login';

    angular.module('stocksTrackerApp').controller(controllerId, [
        'common',
        'commonConfig',
        'authenticationService',
        '$location',
        login]);

    function login(common, commonConfig, authenticationService, $location) {
        
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
    }
})();