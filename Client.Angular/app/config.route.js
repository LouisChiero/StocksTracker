(function () {
    'use strict';

    angular.module('stocksTrackerApp')
    
    // Configure the routes and route resolvers
    .config(['$routeProvider', function ($routeProvider) {

        getRoutes().forEach(function (r) {
            $routeProvider.when(r.url, r.config);
        });

        $routeProvider.otherwise({ redirectTo: '/home' });
    }]);

    // Define the routes 
    function getRoutes() {
        return [
            {
                url: '/home',
                config: {
                    templateUrl: 'app/views/landingPage.html',
                    reloadOnSearch: false,
                    title: 'home',
                    settings: {
                        nav: 1,
                        content: '<i class="fa fa-dashboard"></i> Dashboard'
                    }
                }
            }, {
                url: '/stocktracker/:stocktrackerid',
                config: {
                    templateUrl: 'app/views/stockTracker.html',
                    reloadOnSearch: false,
                    title: 'stocktracker',
                    settings: {
                        nav: 1,
                        content: '<i class="fa fa-dashboard"></i> Dashboard'
                    }
                }
            }, {
                url: '/stock/:ticker',
                config: {
                    title: 'stock',
                    templateUrl: 'app/views/stock.html',
                    reloadOnSearch: false,
                    settings: {
                        nav: 2,
                        content: '<i class="fa fa-lock"></i> Admin'
                    }
                }
            }, {
                url: '/stock/:ticker/charts',
                config: {
                    title: 'charts',
                    templateUrl: 'app/views/charts.html',
                    reloadOnSearch: false,
                    settings: {
                        nav: 2,
                        content: '<i class="fa fa-lock"></i> Admin'
                    }
                }
            }, {
                url: '/register',
                config: {
                    title: 'charts',
                    templateUrl: 'app/views/register.html',
                    reloadOnSearch: false,
                    settings: {
                        nav: 2,
                        content: '<i class="fa fa-lock"></i> Admin'
                    }
                }
            }, {
                url: '/login',
                config: {
                    title: 'charts',
                    templateUrl: 'app/views/login.html',
                    reloadOnSearch: false,
                    settings: {
                        nav: 2,
                        content: '<i class="fa fa-lock"></i> Admin'
                    }
                }
            }
        ];
    }
})();