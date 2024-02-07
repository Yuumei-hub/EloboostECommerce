// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification

function redirectToGameIndex(){
    window.location.href = '/Game/Index';
}

document.addEventListener('DOMContentLoaded', function () {
    //const variables
    const gameimgurl = document.getElementById('productImage').src;
    const gamename = document.getElementById('gamename').textContent;
    const productPrice = document.getElementById('productPrice');
    const select1 = document.getElementById('select1');
    const select2 = document.getElementById('select2');
    const buynowBtn = document.getElementById('buyNowButton');
    var quantity = 0;
    var priceDifference = 0;
    //Events
    select1.addEventListener('change', getPrice);
    select2.addEventListener('change', getPrice);
    buynowBtn.addEventListener('click', handleBuyNow);

    //functions-------------------------------------------
    function getPrice() {
        const tier1Price = parseFloat(select1.options[select1.selectedIndex].getAttribute('data-price'));
        const tier2Price = parseFloat(select2.options[select2.selectedIndex].getAttribute('data-price'));
        priceDifference = tier2Price - tier1Price;
        // Update product image and price display
        productPrice.textContent = `$${priceDifference.toFixed(2)}`;
    }
    function getSelectedOptionText(id) {
        var select = document.getElementById(id);
        var selectedOption = select.options[select.selectedIndex];
        return selectedOption.textContent;
    }
    function handleBuyNow() {
        var fromSkillRating = getSelectedOptionText("select1");
        var toSkillRating = getSelectedOptionText("select2");
        var price = priceDifference;
        quantity++;
        //game-specific properties here

        //AJAX request to controller action
        sendBuyNowRequest(fromSkillRating,toSkillRating,price,gamename,gameimgurl, quantity);
    }
    function sendBuyNowRequest(fromSkillRating, toSkillRating, price,gamename,gameimgurl,quantity) {
        $.ajax({
            url: "/Cart/AddCartItem",
            type: "POST",
            data: {
                quantity: quantity,
                fromSkillRating: fromSkillRating,
                toSkillRating: toSkillRating,
                price: price,
                gamename: gamename,
                gameimgurl: gameimgurl,
            },
            success: function (result) {
                window.location.href = "/Cart/Index";
            },
            error: function (error) {
                console.error(error);
            }
        });

    }
});
