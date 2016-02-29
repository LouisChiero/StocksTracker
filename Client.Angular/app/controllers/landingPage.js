(function() {
    'use strict';
    var controllerId = 'landingpage';

    angular.module('stocksTrackerApp').controller(controllerId, [
        'common',
        '$rootScope',
        'config',
        landingpage]);

    function landingpage(common, $rootScope, config) {

        console.log("landingpage.");

        var vm = this,
            loginVisible = true;

        activate();

        function activate() {
            common.activateController([], controllerId)
                .then(function () { console.log(controllerId + " loaded"); });
        }

        vm.showLogin = function() {
            return loginVisible;
        };

        // only show resources to authenticated user
        $rootScope.$on(config.events.userAuthenticationStatusChanged, function (event, args) {
            console.log("landingpage.userAuthenticationStatusChanged");
            console.log(event);
            console.log(args);
            //vm.showLogin = !args.authenticated;
            loginVisible = !args.authenticated;
            console.log(loginVisible);
            console.log(vm.showLogin());
        });
    }
})();