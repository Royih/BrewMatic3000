
angular.module('BrewMatic').controller('tempLogController', ['$scope', '$window', 'tempLogService', function ($scope, $window, tempLogService) {
    'use strict';

    var self = this;

    self.loadData = function () {
        tempLogService.getBrewLogs().then(function (result) {
            $scope.brewLogs = result;
        });
    };

    self.loadData();

}]);