$(document).ready(function () {

    var removeProductFromCart = function (event) {
        var btn = event.target;
        var productId = $(btn).data('product-id'); 
        var divToDelete = $(btn).closest(".isGenerated");

        var removeProductCartUrl = $('#removeProductCartUrl').data('remove-action-url')

        $(btn).removeAttr('enabled');
        $(btn).attr('disabled', 'disabled');

        $.ajax({
            type: 'POST',
            url: removeProductCartUrl,
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

                $(event.target).removeAttr('disabled');
                $(event.target).attr('enabled', 'enabled');
            },
            error: function (error) {
            }
        });
    };

    var displayCartModal = function (cartModelList) {

        var isGenerated = document.getElementsByClassName('isGenerated').length;
        if (!isGenerated) {
            $.each(cartModelList, function (index, value) {

                var htmlCode = $([
                    "<div class='card isGenerated'>",
                    "   <div class='card-body'>",
                    "       <img class='card-img'/>",
                    "           <div class='img-thumbnail'",
                    "               <p class='card-text'>Name: " + value.productName + "</p>",
                    "               <p class='card-text'>Price: " + value.productPrice + "</p>",
                    "           </div>",
                    "   </div>",
                    "   <button type='button' data-product-id='" + value.productId + "' class='removeProductBtn btn btn-primary'>Remove Product</button>",
                    "</div>"
                ].join("\n"));

                $(".insertModalBody").append(htmlCode);
            });
        }
    };

    var openCartModal = function (event) {
        var btn = event.target;

        $(btn).removeAttr('enabled');
        $(btn).attr('disabled', 'disabled');

        var cartUrl = $('#cartUrl').data('cart-action-url');

        $.ajax({
            type: 'GET',
            url: cartUrl,
            data: {
            },
            success: function (response) {
                if (response && response.flag) {
                    displayCartModal(response.cartModelList);
                    $('.removeProductBtn').on('click', removeProductFromCart);
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
    $('.cartBtn').on('click', openCartModal);
});
