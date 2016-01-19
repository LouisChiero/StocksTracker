(function () {
    'use strict';

    var controllerId = 'shell';
    angular.module('stocksTrackerApp').controller(controllerId, [
        '$rootScope',
        '$location',
        '$document',
        'common',
        'config',
        'commonConfig',
        'authenticationService',
        'stockTrackerService',
        shell]);

    function shell($rootScope, $location, $document, common, config, commonConfig, authenticationService, stockTrackerService) {
        // logging
        var logSuccess = common.logger.getLogFn(controllerId, 'success'),
            log = common.logger.getLogFn(),
        // promises
            $q = common.$q,
        // events
            events = config.events,
        // application user
            applicationUser = null,
        // bindings
            vm = this;

        vm.busyMessage = 'Please wait ...';
        vm.isBusy = true;
        vm.spinnerOptions = {
            radius: 40,
            lines: 7,
            length: 0,
            width: 30,
            speed: 1.7,
            corners: 1.0,
            trail: 100,
            color: '#F58A00'
        };

        console.log("shell");
        activate();

        function activate() {
            console.log("shell.activate");
            common.activateController([
                    logSuccess('Stocks Tracker loaded!', null, true),
                    checkForAuthenticatedUserAndLoadStockTrackers()
                ], controllerId)
                // put any post-loading action(s) here
                .then(function() { console.log("all done"); });
        }

        $rootScope.$on(config.events.userLoggedOut, function (event, args) {
            common.eventRaiser(commonConfig.config.userAuthenticationStatusChangedEvent,
                { authenticated: false });
            // go to login page
        });

        $rootScope.$on(config.events.userLoggedIn, function (event, args) {
            $q.when(checkForAuthenticatedUserAndLoadStockTrackers());
        });

        $rootScope.$on('$routeChangeStart', function (event, next, current) {
                console.log("$routeChangeStart");
                $q.all([
                        toggleSpinner(true),
                        getApplicationUser()
                    ])
                    .then(function () {

                        toggleSpinner(false);
                        confirmUserAuthentication();
                });
            }
        );

        $rootScope.$on('$routeChangeSuccess', function(event, next, current) {
            console.log("$routeChangeSuccess");
            $q.all([
                toggleSpinner(true),
                getApplicationUser()
            ])
                .then(function () {

                    toggleSpinner(false);
                    confirmUserAuthentication();
            });
        });

        function toggleSpinner(on) { vm.isBusy = on; }

        $rootScope.$on(events.controllerActivateSuccess,
            function (data) { toggleSpinner(false); }
        );

        $rootScope.$on(events.spinnerToggle,
            function (data) { toggleSpinner(data.show); }
        );

        function getApplicationUser() {
            return $q.when(authenticationService.user())
                .then(function (data) { applicationUser = data; });
        }

        function checkForAuthenticatedUserAndLoadStockTrackers() {

            return $q.when(authenticationService.user())
                .then(function (data) { applicationUser = data; })
                .then(function () {

                    if (applicationUser.authenticated()) {
                        log("User " + applicationUser.userName() + " logged in.");
                    }
                })
                .then(function () {

                    if (applicationUser.authenticated()) {

                        var stockTrackers = [];
                        stockTrackerService.getStockTrackers()
                            .then(function (data) { stockTrackers = data; })
                            .then(function() {
                            
                                if (stockTrackers.length > 0) {
                                    
                                    log("Loading " + stockTrackers.length + " stock tracker(s).");

                                    angular.forEach(stockTrackers, function (value, key) {
                                        common.eventRaiser(commonConfig.config.stockTrackerLoadedEvent,
                                            { stockTracker: value });
                                    });
                                }
                            })
                            .then(function () {

                                common.eventRaiser(commonConfig.config.userAuthenticationStatusChangedEvent,
                                    { authenticated: true, userName: applicationUser.userName() });

                            });
                    } else {

                        common.eventRaiser(commonConfig.config.userAuthenticationStatusChangedEvent,
                                    { authenticated: false });

                    }
                });
        }

        function confirmUserAuthentication() {
            // check if user is authenticated, and alert listeners
            if (applicationUser.authenticated() === true) {
                common.eventRaiser(commonConfig.config.userAuthenticationStatusChangedEvent, { authenticated: true, userName: applicationUser.userName() });
            } else {
                common.eventRaiser(commonConfig.config.userAuthenticationStatusChangedEvent, { authenticated: false });
                $location.path("/home");
            }
            
        }
    };
})();