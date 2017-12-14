'use strict';
angular.module('sasTokenApp', ['ngRoute', 'AdalAngular', 'ui.bootstrap' ])
.config(['$routeProvider', '$httpProvider', 'adalAuthenticationServiceProvider',
    function ($routeProvider, $httpProvider, adalProvider) {

    $routeProvider.when("/Home", {
        controller: "homeCtrl",
        templateUrl: "/App/Components/Home/Home.html",
        requireADLogin: false
    }).when("/UserData", {
        controller: "userDataCtrl",
        templateUrl: "/App/Components/UserData/UserData.html",
        requireADLogin: false
    }).when("/SASTokenHome", {
        controller: "sasTokenCtrl",
        templateUrl: "/App/Components/SasTokenCreator/SasToken.html",
        requireADLogin: false
    }).when("/Devices", { //added for sasDevice List All Devices
        controller: "deviceListCtrl",
        templateUrl: "/App/Components/ListDevices/DeviceList.html",
        requireADLogin: false
    }).when("/AddDevice", { //added for sasDevice Add
        controller: "deviceAddCtrl",
        templateUrl: "/App/Components/AddDevice/DeviceAdd.html",
        requireADLogin: false
    }).when("/EditDevice/DeviceId/:id", { //added for sasDevice Edit
        controller: "deviceEditCtrl",
        templateUrl: "/App/Components/EditDevice/DeviceEdit.html",
        requireADLogin: false
    })
      .otherwise({ redirectTo: "/Home" });

    adalProvider.init(
        {
            tenant: '84469e82-b5b5-4539-b80f-95df7ebbbff8',
            clientId: 'eda85b2f-303c-4d45-8e61-2174385a9f8d',
           // clientId: 'http://ddobricdaenet/sastokenservice',
           // resource: 'http://ddobricdaenet/sastokenservice',
            extraQueryParameter: 'nux=1'
        },
        $httpProvider
        );
}]);
