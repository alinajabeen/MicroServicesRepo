using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        // Why we use constructure here?
        //Because when we have base class consisting the ctor injected with the parameter(ordercontext) so we need to inject it in the sub class
        public OrderRepository(OrderContext orderContext) : base(orderContext)
        {
               
        }
        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            var orderList = await _orderContext.Orders.Where(o => o.UserName == userName)
                 .ToListAsync();
            return orderList;
        }
    }
}
