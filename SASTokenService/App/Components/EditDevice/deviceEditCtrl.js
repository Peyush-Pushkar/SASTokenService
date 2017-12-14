myapp.controller("deviceEditCtrl", function ($scope, $http, $routeParams) {
    // Get Deatils Of Specific Device

   
    $http.get('/api/DeviceRegistry/', { params: { ID: $routeParams.id, ProviderID: 'DAL.CACHE' } })
        .success(function (data) {
            $scope.selectedDevice = data[0];
        })
.error(function () {
    $scope.error = "An Error has occured while getting the Device";
});

    // Delete Functionality
    $scope.isDeleteSucess = false;
    $scope.deleteDevice = function deleteDevice(DeviceId) {

        $scope.devicetoBeDeleted = {
            ProviderID: 'DAL.CACHE',
            ID : DeviceId
        };
        $http.put('/api/DeviceRegistry/Delete', $scope.devicetoBeDeleted).
            success(function (data) {
                $scope.isDeleteSucess = true;
            })
      .error(function (error) {
          $scope.error = "An Error has occured while deleting the device";
      });
    }
    // save Or update Functionlaity

    $scope.isDeviceSavedSucess = false;

    $scope.saveDevice = function saveDevice() {
        $scope.modifiedDevice = {
            ID: $scope.selectedDevice.DeviceID,
            Name: $scope.selectedDevice.Name,
            IP: $scope.selectedDevice.IP,
            Uri: $scope.selectedDevice.Uri,
            DeviceKey: $scope.selectedDevice.DeviceKey,
            ProviderID: $scope.selectedDevice.ProviderID,
            Owner: $scope.selectedDevice.Owner,
            GeoPosition: $scope.selectedDevice.GeoPosition,
            KeyValidity: $scope.selectedDevice.KeyValidity,
            ProviderID: 'DAL.CACHE'

        }
        $http.put("api/DeviceRegistry/", $scope.modifiedDevice)
            .success(function (data) {
                $scope.isDeviceSavedSucess = true;
                $scope.selectedDevice = data;
            })
      .error(function () {
          $scope.error = "An Error has occured while Updating the device";
      });

    }


});