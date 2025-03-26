using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModel.AttachmentType
{
    public class AttachmentTypeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "نام نوع پیوست الزامی است")]
        public string Name { get; set; }
    }
}
