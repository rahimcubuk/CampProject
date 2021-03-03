using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfOrderDal:EfEntityRepositoryBase<Order,NorthwindContext>,IOrderDal
    {
    }
}
