'use strict';
angular.module('sasTokenApp')
.controller('sasTokenCtrl', ['$scope', '$http', 'adalAuthenticationService',
    function ($scope, $http, adalService) {

        $scope.adalService = adalService;
        $scope.deviceId = "4";
        $scope.deviceKey = "zgmq9745ZG";
        $scope.validate = false;
        

        $scope.init = function () {
            var a = 1;
        };

        $scope.getSasToken = function (input) {

            $scope.loadingMessage = "Loading...";
            $scope.token = "";
           // $scope.loading = true;

            var token = this.adalService.getCachedToken(this.adalService.config.loginResource);
       
            if (input === "TOPIC") {
                $http.post('/api/sastoken/create',
               {
                   'providerId': 'SB.SAS.01',
                   'tokenIdentifier': input,
                   'sbNamespace': 'iotlab2',
                   'path': 'sampletopic',
                   'entityType': 'Topic'
               },
               {
                   headers: { 'DeviceCredentials': $scope.deviceId + "," + $scope.deviceKey }
               }).
                 success(function (data, status, headers, config) {
                     $scope.error = "";
                     $scope.loadingMessage = "";
                     $scope.loading = false;
                     $scope.token = JSON.stringify(data);
                 }).
                 error(function (data, status, headers, config) {
                     $scope.error = data;
                     $scope.loadingMessage = "";
                     
                 });
                }
            else if (input === "IoTHub") {
                $http.post('/api/sastoken/create',
               { 
                   'providerId': 'IoTHub.SAS.01',
                   'tokenIdentifier': input,
                   'deviceId': $scope.deviceId,
                   'deviceName':'TestDevice1'
                
               },
               {
                   headers: { 'DeviceCredentials': $scope.deviceId + "," + $scope.deviceKey }
               }).
                 success(function (data, status, headers, config) {
                     $scope.error = "";
                     $scope.loadingMessage = "";
                     $scope.token = JSON.stringify(data);
                 }).
                 error(function (data, status, headers, config) {
                     $scope.error = data;
                     $scope.loadingMessage = "";
                 });
            }

            else if (input === "EVENTHUB") {
                $http.post('/api/sastoken/create',
               {
                   'providerId': 'SB.SAS.01',
                   'tokenIdentifier': input,
                   'sbNamespace': 'iotlab2',
                   'path': 'saseventhub',
                   'entityType': 'eventhub',
               },
               {
                   headers: { 'DeviceCredentials': $scope.deviceId + "," + $scope.deviceKey }
               }).
                 success(function (data, status, headers, config) {
                     $scope.error = "";
                     $scope.loadingMessage = "";
                     $scope.token = JSON.stringify(data);
                 }).
                 error(function (data, status, headers, config) {
                     $scope.error = data;
                     $scope.loadingMessage = "";
                 });
            }

            else if (input === "QUEUESB") {
                $http.post('/api/sastoken/create',
               {
                   'providerId': 'SB.SAS.01',
                   'tokenIdentifier': input,
                   'entityType': 'QueueSB',
                   'sbNamespace': 'iotlab2',
                   'path': 'samplequeue',
               },
               {
                   headers: { 'DeviceCredentials': $scope.deviceId + "," + $scope.deviceKey }
               }).
                 success(function (data, status, headers, config) {
                     $scope.error = "";
                     $scope.loadingMessage = "";
                     $scope.token = JSON.stringify(data);
                 }).
                 error(function (data, status, headers, config) {
                     $scope.error = data;
                     $scope.loadingMessage = "";
                 });
            }

            else if (input === "QUEUE") {
                $http.post('/api/sastoken/create',
               {
                   'providerId': 'STORAGE.SAS.01',
                   'tokenIdentifier': input,
                   'accountName': 'iotlab2',
                   'name': 'TestQueueSASProjectV01',
               },
               {
                   headers: { 'DeviceCredentials': $scope.deviceId + "," + $scope.deviceKey }
               }).
                 success(function (data, status, headers, config) {
                     $scope.error = "";
                     $scope.loadingMessage = "";
                     $scope.token = JSON.stringify(data);
                 }).
                 error(function (data, status, headers, config) {
                     $scope.error = data;
                     $scope.loadingMessage = "";
                 });
            }


            else if (input === "TABLE") {
                $http.post('/api/sastoken/create',
               {
                   'providerId': 'STORAGE.SAS.01',
                   'tokenIdentifier': input,
                   'accountName': 'beststudents2',
                   'name': 'TGuestBook',
               },
               {
                   headers: { 'DeviceCredentials': $scope.deviceId + "," + $scope.deviceKey }
               }).
                 success(function (data, status, headers, config) {
                     $scope.error = "";
                     $scope.loadingMessage = "";
                     $scope.token = JSON.stringify(data);
                 }).
                 error(function (data, status, headers, config) {
                     $scope.error = data;
                     $scope.loadingMessage = "";
                 });
            }


            else if (input === "BLOB") {
                $http.post('/api/sastoken/create',
               {
                   'providerId': 'STORAGE.SAS.01',
                   'tokenIdentifier': input,
                   'accountName': 'sascontainer',
                   'name': 'villa.png',
               },
               {
                   headers: { 'DeviceCredentials': $scope.deviceId + "," + $scope.deviceKey }
               }).
                 success(function (data, status, headers, config) {
                     $scope.error = "";
                     $scope.loadingMessage = "";
                     $scope.token = JSON.stringify(data);
                 }).
                 error(function (data, status, headers, config) {
                     $scope.error = data;
                     $scope.loadingMessage = "";
                 });
            } 
        };


        $scope.getOauthToken = function () {

        }
    }]);
