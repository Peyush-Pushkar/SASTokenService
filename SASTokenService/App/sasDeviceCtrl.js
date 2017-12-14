'use strict';
angular.module('sasTokenApp')
.controller("sasDeviceCtrl", function ($scope, $timeout, $rootScope, $window, $http) {
    $scope.DeviceDetail = {};
    $scope.Devices = {};
    $scope.data = {};
    $scope.dateDifference = false;
    var currentDate = new Date()

    $scope.stringToFind = "";

    $scope.showDevicesDetail = false;
    $scope.showDeviceAdd = true;
    $scope.addEditDevices = false;
    $scope.showDevicesList = true;
    $scope.showItem = true;

    //These variable will be used for Insert/Edit/Delete Device details.
    $scope.deviceId = "";
    $scope.deviceKey = "";
    $scope.deviceNames = "";
    $scope.owners = "";
    $scope.keyValiditys = "";
    $scope.ips = "";
    $scope.locations = "";

    //Search GET(stringToFind)
    $scope.searchDeviceDetails = function () {
        selectDeviceDetails($scope.stringToFind);
    }

    function selectDeviceDetails(stringToFind) {
        console.log("Inside select" + $scope.stringToFind);
        $http.get('/api/DeviceRegistry/', { params: { stringToFind: stringToFind }           
        }).success(function (data) {
            $scope.Devices = data;
            $scope.showDeviceAdd = true;
            $scope.addEditDevices = false;
            $scope.showDevicesDetail = false;
            $scope.showDevicesList = true;
            $scope.showItem = true;

            if ($scope.Devices.length === 0) {
                alert("Searched string " + $scope.stringToFind + " not found. Please try another device property!");
            }
        })
  .error(function () {
      $scope.Devices = [];
      $scope.error = "An Error has occured while loading posts!";
  });
    }

    //Calling deviceDetail function passing a deviceId
    $scope.deviceDetail = function () {
        deviceDetail($scope.deviceId);
    }

    //Calling HTTPGET for GetDeviceById method in DeviceRegistryController
    //Fetches the detail of the device matching the deviceId
    $scope.deviceDetail = function deviceDetail(DeviceId) {
        $http.get('/api/DeviceRegistry/GetDeviceById/', { params: { deviceId: DeviceId } }).success(function (data) {

            $scope.DeviceDetail = data;

            var keyValidityDate = new Date($scope.DeviceDetail.KeyValidity);
            if (currentDate > keyValidityDate) {
                $scope.dateDifference = true;
                $scope.DeviceDetail.KeyValidity = keyValidityDate.setMinutes(keyValidityDate.getMinutes() + 1);               
            }
            else {
                $scope.dateDifference = false;
            }

            $scope.showDeviceAdd = true;
            $scope.showDevicesDetail = true;
            $scope.showDevicesList = false;
            $scope.showItem = true;
            $scope.addEditDevices = false;

        })
  .error(function () {
      $scope.error = "An Error has occured while loading posts!";
  });
    }


    //Loads the fields to input the data for editing a device
    $scope.deviceEdit = function deviceEdit(DeviceId, DeviceName, DeviceKey, KeyValidity, Ip, Owner, Location,dateDifference) {

        cleardetails();

        $scope.deviceId = DeviceId;
        $scope.deviceNames = DeviceName
        $scope.deviceKey = DeviceKey;
        $scope.keyValiditys = KeyValidity;
        $scope.owners = Owner;
        $scope.ips = Ip;
        $scope.locations = Location;
        $scope.dateDifferences = dateDifference;

        $scope.showDeviceAdd = true;
        $scope.addEditDevices = true;
        $scope.devicesList = true;
        $scope.showItem = true;

    }

    $scope.showDeviceDetails = function () {
        cleardetails();
        $scope.showDeviceAdd = true;
        $scope.addEditDevices = true;
        $scope.showDevicesDetail = false;
        $scope.devicesList = false;
        $scope.showItem = true;


    }

    //To clear all the field values after insert and edit.
    function cleardetails() {
        $scope.deviceId = 0;
        $scope.deviceKey = "";
        $scope.deviceNames = "";
        $scope.owners = "";
        $scope.ips = "";
        $scope.locations = "";
    }


    //Form Validation
    $scope.Message = "";
    $scope.IsFormSubmitted = false;

    $scope.IsFormValid = false;


    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;

    });

    //Executing saveDetails function to implement saving or updating of device
    $scope.saveDetails = function () {

        $scope.IsFormSubmitted = true;
        if ($scope.IsFormValid) {
            //if the DeviceId=0 it means new Device insert 
            if ($scope.deviceId === 0) {

                //Calling HTTPPOST for insertDevice method in DeviceregistryController
                //Inserts the new device related information provided by the user and auto assign a deviceId
                //Fetches and displays all the devices after succesfull completion
                $http({
                    url: "api/DeviceRegistry/RegisterNewDevice/",
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    params: { DeviceName: $scope.deviceNames, Owner: $scope.owners, Ip: $scope.ips, Location: $scope.locations }
                }).success(function (id) {

                    $scope.deviceId = id;
                    alert($scope.DevicesInserted);
                    cleardetails();
                    selectDeviceDetails('', '');
                })
            .error(function (e1,e2,e3) {
             $scope.error = "An Error has occured while loading posts!";
         });
            }
            else {
                //Calling updateDevice method in DeviceregistryController
                //Updates the device related information provided by the user
                //Fetches and displays all the devices after succesfull completion
                console.log("inside else update");
                $http({
                    url: "api/DeviceRegistry/updateDevice/",
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    //params: { DeviceId: $scope.deviceId, DeviceName: $scope.deviceNames, Ip: $scope.ips, Owner: $scope.owners, Location: $scope.locations, KeyValidity: $scope.keyValiditys, DateDifference: $scope.dateDifferences }
                    params: {
                        DeviceId: $scope.deviceId, DeviceName: $scope.deviceNames, DeviceKey: $scope.deviceKey, KeyValidity: $scope.keyValiditys,
                        Ip: $scope.ips, Owner: $scope.owners, Location: $scope.locations, DateDifference: $scope.dateDifference
                    }
                }).success(function (data) {
                /*$http.put('/api/DeviceRegistry/updateDevice/',
                    {
                        params: { "DeviceId": $scope.deviceId, "DeviceName": $scope.deviceNames, "Ip": $scope.ips, "Owner": $scope.owners, "Location": $scope.locations, "KeyValidity": $scope.keyValiditys, "DateDifference": $scope.dateDifferences }
            }).success(function (data) {*/
                    $scope.DevicesUpdated = data;
                    alert($scope.DevicesUpdated);

                    cleardetails();
                    selectDeviceDetails('', '');

                })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
        });
            }
        }
        else {
            $scope.Message = "All the fields are required.";
        }
    }

    //Calling updateDevice method in DeviceregistryController
    //Updates the DeviceKey and KeyValidity if device has be expired
    //Fetches and displays the updated key and KeyValidity Timestamp
    $scope.updateKey = function updateKey(DeviceId, DeviceName, DeviceKey, KeyValidity, Ip, Owner, Location, dateDifference) {
        console.log("inside update key");
        $http({
            url: "api/DeviceRegistry/updateDevice/",
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            params: { DeviceId: DeviceId, DeviceName: DeviceName, DeviceKey: DeviceKey, KeyValidity: KeyValidity, Ip: Ip, Owner: Owner, Location: Location, DateDifference: dateDifference }
        }).success(function (data) {
            $scope.DevicesUpdated = data;
            alert($scope.DevicesUpdated);
            $scope.deviceDetail(DeviceId);

        })
       .error(function () {
           $scope.error = "An Error has occured while loading posts!";
       });
    }

    //Calling HTTPDELETE for deleteDevice method in DeviceRegistryController 
    //params: DeviceId
    //Fetches the remaining list of devices
    $scope.deviceDelete = function deviceDelete(DeviceId, DeviceName) {
        cleardetails();
        $scope.deviceId = DeviceId;
        var delConfirm = confirm("Are you sure you want to delete the Device " + DeviceName + " ?");
        if (delConfirm === true) {
            $http.delete('/api/DeviceRegistry/deleteDevice/', { params: { DeviceId: $scope.deviceId } }).success(function (data) {

                alert("Device Deleted Successfully!!");
                cleardetails();
                selectDeviceDetails('', '');
            })
      .error(function () {
          $scope.error = "An Error has occured while loading posts!";
      });

        }
    }

});