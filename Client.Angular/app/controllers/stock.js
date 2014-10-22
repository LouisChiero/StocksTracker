(function () {
    'use strict';
    var controllerId = 'stock';
    angular.module('stocksTrackerApp')
        .controller(controllerId, [
            '$routeParams',
            '$location',
            'common',
            'stockTrackerService',
            'headlinesService',
            'bootstrap.addToStockTrackerDialogService',
            stock]);

    function stock($routeParams, $location, common, stockTrackerService, headlinesService, dialog) {
        // logging
        var getLogFn = common.logger.getLogFn,
            log = getLogFn(controllerId),
            tickerSymbol = $routeParams.ticker,
        // bindings
            vm = this;

        vm.stock = {};
        vm.headlines = {};
        vm.showCharts = function() {
            $location.path("/stock/" + tickerSymbol + "/charts");
        };
        vm.addToStockTracker = addToStockTracker;

        activate();

        function activate() {
            common.activateController(getStock(), controllerId)
                .then(function() { log("Viewing " + tickerSymbol + "."); })
                .then(function() { getStockHeadlines(); });
        }

        function getStock() {
            return stockTrackerService.getStock(tickerSymbol)
                .then(function (data) { return vm.stock = data; });
        }

        function getStockHeadlines() {
            return headlinesService.getHeadlinesForStock(tickerSymbol)
                .then(function(data) { return vm.headlines = data; });
        }

        function addToStockTracker() {
            // launch dialog
            dialog.showAddToStockTrackerDialog(vm.stock);
        }
    }
})();