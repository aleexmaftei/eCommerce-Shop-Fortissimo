$(document).ready(function () {

    // for delete button
    var deleteFunction = function (event) {
        var btn = event.target; 
        var productId = $(btn).data('product-id'); 
        var divToDelete = $(btn).closest(".productButtons");

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

    var globalModelToDisplay = null;
    var displayCurrentProductForUpdate = function (modelToDisplay) {
        var productName = modelToDisplay.productName;
        var quantity = modelToDisplay.quantity;
        var productPropertiesWithValues = modelToDisplay.productPropertiesWithValues;

        document.getElementById('productNameInput').value = productName;
        document.getElementById('quantityInput').value = quantity;

        var isGenerated = document.getElementsByClassName('isGenerated').length;
        if (!isGenerated) {
            $.each(productPropertiesWithValues, function (index, value) {

                var htmlCode = $([
                    "<div class='form-group isGenerated'>",
                    "   <label class='control-label'>" + value.propertyName + "</label>",
                    "   <input class='form-control' id='" + value.propertyName + "' value='" + value.detailValue + "'/>",
                    "   <span class='text-danger'></span>",
                    "</div>"
                ].join("\n"));

                $(".insertAfterThis").after(htmlCode);
            });
        }

        globalModelToDisplay = modelToDisplay;
    };

    // close modal 
    function closeModal() {
        $('#modalBox').modal('hide');
    };

    // for update button from modal
    var updateDatabase = function (event) {
        if (globalModelToDisplay == null) {
            alert("Select a product to update first!")
            return;
        }

        var btn = event.target;
        $(btn).removeAttr('enabled');
        $(btn).attr('disabled', 'disabled');

        var productName = document.getElementById('productNameInput').value;
        var quantity = parseInt(document.getElementById('quantityInput').value, 10);

        globalModelToDisplay.productName = productName;
        globalModelToDisplay.quantity = quantity;

        $.each(globalModelToDisplay.productPropertiesWithValues, function (index, value) {
            var currentPropertyName = value.propertyName;
            var currentProductDetail = document.getElementById(currentPropertyName).value;

            globalModelToDisplay.productPropertiesWithValues[index].detailValue = currentProductDetail;
        });
        
        var updateUrlPost = $('#updateActionUrl').data('update-action-url');
        $.ajax({
            type: 'POST',
            url: updateUrlPost,
            data: {
                model: globalModelToDisplay
            },
            success: function (response) {
                if (response && response.flag) {
                    closeModal();
                    location.reload(true);
                }
                else {
                    alert("Error");
                }

                $(btn).removeAttr('disabled');
                $(btn).attr('enabled', 'enabled');
            },
            error: function (error) {
            }
        });
    };

    // for update button
    var updateFunction = function (event) {

        setTimeout(function () {
            $("#modalBox").modal("show");
        }, 200);
        
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
                    displayCurrentProductForUpdate(response.modelToDisplay);
                }
                else {
                    alert("Error");
                }

                $(btn).removeAttr('disabled');
                $(btn).attr('enabled', 'enabled');
            },
            error: function (error) {
            }
        });
    };

    //main
    $('.deleteBtn').on('click', deleteFunction);
    $('.updateBtn').on('click', updateFunction);
    $('.updateBtnModal').on('click', updateDatabase);
    $('.closeBtnModal').on('click', closeModal);
});