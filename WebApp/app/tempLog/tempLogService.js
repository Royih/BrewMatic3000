
angular.module('BrewMatic').service('tempLogService', ['$http', '$q', 'ngAuthSettings', 'pageHelperService', function ($http, $q, ngAuthSettings, pageHelperService) {
    'use strict';

    var self = this;

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    
    self.getBrewLogs = function () {
        return $q(function (resolve, reject) {
            $http.get(serviceBase + 'brewStatusLog/get50latest').success(function (result) {
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