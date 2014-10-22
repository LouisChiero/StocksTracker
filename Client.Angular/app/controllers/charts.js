(function() {
    'use strict';
    var controllerId = 'charts';

    angular.module('stocksTrackerApp').controller(controllerId, ['common', 'chartsService', '$routeParams', charts]);

    function charts(common, chartsService, $routeParams) {
        var getLogFn = common.logger.getLogFn,
            log = getLogFn(controllerId),
            tickerSymbol = $routeParams.ticker,
            vm = this;

        // query options
        vm.timeSpanOptions = [
            { option: "One Day", q: "OneDay" },
            { option: "Five Days", q: "FiveDays" },
            { option: "One Month", q: "OneMonth" },
            { option: "Three Months", q: "ThreeMonths" },
            { option: "Six Months", q: "SixMonths" },
            { option: "One Year", q: "OneYear" },
            { option: "Two Years", q: "TwoYears" },
            { option: "Five Years", q: "FiveYears" },
            { option: "Max", q: "Maximum" }];
        vm.typeOptions = [
            { option: "Bar", q: "Bar" },
            { option: "Line", q: "Line" },
            { option: "Candle", q: "Candle" }
        ];
        vm.sizeOptions = [
            { option: "Large", q: "Large" },
            { option: "Medium", q: "Middle" },
            { option: "Small", q: "Small" }
        ];

        // defaults on load
        vm.selectedTimeSpan = vm.timeSpanOptions[5]; // One Year
        vm.selectedType = vm.typeOptions[1]; // Line
        vm.selectedSize = vm.sizeOptions[0]; // Large

        vm.url = null;
        vm.ticker = tickerSymbol;
        vm.updateUrl = updateUrl;

        activate();

        function activate() {
            common.activateController(updateUrl(), controllerId)
                .then(function() { log("Loaded charts for " + tickerSymbol + "."); });
        }

        function updateUrl() {
            return chartsService.getStockChartForOptions(vm.ticker, vm.selectedTimeSpan.q, vm.selectedType.q, vm.selectedSize.q)
                .then(function (data) { return vm.url = data.url; });
        }
    }
})();