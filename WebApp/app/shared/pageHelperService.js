angular.module("BrewMatic").service('pageHelperService', ['$http', '$q', '$rootScope', 'ngAuthSettings', function ($http, $q, $rootScope, ngAuthSettings) {
    'use strict';
    var self = this;
    var textResources;
    var textResourcesLoaded = false;
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    self.handleError = function (data, response) {
        console.log(data);
        if (data) {
            if (response === 404) {
                $rootScope.errors = $rootScope.errors || [];
                var errorMessage = "404: Url not found. ";
                throw { Message: errorMessage, ExceptionMessage: "" };
            } else {
                throw data;
            }
        }

    };

    self.pushOkMessage = function (message) {
        $rootScope.messages = $rootScope.messages || [];
        $rootScope.messages.push(message);
        setTimeout(function () {
            var messageIndex = $rootScope.messages.indexOf(message);
            $rootScope.messages.splice(messageIndex, 1);
            $rootScope.$apply();
        }, 3000);
    };

}]);