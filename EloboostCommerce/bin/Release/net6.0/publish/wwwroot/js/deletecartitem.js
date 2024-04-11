document.querySelectorAll('.content-button').forEach(button => {
    button.addEventListener('click', function () {
        const cartItemId = this.getAttribute('data-cartItemId');

        // Send an AJAX request to delete the cart item
        fetch(`/Checkout/DeleteCartItem?cartItemId=${cartItemId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                // Include any necessary headers, such as anti-forgery tokens
            },
            // Optionally, handle response or errors
        }).then(response => {
            // Handle response or errors
        }).catch(error => {
            console.error('Error:', error);
        });
    });
});
