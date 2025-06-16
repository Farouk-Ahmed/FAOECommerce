using CleanArchitecture.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.DataAccess.IRepository
{
    public interface IShoppingCartRepository:IRepository<ShoppingCartItem>
    {
     
    }
}
