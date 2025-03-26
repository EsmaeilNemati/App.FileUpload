using Application.ViewModel.Customers;
using Microsoft.AspNetCore.Mvc;

namespace UI.Web.Controllers
{
  
        public class CustomerController : Controller
        {
            private readonly ICustomerService _customerService;

            public CustomerController(ICustomerService customerService)
            {
                _customerService = customerService;
            }

            public async Task<IActionResult> Index()
            {
                var customers = await _customerService.GetAllCustomersAsync();
                return View(customers);
            }

            public IActionResult Create()
            {
                return View(new CustomerViewModel());
            }

            [HttpPost]
            public async Task<IActionResult> Create(CustomerViewModel model)
            {
                if (ModelState.IsValid)
                {
                    await _customerService.AddCustomerAsync(model);
                    return RedirectToAction("Index");
                }
                return View(model);
            }
        }
    
}
