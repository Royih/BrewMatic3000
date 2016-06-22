
angular.module('BrewMatic').service('homeControllerService', ['$http', '$q', 'ngAuthSettings', 'pageHelperService', function ($http, $q, ngAuthSettings, pageHelperService) {
    'use strict';

    var self = this;

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    self.getLastLog = function () {
        return $q(function (resolve, reject) {
            $http.get(serviceBase + 'brewStatusLog').success(function (result) {
                resolve(result);
            }).error(pageHelperService.handleError);
        });
    };

    self.getTargetTemperature = function () {
        return $q(function (resolve, reject) {
            $http.get(serviceBase + 'targetTemperature').success(function (result) {
                resolve(result);
            }).error(pageHelperService.handleError);
        });
    };

    self.saveTargetTemperature = function (target1, target2) {
        return $q(function (resolve, reject) {
            $http.post(serviceBase + 'targetTemperature', { target1: target1, target2: target2 }).success(function (result) {
                resolve(result);
            }).error(pageHelperService.handleError);
        });
    };

    /*self.saveSomething = function (something) {
        return $q(function (resolve, reject) {
            $http.post(serviceBase + 'api/something/save', something).success(function (result) {
                resolve(result);
                pageHelperService.pushOkMessage("Something was saved successfully");
            }).error(pageHelperService.handleError);
        });
    };

    self.listSomething = function () {
        return $q(function (resolve, reject) {
            $http.get(serviceBase + 'api/something/list').success(function (result) {
                resolve(result);
            }).error(pageHelperService.handleError);
        });
    };
    */

}]);