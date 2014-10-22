(function () {
    'use strict';
    var controllerId = 'stocktracker';
    angular.module('stocksTrackerApp')
        .controller(controllerId, [
            '$rootScope',
            '$routeParams',
            'common',
            'config',
            'stockTrackerService',
            'chartsService',
            'bootstrap.addToStockTrackerDialogService',
            stockTracker]);

    function stockTracker($rootScope, $routeParams, common, config, stockTrackerService, chartsService, dialog) {
        // logging
        var getLogFn = common.logger.getLogFn,
            log = getLogFn(controllerId),
        // parameter
            stockTrackerId = $routeParams.stocktrackerid,
        // bindings
            vm = this;

        vm.stockTracker = {};
        vm.removeStock = removeStock;
        vm.addToStockTracker = addToStockTracker;

        activate();

        function activate() {
            common.activateController(getStockTracker(), controllerId)
                .then(function () {
                    log("Loaded " + vm.stockTracker.name + ".");
            });
        }

        $rootScope.$on(config.events.stockTrackerUpdated, function (event, args) {
            // only update active tracker
            if (vm.stockTracker.id == args.updatedStockTracker.id) {
                vm.stockTracker.name = args.updatedStockTracker.name;
            }
        });
        
        function getStockTracker() {
            return stockTrackerService.getStockTracker(stockTrackerId)
                .then(function (data) {
                    vm.stockTracker = data;
                    angular.forEach(vm.stockTracker.stocks, function (value, key) {
                        // calculated on the fly
                        value.url = "/stock/" + value.ticker;
                        value.dayGainOrLoss = calculateDayGainOrLoss(value);
                        value.dayGainOrLossPercent = calculateDayGainOrLossPercent(value);
                        chartsService.getStockChartForOptions(value.ticker, "OneDay", "Line", "Small")
                        .then(function(chartData) {
                            value.imageSource = chartData.url;
                        });
                    });
            });
        }

        function calculateDayGainOrLoss(stock) {
            
            if (stock.lastTrade && stock.previousClose) {
                return stock.lastTrade - stock.previousClose;
            }

            return 0;
        }

        function calculateDayGainOrLossPercent(stock) {

            if (stock.previousClose) {
                return (calculateDayGainOrLoss(stock) / stock.previousClose) * 100;
            }

            return 0;
        }

        function removeStock(stock) {
            return stockTrackerService.removeStockFromStockTracker(vm.stockTracker, stock)
                .then(function () {
                    // remove succeeded - remove stock from array
                    vm.stockTracker.stocks = common.underscore.without(vm.stockTracker.stocks, stock);
            });
        }

        function addToStockTracker(stock) {
            // launch dialog
            dialog.showAddToStockTrackerDialog(stock);
        }
    }
})();