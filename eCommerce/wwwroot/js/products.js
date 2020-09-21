$(document).ready(function () {

    // for delete button
    var deleteFunction = function (event) {
        var btn = event.target; // get clicked button
        var productId = $(btn).data('product-id'); // current id
        var divToDelete = $(btn).closest(".productButtons");

        // block the button
        $(btn).removeAttr('enabled');
        $(btn).attr('disabled', 'disabled');

        var deleteUrl = $('#deleteActionUrl').data('delete-action-url');

        $.ajax({
            type: 'POST',
            url: deleteUrl,
            data: {
                productId: productId
            },
            success: function (response) {
                if (response && response.flag) {
                    $(divToDelete).remove();
                }
                else {
                    alert("Error");
                }

                // unblock the button
                $(event.target).removeAttr('disabled');
                $(event.target).attr('enabled', 'enabled');
            },
            error: function (error) {
            }
        });
    };


    var displayCurrentProductForUpdate = function (modelToDisplay) {
        
    };


    // for update button
    var updateFunction = function (event) {
        var btn = event.target;
        var productId = $(btn).data('product-id');

        $(btn).removeAttr('enabled');
        $(btn).attr('disabled', 'disabled');

        var updateUrl = $('#updateActionUrl').data('update-action-url');

        $.ajax({
            type: 'GET',
            url: updateUrl,
            data: {
                productId: productId
            },
            success: function (response) {
                if (response && response.flag) {
                    //
                    console.log(response.modelToDisplay.productName);
                    displayCurrentProductForUpdate(response.modelToDisplay);
                }
                else {
                    alert("Error");
                }

                $(event.target).removeAttr('disabled');
                $(event.target).attr('enabled', 'enabled');
            },
            error: function (error) {
            }
        });
    };

    //main
    $('.deleteBtn').on('click', deleteFunction);
    $('.updateBtn').on('click', updateFunction);
});