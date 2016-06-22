
angular.module('BrewMatic').controller('homeController', ['$scope', '$window', '$timeout', 'homeControllerService', function ($scope, $window, $timeout, service) {
    'use strict';

    var self = this;

    self.loadLastLog = function () {
        service.getLastLog().then(function (result) {
            $scope.lastLog = result;
        });
    };

    self.loadData = function () {
        self.loadLastLog();
        service.getTargetTemperature().then(function (result) {
            $scope.targetTemperature = result;

            $scope.$watchGroup(['targetTemperature.target1', 'targetTemperature.target2'], function (newValues, oldValues, scope) {
                service.saveTargetTemperature(newValues[0], newValues[1]).then(function (result) {
                    console.log("Changes was saved");
                });
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