/// <reference path="app.js" />
'use strict';
var myapp = angular.module('todoApp');
myapp.controller("deviceMaintainanceCtrl",function ($scope, $timeout, $rootScope, $window, $http) {
      $http.get('/api/ListAllDevices')
        .success(function (data) {
            $scope.deviceList = data;
        })
            .error(function () {
                $scope.Devices = [];
                $scope.error = "An Error has occured while loading posts!";
            })
    

});