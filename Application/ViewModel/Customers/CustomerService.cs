using Application.Interfaces.Contexts;
using Domain.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModel.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly IDataBaseContext _context;

        public CustomerService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerViewModel>> GetAllCustomersAsync()
        {
            return await _context.Customers
                .Select(c => new CustomerViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToListAsync();
        }

        public async Task AddCustomerAsync(CustomerViewModel model)
        {
            var customer = new Customer
            {
                Name = model.Name
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }
    }
}
