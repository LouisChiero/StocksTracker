(function () {
    'use strict';
    var controllerId = 'register';

    angular.module('stocksTrackerApp').controller(controllerId, [
        'common',
        'commonConfig',
        'authenticationService',      
        'bootstrap.alertService',
        '$timeout',
        register]);

    function register(common, commonConfig, authenticationService, alertService, $timeout) {

      var alertType = "",
          vm = this;

      vm.registrationData = {
        userName: "",
        password: "",
        confirmPassword: ""
      };

      vm.message = "";

      vm.alertType = function () {
        return alertType;
      };    
   
      vm.submit = registerUser;

      vm.cancel = function (registrationForm) {       
        alertType = "";
        registrationForm.$setPristine();
        vm.registrationData = { userName: "", password: "", confirmPassword: "" };
        vm.message = "";       
      };      

      activate();

      function activate() {
        common.activateController([], controllerId)
            .then(function () { console.log(controllerId + " loaded"); });
      }

      function registerUser() {
      
          var success = false;
          common.eventRaiser(commonConfig.config.userRegistrationStartedEvent);      

          authenticationService.register(vm.registrationData)
              .then(function (response) {                
                // success
                success = true;
                alertType = "success";                
                vm.message = "User " + vm.registrationData.userName + " has been registered successfully.  Redirecting to the Login page...";
              },
              // error
             function (err) {
                  alertType = "danger";
                 
                 var errors = [];
                 for (var key in err.data.modelState) {                   
                   for (var i = 0; i < err.data.modelState[key].length; i++) {
                     errors.push(err.data.modelState[key][i]);
                   }
                 };

                 vm.message = "User registration failed due to: " + errors.join(' ');                 
             })
            .finally(function () {           
              
              if (success === true) {
                  common.eventRaiser(commonConfig.config.userRegistrationCompletedEvent);                                   
                  startTimer();                  
                } else {
                  common.eventRaiser(commonConfig.config.userRegistrationCompletedEvent, { success: false });
                }

          });
      }

      var startTimer = function () {
          var timer = $timeout(function () {
            $timeout.cancel(timer);
            common.eventRaiser(commonConfig.config.userRegistrationCompletedEvent, { success: true });
          }, 2000);
        }
    }
})();