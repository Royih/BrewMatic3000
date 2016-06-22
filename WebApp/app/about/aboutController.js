
angular.module('BrewMatic').controller('aboutController', ['$scope', '$window', function ($scope, $window) {
    'use strict';

    var self = this;

    self.loadData = function () {
        console.log("hello world. about");
    };

    self.loadData();

}]);