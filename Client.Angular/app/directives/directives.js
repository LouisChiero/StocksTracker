(function() {
    "use strict";

    angular.module('stocksTrackerApp')
        .directive("stSpinner", ['$window', function($window) {
            
            var directive = {
                link: link,
                restrict: 'A'
            };
            return directive;

            function link(scope, element, attrs) {
                scope.spinner = null;
                scope.$watch(attrs.stSpinner, function (options) {

                    if (scope.spinner) {
                        scope.spinner.stop();
                    }
                    console.log("spinner options");
                    console.log(options);
                    scope.spinner = new $window.Spinner(options);
                    scope.spinner.spin(element[0]);
                }, true);
            }

        }
    ]);

})();