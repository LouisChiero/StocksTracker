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
            // TODO: this is not the correct event to raise here. s/b loginBegunEvent
            common.eventRaiser(commonConfig.config.spinnerToggleEvent, { show: true });

            authenticationService.login(vm.loginData)
                .then(function (response) {

                    // TODO: is it possible to capture the route & redirect?
                    $location.path('/home');
            },
             function (err) {

                 vm.message = err.error_description;

             })
            .finally(function () {
                common.eventRaiser(commonConfig.config.spinnerToggleEvent, { show: false });
            });
            
        }
    }
})();