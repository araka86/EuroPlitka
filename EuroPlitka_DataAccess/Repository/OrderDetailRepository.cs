using EuroPlitka_DataAccess.Data;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;

namespace EuroPlitka_DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {

        private readonly EuroPlitkaDbContext _db;

        public OrderDetailRepository(EuroPlitkaDbContext db) : base(db)
        {
            _db = db;
        }

        //public void Update(OrderDetail obj)
        //{
        //    _db.OrderDetail.Update(obj);
        //}

    }
}
