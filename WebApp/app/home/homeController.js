
angular.module('BrewMatic').controller('homeController', ['$scope', '$window', '$timeout', 'homeControllerService', function ($scope, $window, $timeout, service) {
    'use strict';

    var self = this;
    var changeTimeout;

    self.loadLastLog = function () {
        service.getLastLog().then(function (result) {
            $scope.lastLog = result;
        });
    };
    
    self.updateIfNotChangedAfterNSeconds = function (newValue1, newValue2, seconds) {
        if (changeTimeout) $timeout.cancel(changeTimeout);
        changeTimeout = $timeout(function () {
            service.saveTargetTemperature(newValue1, newValue2).then(function (result) {
                console.log("Changes was saved");
            });
        }, seconds * 1000); // delay n seconds
    };
    

    self.loadData = function () {
        self.loadLastLog();
        service.getTargetTemperature().then(function (result) {
            $scope.targetTemperature = result;

            $scope.$watchGroup(['targetTemperature.target1', 'targetTemperature.target2'], function (newValues, oldValues, scope) {
                var newTemp1 = newValues[0];
                var newTemp2 = newValues[1];
                var oldTemp1 = oldValues[0];
                var oldTemp2 = oldValues[1];

                console.log({ newTemp1: newTemp1, oldTemp1: oldTemp1, newTemp2: newTemp2, oldTemp2: oldTemp2 })
                if (newTemp1 !== oldTemp1 || newTemp2 !== oldTemp2) {
                    console.log("Changes found");
                    if (newTemp1 && newTemp1 !== "" && newTemp2 && newTemp2 !== "") {
                        console.log("Input found");
                        console.log(angular.isNumber(parseFloat(newTemp1)));
                        if (angular.isNumber(parseFloat(newTemp1)) && angular.isNumber(parseFloat(newTemp2))) {
                            console.log("Numeric input found");
                            self.updateIfNotChangedAfterNSeconds(newTemp1, newTemp2, 3);
                        }
                    }
                }
            });
        });
    };

    self.loadData();

    var poll = function () {
        $timeout(function () {
            self.loadLastLog();
            poll();
        }, 1000);
    };
    poll();

}]);