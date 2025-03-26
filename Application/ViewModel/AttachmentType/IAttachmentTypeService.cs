using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModel.AttachmentType
{
    public interface IAttachmentTypeService
    {
        Task<List<AttachmentTypeViewModel>> GetAllAttachmentTypesAsync();
        Task AddAttachmentTypeAsync(AttachmentTypeViewModel model);
    }
}
