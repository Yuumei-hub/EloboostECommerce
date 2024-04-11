const payments = Square.payments(appId, locationId);
const card = await payments.card();
await card.attach('#card');
const form = document.querySelector('#card-payment');
form.addEventListener('submit', async (event) => {
   event.preventDefault();
   const result = await card.tokenize(); // the card nonce
});