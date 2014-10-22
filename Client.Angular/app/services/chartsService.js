(function() {
    'use strict';

    angular.module("services")
        .factory("chartsService", [
            '$http',
            'ngStocksTrackerApiSettings',
            chartsService]);

    function chartsService($http, ngStocksTrackerApiSettings) {

        var service = { getStockChartForOptions: getStockChartForOptions };
        return service;

        function getStockChartForOptions(ticker, timeSpan, type, size) {
   
            var url = ngStocksTrackerApiSettings.apiServiceBaseUri + "/" + ngStocksTrackerApiSettings.stocksApiPrefix + "/" + ticker.trim() + "/" + ngStocksTrackerApiSettings.chartsApiPrefix;
            return $http({ method: 'GET', url: url, data: { ticker: ticker }, params: { 'timeSpan': timeSpan, 'chartType': type, 'chartSize': size }, responseType: 'json' })
                .then(function (data) { return data.data; });
        }
    }
})();