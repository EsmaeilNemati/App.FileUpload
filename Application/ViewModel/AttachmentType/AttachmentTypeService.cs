using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModel.AttachmentType
{
    public class AttachmentTypeService : IAttachmentTypeService
    {
        private readonly IDataBaseContext _context;

        public AttachmentTypeService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<List<AttachmentTypeViewModel>> GetAllAttachmentTypesAsync()
        {
            return await _context.AttachmentTypes
                .Select(a => new AttachmentTypeViewModel
                {
                    Id = a.Id,
                    Name = a.Name
                }).ToListAsync();
        }

        public async Task AddAttachmentTypeAsync(AttachmentTypeViewModel model)
        {
            var attachmentType = new Domain.Entites.AttachmentType
            {
                Name = model.Name
            };
            _context.AttachmentTypes.Add(attachmentType);
            await _context.SaveChangesAsync();
        }
    }
}
