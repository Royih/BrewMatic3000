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