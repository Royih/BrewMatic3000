
angular.module('BrewMatic').controller('currentController', ['$scope', '$window', 'BrewGuideService', '$state', function ($scope, $window, service, $state) {
    'use strict';

    var self = this;

    self.loadData = function () {
        service.getCurrentBrew().then(function (brewId) {
            $state.go("displayBrew", { brewId: brewId });
        });
    };

    self.loadData();

}]);