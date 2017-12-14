'use strict';
var myapp = angular.module('sasTokenApp');
myapp.controller("deviceAddCtrl", function ($scope, $http) {
    // Use ProviderID: 'DAL.USEDB' to call actual DB
    $scope.device = { ProviderID: 'DAL.CACHE' };

    $scope.Adddetail = function () {

        $http.post('/api/DeviceRegistry/Register', $scope.device).then(function (data) {
            // success
            $scope.device = data;
            $scope.isInsertSucess = true;
        }, function (errData) {
            // error
            $scope.error = "An Error has occured while updating a new device";
        });
    };
});