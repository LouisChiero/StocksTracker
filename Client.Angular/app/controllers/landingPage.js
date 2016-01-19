(function() {
    'use strict';

    angular.module('stocksTrackerApp').controller('landingpage', [
        '$rootScope',
        'config',
        landingpage]);

    function landingpage($rootScope, config) {

        console.log("landingpage.");

        var vm = this,
            loginVisible = true;

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