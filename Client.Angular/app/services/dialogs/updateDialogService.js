(function() {
    'use strict';

    angular.module('common.bootstrap')
        .factory('bootstrap.updateDialogService', ['$modal', updateDialog])
        // controller will bind scope to UI elements
        .controller('updateStockTracker', function () { });

    function updateDialog($modal) {
        var service = {
            showUpdateStockTrackerDialog: showUpdateStockTrackerDialog
        };

        return service;

        function showUpdateStockTrackerDialog(existingStockTracker) {
            
            var modalOptions = {
                templateUrl: 'app/dialogs/updateDialog.html',
                controller: modalInstance,
                keyboard: true,
                size: 'sm',
                resolve: {
                    options: function () {
                        return {
                            title: "Update Stock Tracker",
                            saveText: "Save",
                            cancelText: "Cancel",
                            stockTrackerName: existingStockTracker.name,
                            stockTrackerId: existingStockTracker.id
                        };
                    }
                }
            };

            return $modal.open(modalOptions).result;
        }
    };

    var modalInstance = ['$scope', '$modalInstance', 'options', 'stockTrackerService',
        function ($scope, $modalInstance, options, stockTrackerService) {

            $scope.title = options.title || 'Update Stock Tracker';
            $scope.saveText = options.saveText || 'Save';
            $scope.cancelText = options.cancelText || 'Cancel';
            $scope.stockTrackerName = options.stockTrackerName;
            $scope.cancel = function () { $modalInstance.dismiss('cancel'); };

            $scope.ok = function (stockTrackerForm) {
                if (stockTrackerForm.$valid) {
                    stockTrackerService.updateStockTracker({ stockTrackerId: options.stockTrackerId, stockTrackerName: stockTrackerForm.stockTrackerName.$modelValue })
                        .then(function () {
                            $modalInstance.close('ok');
                        });
                }
            };
        }];
})();