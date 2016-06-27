
angular.module('BrewMatic').controller('displayController', ['$scope', '$window', 'BrewGuideService', '$stateParams', '$state', function ($scope, $window, service, $stateParams, $state) {
    'use strict';

    var self = this;

    var brewId = $stateParams.brewId;

    self.loadData = function () {
        service.getBrew(brewId).then(function (result) {
            $scope.brew = result;
        });
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