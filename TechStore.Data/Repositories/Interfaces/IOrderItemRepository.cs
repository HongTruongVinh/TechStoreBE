using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;

namespace TechStore.Data.Repositories.Interfaces
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
    }
}
