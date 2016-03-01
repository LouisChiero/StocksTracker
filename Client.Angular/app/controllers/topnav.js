(function() {
    'use strict';
    var controllerId = 'topnav';

    angular.module('stocksTrackerApp').controller(controllerId, [
        '$location',
        '$rootScope',
        'stockTrackerService',
        'authenticationService',
        'config',
        'common',
        topnav]);

    function topnav($location, $rootScope, stockTrackerService, authenticationService, config, common) {
        
        var vm = this;
        vm.loggedInUserName = null;
        vm.showResources = false;
        vm.searchStocks = searchStocks;
        vm.done = done;
        vm.logout = logout;      

        activate();

        function activate() {
            common.activateController([], controllerId)
                .then(function () { console.log(controllerId + " loaded"); });
        }
        
        // only show resources to authenticated user
        $rootScope.$on(config.events.userAuthenticationStatusChanged, function (event, args) {
            vm.showResources = args.authenticated;
            vm.loggedInUserName = args.userName;
        });

        function searchStocks(val) {

            return stockTrackerService.searchForStock(val)
                .then(function(data) { return common.underscore.first(data, 10); });
        }

        function done($item, $model, $label) {
            // show selected stock
            $location.path("/stock/" + $item.ticker);
        }

        function logout() {
            authenticationService.logout();
        }        
    };
})();