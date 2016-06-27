
angular.module('BrewMatic').controller('newController', ['$scope', '$window', 'BrewGuideService', '$state', function ($scope, $window, service, $state) {
    'use strict';

    var self = this;

    self.loadData = function () {
        service.getDefaultSetup().then(function (result) {
            $scope.setup = result;
            console.log("test");
        });
    };

    self.loadData();

    $scope.startNewBrew = function () {
        service.startNewBrew($scope.setup).then(function (newId) {
            $state.go("displayBrew", { brewId: newId });
            console.log({ "test": newId });
        });
    };

}]);