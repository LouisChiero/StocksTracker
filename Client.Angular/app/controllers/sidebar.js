(function () {
    'use strict';

    var controllerId = 'sidebar';
    angular.module('stocksTrackerApp').controller(controllerId, [
        '$rootScope',
        'common',
        'config',
        '$route',
        '$routeParams',
        '$location',
        'bootstrap.addDialogService',
        'bootstrap.updateDialogService',
        'bootstrap.deleteDialogService',
        sidebar]);

    function sidebar($rootScope, common, config, $route, $routeParams, $location, addDialog, updateDialog, deleteDialog) {
        // logging
        var log = common.logger.getLogFn(controllerId);

        // bindings
        var vm = this;
        vm.showResources = false;
        vm.isActive = isActive;
        vm.navRoutes = [];
        vm.createNewStockTracker = createNewStockTracker;
        vm.renameStockTracker = renameStockTracker;
        vm.deleteStockTracker = deleteStockTracker;

        // only show resources to authenticated user
        $rootScope.$on(config.events.userAuthenticationStatusChanged, function (event, args) {
            vm.showResources = args.authenticated;
        });

        // each time a tracker is loaded, add it to the route collection
        $rootScope.$on(config.events.stockTrackerLoaded, function (event, args) {
            vm.navRoutes.push(makeNavRoute(args.stockTracker));
        });

        // add new tracker to route collection
        $rootScope.$on(config.events.stockTrackerCreated, function (event, args) {
            var newRoute = makeNavRoute(args.newStockTracker);
            vm.navRoutes.push(newRoute);
            log("Created stock tracker " + newRoute.display + ".");

            // "activate" new tracker
            $location.path("/stocktracker/" + newRoute.id);
        });

        // update an existing tracker
        $rootScope.$on(config.events.stockTrackerUpdated, function (event, args) {
            var st = args.updatedStockTracker;
            var targetRoute = common.underscore.find(vm.navRoutes, function (nav) {
                return nav.id == st.id;
            });

            log("Updated stock tracker " + st.name + ".");
            
            if (targetRoute) {
                targetRoute.display = st.name;
                targetRoute.stocks = st.stocks.length;
            }
        });

        // remove tracker from route collection
        $rootScope.$on(config.events.stockTrackerRemoved, function (event, args) {
            var targetRoute = common.underscore.find(vm.navRoutes, function (nav) {
                return nav.id == args.removedStockTrackerId;
            });

            log("Stock tracker removed.");

            if (targetRoute) {
                vm.navRoutes = common.underscore.without(vm.navRoutes, targetRoute);

                // if the active route is deleted, redirect to home.
                if (isActive(targetRoute)) {
                    $location.path("/");
                }
            }
        });

        function isActive(route) {
            if (!$route.current || !$route.current.title || $route.current.title !== "stocktracker")
                return '';
           
            return $routeParams.stocktrackerid == route.id ? 'active' : '';
        }

        function createNewStockTracker() {
            // launch add dialog
            addDialog.showAddStockTrackerDialog();
        }

        function renameStockTracker(route) {
            // launch update dialog
            updateDialog.showUpdateStockTrackerDialog({ id: route.id, name: route.name });
        }

        function deleteStockTracker(route) {
            // launch update dialog
            deleteDialog.showDeleteStockTrackerDialog({ id: route.id, name: route.display });
        }

        function makeNavRoute(stockTracker) {
            return {
                id: stockTracker.id,
                display: stockTracker.name,
                url: "/stocktracker/" + stockTracker.id,
                stocks: stockTracker.stocks.length// ? stockTracker.stocks.length : null
            };
        }
    };
})();
