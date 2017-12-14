'use strict';
var myapp = angular.module('sasTokenApp');
myapp.controller("deviceListCtrl", function ($scope, $http) {

    $scope.searchedDevice = "";
    $scope.SearchEnabled = false;
    $scope.isInsertSucess = false;
    $scope.itemsPerPage = 10;
    $scope.loading = true;
    // to Load List of Data 
    // Use ProviderID: 'DAL.USEDB' to call actual DB
    $http.get('/api/DeviceRegistry', { params: { ProviderID: 'DAL.CACHE' } }).success(function (data) {
            $scope.deviceList = data;
            $scope.currentPage = 1;
            
            $scope.loading = false;
        })
        .error(function () {
            $scope.loading = false;
            $scope.error = "An Error has occured while Fetching Data";
        });



    

    $scope.searchDeviceDetails = function searchDeviceDetails(searchedDevice) {
        $http.get('/api/DeviceRegistry/', { params: { Name: searchedDevice, ProviderID: 'DAL.CACHE' } }).then(function (data) {
            // success
            $scope.deviceList = data.data;
        }, function (errData) {
            // error
            $scope.error = "An Error has occured";
        });
    };

})
;