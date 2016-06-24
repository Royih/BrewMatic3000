angular.module('BrewMatic', ['ui.router', 'angular-loading-bar'])
    .config([
        '$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
            $urlRouterProvider.otherwise("/home");
            $stateProvider
                .state('home', {
                    url: "/home",
                    templateUrl: "/html/home/index.html",
                    controller: "homeController"
                })
                .state('tempLog', {
                    url: "/tempLog",
                    templateUrl: "/html/tempLog/index.html",
                    controller: "tempLogController"
                })
                .state('about', {
                    url: "/about",
                    templateUrl: "/html/about/index.html",
                    controller: "aboutController"
                });
        }
    ])
    .config([
        '$httpProvider', function ($httpProvider) {

            //This is to prevent caching of $http results. Seems to be required for Edge / IE. 
            //initialize get if not there
            if (!$httpProvider.defaults.headers.get) {
                $httpProvider.defaults.headers.get = {};
            }

            // Answer edited to include suggestions from comments
            // because previous version of code introduced browser-related errors

            //disable IE ajax request caching
            $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
            // extra
            $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
            $httpProvider.defaults.headers.Pragma = 'no-cache';
        }
    ]);

//var serviceBase = 'http://localhost:26264/';
var serviceBase = 'http://brewmaticwebapp.azurewebsites.net/api/';
//var serviceBase = 'http://localhost:5000/api/';
angular.module('BrewMatic').constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'ngAuthApp'
});

/*angular.module('BrewMatic').config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
}]);*/

/*angular.module('BrewMatic').run(['authService', function (authService) {
    authService.fillAuthData();
}]);*/




angular.module('BrewMatic').controller('aboutController', ['$scope', '$window', function ($scope, $window) {
    'use strict';

    var self = this;

    self.loadData = function () {
        console.log("hello world. about");
    };

    self.loadData();

}]);

angular.module('BrewMatic').controller('homeController', ['$scope', '$window', '$timeout', 'homeControllerService', function ($scope, $window, $timeout, service) {
    'use strict';

    var self = this;
    var changeTimeout;

    self.loadLastLog = function () {
        service.getLastLog().then(function (result) {
            $scope.lastLog = result;
        });
    };
    
    self.updateIfNotChangedAfterNSeconds = function (newValue1, newValue2, seconds) {
        if (changeTimeout) $timeout.cancel(changeTimeout);
        changeTimeout = $timeout(function () {
            service.saveTargetTemperature(newValue1, newValue2).then(function (result) {
                console.log("Changes was saved");
            });
        }, seconds * 1000); // delay n seconds
    };
    

    self.loadData = function () {
        self.loadLastLog();
        service.getTargetTemperature().then(function (result) {
            $scope.targetTemperature = result;

            $scope.$watchGroup(['targetTemperature.target1', 'targetTemperature.target2'], function (newValues, oldValues, scope) {
                var newTemp1 = newValues[0];
                var newTemp2 = newValues[1];
                var oldTemp1 = oldValues[0];
                var oldTemp2 = oldValues[1];

                console.log({ newTemp1: newTemp1, oldTemp1: oldTemp1, newTemp2: newTemp2, oldTemp2: oldTemp2 })
                if (newTemp1 !== oldTemp1 || newTemp2 !== oldTemp2) {
                    console.log("Changes found");
                    if (newTemp1 && newTemp1 !== "" && newTemp2 && newTemp2 !== "") {
                        console.log("Input found");
                        console.log(angular.isNumber(parseFloat(newTemp1)));
                        if (angular.isNumber(parseFloat(newTemp1)) && angular.isNumber(parseFloat(newTemp2))) {
                            console.log("Numeric input found");
                            self.updateIfNotChangedAfterNSeconds(newTemp1, newTemp2, 3);
                        }
                    }
                }
            });
        });
    };

    self.loadData();

    var poll = function () {
        $timeout(function () {
            self.loadLastLog();
            poll();
        }, 1000);
    };
    poll();

}]);

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
angular.module("BrewMatic").directive('decimalChanger', function () {
  return {
    restrict: 'A',
    scope: {
      ngModel: '=',
      min: '@',
      max: '@'
    },
    link: function (scope, element, attrs) {
      Number.prototype.round = function (p) {
        p = p || 10;
        return parseFloat(this.toFixed(p));
      };

      scope.changeTarget = function (value) {
        var newVal = scope.ngModel.round(1) + value;
        if (newVal >= scope.min && newVal <= scope.max) {
          scope.ngModel += value;
        }
      };
    },
    templateUrl: 'html/shared/decimalChangerlDirective/index.html'
  };
})