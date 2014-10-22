(function() {
    'use strict';

    angular.module("services")
        .factory("headlinesService", [
            '$http',
            'ngStocksTrackerApiSettings',
            headlinesService]);

    function headlinesService($http, ngStocksTrackerApiSettings) {

        var service = { getHeadlinesForStock: getHeadlinesForStock }
        return service;

        function getHeadlinesForStock(ticker) {

            var url = ngStocksTrackerApiSettings.apiServiceBaseUri + "/" + ngStocksTrackerApiSettings.stocksApiPrefix + "/" + ticker.trim() + "/" + ngStocksTrackerApiSettings.headlinesApiPrefix;
            return $http.get(url)
                .then(function(data) { return data.data; });
        }
    }
})();