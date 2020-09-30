$(document).ready(function () {


    var displayModifyDeliveryTab = function () {
        $("#changePassword").css("display", "none");
        $("#addDelivery").css("display", "none");
        $("#deleteAccount").css("display", "none");

        $("#modifyDelivery").css("display", "block");
        $("#modifyDeliveryBtn").css("background-color", "red");
    };


    var modifyDelivery = function () {

    };



    // main
    $("#modifyDeliveryBtn").on("click", displayModifyDeliveryTab);
    $("")
});