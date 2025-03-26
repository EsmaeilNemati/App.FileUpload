using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModel.Customers
{
    public interface ICustomerService
    {
        Task<List<CustomerViewModel>> GetAllCustomersAsync();
        Task AddCustomerAsync(CustomerViewModel model);
    }
}
