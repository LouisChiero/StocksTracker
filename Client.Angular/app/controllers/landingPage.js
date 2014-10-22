(function() {
    'use strict';

    angular.module('stocksTrackerApp').controller('landingpage', [
        '$rootScope',
        'config',
        landingpage]);

    function landingpage($rootScope, config) {
        
        var vm = this;
        vm.showLogin = true;

        // only show resources to authenticated user
        $rootScope.$on(config.events.userAuthenticationStatusChanged, function (event, args) {
            vm.showLogin = !args.authenticated;
        });
    }
})();