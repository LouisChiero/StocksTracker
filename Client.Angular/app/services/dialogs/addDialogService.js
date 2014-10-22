(function () {
    'use strict';

    angular.module('common.bootstrap')
        .factory('bootstrap.addDialogService', ['$modal', addDialog])
        // controller will bind scope to UI elements
        .controller('addStockTracker', function() {});

    function addDialog($modal) {
        var service = {
            showAddStockTrackerDialog: showAddStockTrackerDialog
        };

        return service;

        function showAddStockTrackerDialog() {

            var modalOptions = {
                templateUrl: 'app/dialogs/addDialog.html',
                controller: modalInstance,
                keyboard: true,
                size: 'sm',
                resolve: {
                    options: function () {
                        return {
                            title: "New Stock Tracker",
                            saveText: "Save",
                            cancelText: "Cancel"
                        };
                    }
                }
            };

            return $modal.open(modalOptions).result;
        }
    }

    var modalInstance = ['$scope', '$modalInstance', 'options', 'stockTrackerService',
        function ($scope, $modalInstance, options, stockTrackerService) {

            $scope.title = options.title || 'New Stock Tracker';
            $scope.saveText = options.saveText || 'Save';
            $scope.cancelText = options.cancelText || 'Cancel';
            $scope.newStockTrackerName = '';
            $scope.cancel = function () { $modalInstance.dismiss('cancel'); };

            $scope.ok = function (stockTrackerForm) {
                if (stockTrackerForm.$valid) {
                    stockTrackerService.createStockTracker(stockTrackerForm.newStockTrackerName.$modelValue)
                        .then(function () {
                            $modalInstance.close('ok');
                    });
                } 
            };
        }];
})();