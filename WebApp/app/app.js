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


