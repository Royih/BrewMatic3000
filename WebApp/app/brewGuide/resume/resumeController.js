
angular.module('BrewMatic').controller('resumeController', ['$scope', '$window', 'BrewGuideService', '$state', function ($scope, $window, service, $state) {
    'use strict';

    var self = this;

    self.loadData = function () {
        service.getCurrentBrew().then(function (brewId) {
            if (brewId > 0) {
                $state.go("displayBrew", { brewId: brewId });
            }
            else {
                $scope.nothingToResume = true;
            }
        });
    };

    self.loadData();

}]);