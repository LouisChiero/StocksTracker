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
                    getApplicationUser(),
                    //showLoggedInUser(),
                    confirmUserAuthentication(),
                    loadStockTrackers()
                ], controllerId)
                // put any post-loading action(s) here
            .then(function () {
                $q.when(showLoggedInUser())
                    .then(function () { confirmUserAuthentication(); })
                    //.then(function () { confirmUserAuthentication(); })
                    .then(function () { console.log("shell activated"); });
            });
        }

        $rootScope.$on(config.events.userLoggedOut, function (event, args) {
            common.eventRaiser(commonConfig.config.userAuthenticationStatusChangedEvent,
                { authenticated: false });
            // go to login page
            $location.path("/login");
        });

        $rootScope.$on(config.events.userLoggedIn, function (event, args) {
            $q.all([
                getApplicationUser(),
                confirmUserAuthentication(),
                showLoggedInUser(),
                loadStockTrackers()
            ])
                .then(function () { confirmUserAuthentication(); });
        });

        $rootScope.$on('$routeChangeStart', function (event, next, current) {
            $q.all([
                toggleSpinner(true),
                getApplicationUser()
            ])
                .then(function () { confirmUserAuthentication(); });
        });

        $rootScope.$on('$routeChangeSuccess', function(event, current, previous) {
            $q.all([
                toggleSpinner(false),
                getApplicationUser()
            ])
                .then(function () { confirmUserAuthentication(); });
        });

        $rootScope.$on('$routeChangeError', function(event, current, previous, rejection) {
            toggleSpinner(false);
        });

        function toggleSpinner(on) {
            vm.isBusy = on;
        }

        $rootScope.$on(events.controllerActivateSuccess,
            function (data) { toggleSpinner(false); }
        );

        $rootScope.$on(events.spinnerToggle,
            function (event, args) {
                toggleSpinner(args.show);
            }
        );

        function getApplicationUser() {
            return $q.when(authenticationService.user())
                .then(function (data) { applicationUser = data; });
        }

        function showLoggedInUser() {
            return $q.when().then(function() {
                if (applicationUser.authenticated() === true) {
                    log("User " + applicationUser.userName() + " logged in.");
                }
            });
        }

        function loadStockTrackers() {

            return $q.when(stockTrackerService.getStockTrackers())
                .then(function (data) {

                    if (data.length > 0) {

                        log("Loading " + data.length + " stock tracker(s).");

                        angular.forEach(data, function (value, key) {
                            common.eventRaiser(commonConfig.config.stockTrackerLoadedEvent,
                            { stockTracker: value });
                        });
                        
                    }
                    
            });
        }

        function confirmUserAuthentication() {
            return $q.when().then(function () {

                // check if user is authenticated, and alert listeners
                if (applicationUser.authenticated() === true) {
                    common.eventRaiser(commonConfig.config.userAuthenticationStatusChangedEvent, { authenticated: true, userName: applicationUser.userName() });
                } else {
                    common.eventRaiser(commonConfig.config.userAuthenticationStatusChangedEvent, { authenticated: false });
                }

            });
        }
    };
})();