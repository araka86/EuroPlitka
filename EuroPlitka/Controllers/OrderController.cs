using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model.ViewModels;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EuroPlitka.Controllers
{

    public class OrderController : Controller
    {
        private readonly IOrderHeaderRepository _orderHRepo;
        private readonly IOrderDetailRepository _orderDRepo;

        [BindProperty]
        public OrderVM OrderVM { get; set; }    //для метода Details

        public OrderController(IOrderHeaderRepository orderHRepo, IOrderDetailRepository orderDRepo)
        {
            _orderHRepo = orderHRepo;
            _orderDRepo = orderDRepo;
        }





        public IActionResult Index(string searchName = null, string searchEmail = null, string searchPhone = null, string Status = null, string IsReset = null)
        {

            OrderListVm orderListVm = new OrderListVm()
            {
                OrderHeaderList = _orderHRepo.GetAll()
            };
            if (IsReset == null)
            {



                if (!string.IsNullOrEmpty(searchName))
                {
                    orderListVm.OrderHeaderList = orderListVm.OrderHeaderList.Where(u => u.FullName.ToLower().Contains(searchName.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchEmail))
                {
                    orderListVm.OrderHeaderList = orderListVm.OrderHeaderList.Where(u => u.Email.ToLower().Contains(searchEmail.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchPhone))
                {
                    orderListVm.OrderHeaderList = orderListVm.OrderHeaderList.Where(u => u.PhoneNumber.ToLower().Contains(searchPhone.ToLower()));
                }
              
            }



            return View(orderListVm);
        }


        public IActionResult Details(int id)
        {
            OrderVM orderVM = new OrderVM()
            {
                OrderHeader = _orderHRepo.FirstOrDefault(u => u.Id == id),
             
                OrderDetails = _orderDRepo.GetAll(o => o.OrderHeaderId == id, includeProperties: "Product")

            };
            return View(orderVM);
        }




    }
}
