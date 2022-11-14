using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using EuroPlitka_Model.ViewModels;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EuroPlitka.Controllers
{

    public class OrderController : Controller
    {
        private readonly IOrderHeaderRepository _orderHRepo;
        private readonly IOrderDetailRepository _orderDRepo;

        [BindProperty]
        public OrderVM OrderVM { get; set; }   

        public OrderController(IOrderHeaderRepository orderHRepo, IOrderDetailRepository orderDRepo)
        {
            _orderHRepo = orderHRepo;
            _orderDRepo = orderDRepo;
        }


     



        public async Task<IActionResult> Index(string searchName = null, string searchEmail = null, string searchPhone = null, string Status = null, string IsReset = null)
        {

            OrderListVm orderListVm = new OrderListVm()
            {
                OrderHeaderList = await _orderHRepo.GetAll()
            };
            if (IsReset == null)
            {



                if (!string.IsNullOrEmpty(searchName))
                {
                    orderListVm.OrderHeaderList = orderListVm.OrderHeaderList.Where(u => u.FullName.ToLower().Equals(searchName.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchEmail))
                {
                    orderListVm.OrderHeaderList = orderListVm.OrderHeaderList.Where(u => u.Email.ToLower().Contains(searchEmail.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchPhone))
                {
              
                    orderListVm.OrderHeaderList = orderListVm.OrderHeaderList.Where(u => u.PhoneNumber.ToLower().Equals(searchPhone.ToLower()));
                }
              
            }



            return View(orderListVm);
        }


        public async Task<IActionResult> Details(int id)
        {
            OrderVM orderVM = new OrderVM()
            {
                OrderHeader = await _orderHRepo.FirstOrDefault(u => u.Id == id),
             
                OrderDetails =await _orderDRepo.GetAll(o => o.OrderHeaderId == id, includeProperties: "Product")

            };
            return View(orderVM);
        }



        [HttpPost]
        public async Task<IActionResult> UpdateOrderDetails()
        {
            OrderHeader orderHeaderFromDb = await _orderHRepo.FirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);

            // т.к OrderVM - BindProperty
            orderHeaderFromDb.FullName = OrderVM.OrderHeader.FullName;
            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHeaderFromDb.City = OrderVM.OrderHeader.City;
            orderHeaderFromDb.State = OrderVM.OrderHeader.State;
            orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;
            orderHeaderFromDb.Email = OrderVM.OrderHeader.Email;
           



            _orderHRepo.Save();
            TempData[WebConstanta.Success] = "Order Detail Updated Successfuly!!!";
            return RedirectToAction("Details", "Order", new { id = orderHeaderFromDb.Id });

        }



    }
}
