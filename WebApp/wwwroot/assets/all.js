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
                .state('new', {
                    url: "/new",
                    templateUrl: "/html/brewGuide/new/index.html",
                    controller: "newController"
                })
                .state('resume', {
                    url: "/resume",
                    templateUrl: "/html/brewGuide/resume/index.html",
                    controller: "resumeController"
                })
                .state('displayBrew', {
                    url: "/brew/:brewId",
                    templateUrl: "/html/brewGuide/display/index.html",
                    controller: "displayController"
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
//var serviceBase = 'http://brewmaticwebapp.azurewebsites.net/api/';
var serviceBase = 'http://localhost:5000/api/';
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
                if (newTemp1 !== oldTemp1 || newTemp2 !== oldTemp2) {
                    if (newTemp1 && newTemp1 !== "" && newTemp2 && newTemp2 !== "") {
                        console.log(angular.isNumber(parseFloat(newTemp1)));
                        if (angular.isNumber(parseFloat(newTemp1)) && angular.isNumber(parseFloat(newTemp2))) {
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
angular.module('BrewMatic')
  .filter('splitInLines', ['$sce', function ($sce) {
    return function (input) {
      input = input || '';
      var arr = input.split(".");
      var out = "<ul>";
      for (var i = 0; i < arr.length; i++) {
        if (arr[i].trim() !== "") {
          out = out + "<li>" + arr[i] + "</li>";
        }
      }
      return $sce.trustAsHtml(out + "<ul>");
    };
  }]);

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
    };

    self.getDataCapture = function (brewStepId) {
        return $q(function (resolve, reject) {
            $http.get(serviceBase + 'datacapture/'+brewStepId).success(function (result) {
                resolve(result);
            }).error(pageHelperService.handleError);
        });
    };

     self.getDefinedDataCaptureValues = function (brewId) {
        return $q(function (resolve, reject) {
            $http.get(serviceBase + 'datacapture/getDefinedValues/'+brewId).success(function (result) {
                resolve(result);
            }).error(pageHelperService.handleError);
        });
    };

    self.saveDataCapture = function (dataCaptureValues) {
        return $q(function (resolve, reject) {
            $http.post(serviceBase + 'datacapture', dataCaptureValues).success(function (result) {
                resolve(result);
                pageHelperService.pushOkMessage("Successfully saved data capture values");
            }).error(pageHelperService.handleError);
        });
    };

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

angular.module('BrewMatic').controller('displayController', ['$scope', '$window', 'BrewGuideService', '$stateParams', '$state', '$timeout', function ($scope, $window, service, $stateParams, $state, $timeout) {
    'use strict';

    var self = this;

    var changeTimeout;
    $scope.saving = null;
    var brewId = $stateParams.brewId;

    self.saveIfNotChangedAfterNSeconds = function (seconds) {
        if (changeTimeout) $timeout.cancel(changeTimeout);
        changeTimeout = $timeout(function () {
            service.saveDataCapture($scope.dataCaptureValues).then(function (result) {
                $scope.saving = null;
                console.log("Changes was saved");
                self.getDefinedDataCaptureValues();
            });
        }, seconds * 1000); // delay n seconds
    };

    self.getDefinedDataCaptureValues = function () {
        service.getDefinedDataCaptureValues(brewId).then(function (result) {
            $scope.definedDataCaptureValues = result;
        });
    };

    self.loadData = function () {
        service.getBrew(brewId).then(function (result) {
            $scope.brew = result;
            $scope.startTime = result.currentStep.startTime;
            self.updateStepTime();
            if (result.currentStep.completeTime) {
                $scope.completeTime = result.currentStep.completeTime;
                self.updateCountdown();
            }
            service.getDataCapture(result.currentStep.id).then(function (dataCaptureValues) {
                $scope.dataCaptureValues = dataCaptureValues;
                $scope.$watch('dataCaptureValues', function (newValue, oldValue, scope) {
                    if (newValue !== oldValue) {
                        $scope.saving = true;
                        self.saveIfNotChangedAfterNSeconds(1);
                    }
                }, true);
            });
            self.getDefinedDataCaptureValues();
        });
        service.getBrewHistory(brewId).then(function (result) {
            $scope.brewHistory = result;
        });
    };

    self.updateStepTime = function () {
        $timeout(function () {
            var startTime = moment($scope.startTime);
            $scope.stepTime = moment.utc(moment().diff(startTime)).format("HH:mm:ss");
            self.updateStepTime();
        }, 1000);
    };

    self.updateCountdown = function () {
        $timeout(function () {
            var endTime = moment($scope.completeTime);
            $scope.countDown = moment.utc(endTime.diff(moment())).format("HH:mm:ss");
            self.updateCountdown();
        }, 1000);
    };

    self.loadData();

    $scope.goToNextStep = function () {
        service.goToNextStep(brewId).then(function (result) {
            $state.go("displayBrew", { brewId: brewId }, { reload: true });
        });
    };

    $scope.goBackOneStep = function () {
        if (confirm("Are you sure you want to abort the current step and return to the previous?")) {
            service.goBackOneStep(brewId).then(function (result) {
                $state.go("displayBrew", { brewId: brewId }, { reload: true });
            });
        }

    };

}]);

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