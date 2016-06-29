
angular.module('BrewMatic').service('BrewGuideService', ['$http', '$q', 'ngAuthSettings', 'pageHelperService', function ($http, $q, ngAuthSettings, pageHelperService) {
    'use strict';

    var self = this;

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    self.getDefaultSetup = function () {
        return $q(function (resolve, reject) {
            $http.get(serviceBase + 'brewGuide/GetDefaultSetup').success(function (result) {
                resolve(result);
            }).error(pageHelperService.handleError);
        });
    };

    self.getBrew = function (brewId) {
        return $q(function (resolve, reject) {
            $http.get(serviceBase + 'brewGuide/' + brewId).success(function (result) {
                resolve(result);
            }).error(pageHelperService.handleError);
        });
    };

    self.getBrewHistory = function (brewId) {
        return $q(function (resolve, reject) {
            $http.get(serviceBase + 'brewGuide/getBrewHistory/' + brewId).success(function (result) {
                resolve(result);
            }).error(pageHelperService.handleError);
        });
    };

    self.getCurrentBrew = function () {
        return $q(function (resolve, reject) {
            $http.get(serviceBase + 'brewGuide/getLatest').success(function (result) {
                resolve(result);
            }).error(pageHelperService.handleError);
        });
    }

    self.getDataCapture = function (brewStepId) {
        return $q(function (resolve, reject) {
            $http.get(serviceBase + 'datacapture/'+brewStepId).success(function (result) {
                resolve(result);
            }).error(pageHelperService.handleError);
        });
    }

    self.startNewBrew = function (setup) {
        return $q(function (resolve, reject) {
            $http.post(serviceBase + 'brewGuide/StartNewBrew', setup).success(function (result) {
                resolve(result);
                pageHelperService.pushOkMessage("Brew was started successfully");
            }).error(pageHelperService.handleError);
        });
    };

    self.goToNextStep = function (brewId) {
        return $q(function (resolve, reject) {
            $http.post(serviceBase + 'brewGuide/goToNextStep', brewId).success(function (result) {
                resolve(result);
                pageHelperService.pushOkMessage("Successfully moved to next step");
            }).error(pageHelperService.handleError);
        });
    };

    self.goBackOneStep = function (brewId) {
        return $q(function (resolve, reject) {
            $http.post(serviceBase + 'brewGuide/goBackOneStep', brewId).success(function (result) {
                resolve(result);
                pageHelperService.pushOkMessage("Successfully moved to previous step");
            }).error(pageHelperService.handleError);
        });
    };

    /*
        self.listSomething = function () {
            return $q(function (resolve, reject) {
                $http.get(serviceBase + 'api/something/list').success(function (result) {
                    resolve(result);
                }).error(pageHelperService.handleError);
            });
        };
        */

}]);