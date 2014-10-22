(function () {
    'use strict';

    // For use with the HotTowel-Angular-Breeze add-on that uses Breeze
    var remoteServiceName = 'breeze/Breeze';

    var events = {
        controllerActivateSuccess: 'controller.activateSuccess',
        spinnerToggle: 'spinner.toggle',
        stockTrackerCreated: 'stocktracker.created',
        stockTrackerUpdated: 'stocktracker.updated',
        stockTrackerRemoved: 'stocktracker.removed',
        stockTrackerLoaded: 'stocktracker.loaded',
        stockAdded: 'stock.added',
        stockRemoved: 'stock.removed',
        userAuthenticationStatusChanged: 'user.authenticationchanged',
        userLoggedOut: 'user.loggedout',
        userLoggedIn: 'user.loggedin'
    };

    var config = {
        appErrorPrefix: '[ST Error] ', //Configure the exceptionHandler decorator
        docTitle: 'Stocks Tracker: ',
        events: events,
        remoteServiceName: remoteServiceName,
        version: '1.0.0'
    };

    angular.module('stocksTrackerApp')
    .value('config', config)
    .config(['$logProvider', function ($logProvider) {
        // turn debugging off/on (no info or warn)
        if ($logProvider.debugEnabled) {
            $logProvider.debugEnabled(true);
        }
    }])
    //#region Configure the common services via commonConfig
    .config(['commonConfigProvider', function (cfg) {
        cfg.config.controllerActivateSuccessEvent = config.events.controllerActivateSuccess;
        cfg.config.spinnerToggleEvent = config.events.spinnerToggle;
        cfg.config.stockTrackerCreatedEvent = config.events.stockTrackerCreated;
        cfg.config.stockTrackerUpdatedEvent = config.events.stockTrackerUpdated;
        cfg.config.stockTrackerRemovedEvent = config.events.stockTrackerRemoved;
        cfg.config.stockTrackerLoadedEvent = config.events.stockTrackerLoaded;
        cfg.config.stockAddedEvent = config.events.stockAdded;
        cfg.config.stockRemovedEvent = config.events.stockRemoved;
        cfg.config.userAuthenticationStatusChangedEvent = config.events.userAuthenticationStatusChanged;
        cfg.config.userLoggedOutEvent = config.events.userLoggedOut;
        cfg.config.userLoggedInEvent = config.events.userLoggedIn;
    }])
    //#endregion

    // configure HTTP interceptors
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push('httpInterceptorService');
    });

    // Configure Toastr
    toastr.options.timeOut = 4000;
    toastr.options.positionClass = 'toast-bottom-right';
})();