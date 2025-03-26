using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModel.FileUpload
{
    public class FileUploadViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "نام فایل الزامی است")]
        public string FileName { get; set; }
        public IFormFile File { get; set; }
        [Required(ErrorMessage = "نوع پیوست الزامی است")]
        public int AttachmentTypeId { get; set; }
        [Required(ErrorMessage = "موجودیت الزامی است")]
        public string EntityType { get; set; }
        [Required(ErrorMessage = "شناسه موجودیت الزامی است")]
        public int EntityId { get; set; }
        public string UploadedBy { get; set; }
        public string? AttachmentType { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
