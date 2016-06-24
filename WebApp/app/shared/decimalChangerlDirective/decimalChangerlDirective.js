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