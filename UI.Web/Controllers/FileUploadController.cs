using Application.ViewModel.AttachmentType;
using Application.ViewModel.Customers;
using Application.ViewModel.FileUpload;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UI.Web.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly IAttachmentTypeService _attachmentTypeService;
        private readonly ICustomerService _customerService;

        public FileUploadController(IFileUploadService fileUploadService,
                                   IAttachmentTypeService attachmentTypeService,
                                   ICustomerService customerService)
        {
            _fileUploadService = fileUploadService;
            _attachmentTypeService = attachmentTypeService;
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            //var files = await _fileUploadService.GetFilesAsync(User.Identity?.Name ?? "Anonymous");
            var entitiType = "Customer";//get by entiti
            var files = await _fileUploadService.GetFilesAsync(entitiType);
            return View(files);
        }

        public async Task<IActionResult> Upload()
        {
            ViewBag.AttachmentTypes = await _attachmentTypeService.GetAllAttachmentTypesAsync();
            ViewBag.Customers = await _customerService.GetAllCustomersAsync();
            return View(new FileUploadViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Upload(FileUploadViewModel model, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "لطفاً یک فایل انتخاب کنید.";
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.UploadedBy = User.Identity?.Name ?? "کاربر ";
                    var fileId = await _fileUploadService.UploadFileAsync(model, file); // تغییر اینجا
                    TempData["SuccessMessage"] = "فایل با موفقیت آپلود شد.";
                    return RedirectToAction("Index");
                }
                catch (ArgumentException ex)
                {
                    TempData["ErrorMessage"] = ex.Message; // خطاهای خاص مثل "نوع پیوست نامعتبر است"
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"خطا در آپلود فایل: {ex.Message}");
                }
            }

            ViewBag.AttachmentTypes = await _attachmentTypeService.GetAllAttachmentTypesAsync();
            ViewBag.Customers = await _customerService.GetAllCustomersAsync();
            return View(model);
        }
        public async Task<IActionResult> Download(int id)
        {
            // دریافت محتوای فایل
            var fileContent = await _fileUploadService.DownloadFileAsync(id);
            if (fileContent == null) return NotFound();

            // دریافت اطلاعات فایل با AttachmentType
            var file =await _fileUploadService.GetFileByIdAsync(id);
               
            if (file == null) return NotFound();

            // تنظیم نام فایل با پسوند از AttachmentType
            var fileName = file.FileName; // نام فایل که در آپلود تنظیم شده
            var contentType = GetContentType(file.AttachmentType??"");

            return File(fileContent, contentType, fileName);
        }

        // متد کمکی برای تشخیص نوع محتوا
        private string GetContentType(string extension)
        {
            return extension.ToLowerInvariant() switch
            {
                ".pdf" => "application/pdf",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".txt" => "text/plain",
                _ => "application/octet-stream"
            };
        }

        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _fileUploadService.DeleteFileAsync(id);
            if (deleted)
            {
                TempData["SuccessMessage"] = "فایل با موفقیت حذف شد";
            }
            else
            {
                TempData["ErrorMessage"] = "فایل پیدا نشد یا شما اجازه حذف آن را ندارید";
            }
            return RedirectToAction("Index");
        }

       


        public async Task<IActionResult> ViewFile(int id)
        {
            var fileContent = await _fileUploadService.DownloadFileAsync(id);
            if (fileContent == null) return NotFound();

            var file = await _fileUploadService.GetFileByIdAsync(id);
            if (file == null) return NotFound();

            string content;
            string extension = Path.GetExtension(file.FileName).ToLower();

            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                    // برای تصاویر، base64 با نوع MIME درست
                    content = $"<img src='data:image/{(extension == ".png" ? "png" : "jpeg")};base64,{Convert.ToBase64String(fileContent)}' style='max-width: 100%; height: auto;' />";
                    break;
                case ".pdf":
                    // برای PDF، پیام ساده یا iframe (در اینجا پیام نگه داشتم)
                    content = "پیش‌نمایش PDF در این مدال پشتیبانی نمی‌شود. لطفاً فایل را دانلود کنید.";
                    break;
                default:
                    // برای متن یا فرمت‌های ناشناخته
                    try
                    {
                        content = System.Text.Encoding.UTF8.GetString(fileContent);
                    }
                    catch
                    {
                        content = "محتوای این فایل قابل نمایش نیست.";
                    }
                    break;
            }

            ViewData["FileName"] = file.FileName;
            ViewData["FileContent"] = content;
            return PartialView("_ViewFileModal");
        }
    }
}
