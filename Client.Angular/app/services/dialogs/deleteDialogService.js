(function() {
    'use strict';

    angular.module('common.bootstrap')
        .factory('bootstrap.deleteDialogService', ['$modal', deleteDialog])
        // controller will bind scope to UI elements
        .controller('deleteStockTracker', function () { });

    function deleteDialog($modal) {
        var service = {
            showDeleteStockTrackerDialog: showDeleteStockTrackerDialog
        };

        return service;

        function showDeleteStockTrackerDialog(existingStockTracker) {

            var modalOptions = {
                templateUrl: 'app/dialogs/deleteDialog.html',
                controller: modalInstance,
                keyboard: true,
                size: 'sm',
                resolve: {
                    options: function () {
                        return {
                            title: "Delete Stock Tracker " + existingStockTracker.name + "?",
                            okText: "OK",
                            cancelText: "Cancel",
                            stockTrackerId: existingStockTracker.id
                        };
                    }
                }
            };

            return $modal.open(modalOptions).result;
        }
    }

    var modalInstance = ['$scope', '$modalInstance', 'options', 'stockTrackerService',
        function ($scope, $modalInstance, options, stockTrackerService) {

            $scope.title = options.title || 'Delete Stock Tracker?';
            $scope.okText = options.okText || 'OK';
            $scope.cancelText = options.cancelText || 'Cancel';
            $scope.cancel = function () { $modalInstance.dismiss('cancel'); };

            $scope.ok = function () {
                stockTrackerService.deleteStockTracker(options.stockTrackerId)
                    .then(function () {
                        $modalInstance.close('ok');
                    });
            };
        }];
})();