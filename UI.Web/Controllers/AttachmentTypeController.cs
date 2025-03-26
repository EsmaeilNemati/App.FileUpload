using Application.ViewModel.AttachmentType;
using Microsoft.AspNetCore.Mvc;

namespace UI.Web.Controllers
{
   
        public class AttachmentTypeController : Controller
        {
            private readonly IAttachmentTypeService _attachmentTypeService;

            public AttachmentTypeController(IAttachmentTypeService attachmentTypeService)
            {
                _attachmentTypeService = attachmentTypeService;
            }

            public async Task<IActionResult> Index()
            {
                var attachmentTypes = await _attachmentTypeService.GetAllAttachmentTypesAsync();
                return View(attachmentTypes);
            }

            public IActionResult Create()
            {
                return View(new AttachmentTypeViewModel());
            }

            [HttpPost]
            public async Task<IActionResult> Create(AttachmentTypeViewModel model)
            {
                if (ModelState.IsValid)
                {
                    await _attachmentTypeService.AddAttachmentTypeAsync(model);
                    return RedirectToAction("Index");
                }
                return View(model);
            }
        }
    
}
