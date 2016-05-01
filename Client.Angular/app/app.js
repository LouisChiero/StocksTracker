(function () {
    
    angular.module('stocksTrackerApp', [
        // Angular modules 
        'ngAnimate',        // animations
        'ngRoute',          // routing

        // Custom modules 
        'common',           // common functions, logger, spinner
        'common.bootstrap', // bootstrap dialog wrapper functions
        'services', // provider of HTTP services

        // 3rd Party Modules
        'ui.bootstrap'      // ui-bootstrap (ex: carousel, pagination, dialog)
    ])
    .constant('ngStocksTrackerApiSettings', {
        apiTokenUri: 'https://stockstrackerapi.azurewebsites.net/Token',
        apiServiceBaseUri: 'https://stockstrackerapi.azurewebsites.net/api',
        accountApiPrefix: 'Account',
        stocksApiPrefix: 'Stocks',
        stockTrackersApiPrefix: 'StockTrackers',
        headlinesApiPrefix: 'Headlines',
        chartsApiPrefix: 'Charts',
        searchApiPrefix: 'Search'
    })
    // Handle routing errors and success events
    .run(['$route', 'common', 'authenticationService', function ($route, common, authenticationService) {
        // Include $route to kick start the router.

        // load authentication data
        common.$q.when(authenticationService.bootstrapAuthentication());
    }]);
})();