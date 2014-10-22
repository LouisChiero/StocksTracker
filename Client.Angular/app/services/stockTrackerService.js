(function () {
    'use strict';

    angular.module("services")
        .factory("stockTrackerService", [
            'common',
            'commonConfig',
            '$http',
            'ngStocksTrackerApiSettings',
            stocksTrackerService]);
  
    function stocksTrackerService(common, commonConfig, $http, ngStocksTrackerApiSettings) {

        var service = {
            getStockTrackers: getStockTrackers,
            getStockTracker: getStockTracker,
            getStock: getStock,
            createStockTracker: createStockTracker,
            updateStockTracker: updateStockTracker,
            deleteStockTracker: deleteStockTracker,
            removeStockFromStockTracker: removeStockFromStockTracker,
            addStockToStockTracker: addStockToStockTracker,
            searchForStock: searchForStock
        };

        return service;

        function getStockTrackers() {

            return $http.get(ngStocksTrackerApiSettings.apiServiceBaseUri + "/" + ngStocksTrackerApiSettings.stockTrackersApiPrefix)
                .then(function(data) { return data.data; });
        }

        function getStockTracker(stockTrackerId) {
            
            return $http.get(ngStocksTrackerApiSettings.apiServiceBaseUri + "/" + ngStocksTrackerApiSettings.stockTrackersApiPrefix + "/" + stockTrackerId)
                .then(function(data) { return data.data; });
        }

        function getStock(symbol) {
            
            return $http.get(ngStocksTrackerApiSettings.apiServiceBaseUri + "/" + ngStocksTrackerApiSettings.stocksApiPrefix + "/" + angular.uppercase(symbol.trim()))
                .then(function (data) { return data.data; });
        }

        function createStockTracker(newStockTrackerName) {
            
            return $http.post(ngStocksTrackerApiSettings.apiServiceBaseUri + "/" + ngStocksTrackerApiSettings.stockTrackersApiPrefix + "/Create", { StockTrackerName: newStockTrackerName.trim() })
                .then(function (data) {
                    var newStockTracker = data.data;
                    common.eventRaiser(commonConfig.config.stockTrackerCreatedEvent, { newStockTracker: newStockTracker });
                    return newStockTracker;
            });
        }

        function updateStockTracker(updateOptions) {
            
            return $http.put(ngStocksTrackerApiSettings.apiServiceBaseUri + "/" + ngStocksTrackerApiSettings.stockTrackersApiPrefix + "/" + updateOptions.stockTrackerId,
                    { StockTrackerName: updateOptions.stockTrackerName.trim(), Stocks: updateOptions.stocks ? updateOptions.stocks : [] })
                .then(function (data) {
                    var updatedStockTracker = data.data;
                    common.eventRaiser(commonConfig.config.stockTrackerUpdatedEvent, { updatedStockTracker: updatedStockTracker });
                    return updatedStockTracker;
                });
        }

        function deleteStockTracker(stockTrackerId) {
            
            return $http.delete(ngStocksTrackerApiSettings.apiServiceBaseUri + "/" + ngStocksTrackerApiSettings.stockTrackersApiPrefix + "/" + stockTrackerId)
                .then(function () {
                    common.eventRaiser(commonConfig.config.stockTrackerRemovedEvent, { removedStockTrackerId: stockTrackerId });
                });
        }

        function removeStockFromStockTracker(stockTracker, stockToRemove) {
            // collection of stocks minus the one being removed
            var stocks = common.underscore.without(stockTracker.stocks, stockToRemove);
            // map stocks to new array that HTTP call is expecting
            var newStocks = common.underscore.map(stocks, function(s) {
                return { StockId: s.id };
            });
            
            // use existing update functionality
            return updateStockTracker({stockTrackerId: stockTracker.id, stockTrackerName: stockTracker.name, stocks: newStocks});
        }

        function addStockToStockTracker(stockTracker, stockToAdd) {
            // collection of stocks plus the one being added
            var stocks = stockTracker.stocks.concat([stockToAdd]);
            // map stocks to new array that HTTP call is expecting
            var newStocks = common.underscore.map(stocks, function (s) {
                return { StockId: s.id };
            });

            // use existing update functionality
            return updateStockTracker({ stockTrackerId: stockTracker.id, stockTrackerName: stockTracker.name, stocks: newStocks });
        }

        function searchForStock(searchTerm) {
            
            return $http.get(ngStocksTrackerApiSettings.apiServiceBaseUri + "/" + ngStocksTrackerApiSettings.searchApiPrefix + "/" + searchTerm)
                .then(function (data) { return data.data; });
        }
    }
})();