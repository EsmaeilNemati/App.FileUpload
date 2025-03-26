using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModel.Customers
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "نام مشتری الزامی است")]
        public string Name { get; set; }
    }
}
