using System.Linq;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;

namespace E_Shop_Engine.Services.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(IAppDbContext context) : base(context)
        {
            _dbSet = _context.Orders;
        }

        public Order GetByOrderNumber(string orderNumber)
        {
            return _dbSet.Where(x => x.OrderNumber == orderNumber).FirstOrDefault();
        }
    }
}
