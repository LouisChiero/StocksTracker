(function() {
    'use strict';

    angular.module('common.bootstrap')
        .factory('bootstrap.addToStockTrackerDialogService', ['$modal', addToStockTrackerDialog])
        // controller will bind scope to UI elements
        .controller('addToStockTracker', function () { });

    function addToStockTrackerDialog($modal) {
        var service = {
            showAddToStockTrackerDialog: showAddToStockTrackerDialog
        };

        return service;

        function showAddToStockTrackerDialog(stock) {
            
            var modalOptions = {
                templateUrl: 'app/dialogs/addToStockTrackerDialog.html',
                controller: modalInstance,
                keyboard: true,
                size: 'sm',
                resolve: {
                    options: function () {
                        return {
                            title: "Add to Stock Tracker",
                            saveText: "Save",
                            cancelText: "Cancel",
                            stock: stock
                        };
                    }
                }
            };

            return $modal.open(modalOptions).result;
        }
    }

    var modalInstance = ['$scope', '$modalInstance', 'options', 'common', 'stockTrackerService',
        function ($scope, $modalInstance, options, common, stockTrackerService) {

            $scope.items = [];
            stockTrackerService.getStockTrackers().then(function (data) {
                $scope.items = common.underscore.map(data, function(st) {
                    return { name: st.name, st: st };
                });
            });

            $scope.title = options.title || 'Add To Stock Tracker';
            $scope.saveText = options.saveText || 'Save';
            $scope.cancelText = options.cancelText || 'Cancel';
            $scope.cancel = function () { $modalInstance.dismiss('cancel'); };

            $scope.ok = function (stockTrackerForm) {
                if (stockTrackerForm.$valid) {
                    stockTrackerService.addStockToStockTracker(stockTrackerForm.selectedStockTracker.$modelValue.st, options.stock)
                        .then(function () {
                            $modalInstance.close('ok');
                        });
                }
            };
        }];
})();