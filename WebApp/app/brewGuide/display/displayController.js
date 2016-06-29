
angular.module('BrewMatic').controller('displayController', ['$scope', '$window', 'BrewGuideService', '$stateParams', '$state', '$timeout', function ($scope, $window, service, $stateParams, $state, $timeout) {
    'use strict';

    var self = this;

    var brewId = $stateParams.brewId;

    self.loadData = function () {
        service.getBrew(brewId).then(function (result) {
            $scope.brew = result;
            $scope.startTime = result.currentStep.startTime;
            self.updateStepTime();
            if (result.currentStep.completeTime) {
                $scope.completeTime = result.currentStep.completeTime;
                self.updateCountdown();
            }
            service.getDataCapture(result.currentStep.id).then(function (dataCaptureValues) {
                $scope.dataCaptureValues = dataCaptureValues;
            });

        });
        service.getBrewHistory(brewId).then(function (result) {
            $scope.brewHistory = result;
        });
    };

    self.updateStepTime = function () {
        $timeout(function () {
            var startTime = moment($scope.startTime);
            $scope.stepTime = moment.utc(moment().diff(startTime)).format("HH:mm:ss");
            self.updateStepTime();
        }, 1000);
    };

    self.updateCountdown = function () {
        $timeout(function () {
            var endTime = moment($scope.completeTime);
            $scope.countDown = moment.utc(endTime.diff(moment())).format("HH:mm:ss");
            self.updateCountdown();
        }, 1000);
    };

    self.loadData();

    $scope.goToNextStep = function () {
        service.goToNextStep(brewId).then(function (result) {
            $state.go("displayBrew", { brewId: brewId }, { reload: true });
        });
    };

    $scope.goBackOneStep = function () {
        if (confirm("Are you sure you want to abort the current step and return to the previous?")) {
            service.goBackOneStep(brewId).then(function (result) {
                $state.go("displayBrew", { brewId: brewId }, { reload: true });
            });
        }

    };

}]);