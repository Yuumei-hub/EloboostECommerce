using EloboostCommerce.Models.Classes;

namespace EloboostCommerce.Services
{
    public class OrderService
    {
        private readonly Context _context;
        public OrderService(Context context)
        {
            _context = context; 
        }

        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            var order = _context.Orders.Find(orderId);
            if(order!= null)
            {
                order.Status = newStatus;
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Order not found.");
            }
        }
    }
}
