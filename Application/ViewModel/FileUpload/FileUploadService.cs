using Application.Interfaces.Contexts;
using Domain.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModel.FileUpload
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IDataBaseContext _context;

        public FileUploadService(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<(List<FileUploadViewModel> Files, int TotalCount)> GetFilesAsync(string userName, int pageNumber, int pageSize)
        {
            var query = _context.FileUploads
                .Where(f => f.UploadedBy == userName);

            var totalCount = await query.CountAsync(); // تعداد کل فایل‌ها

            var files = await query
                .OrderBy(f => f.UploadDate) // مرتب‌سازی بر اساس تاریخ آپلود (اختیاری)
                .Skip((pageNumber - 1) * pageSize) // رد کردن آیتم‌های صفحات قبلی
                .Take(pageSize) // گرفتن تعداد مشخص
                .Select(f => new FileUploadViewModel
                {
                    Id = f.Id,
                    FileName = f.FileName,
                    AttachmentTypeId = f.AttachmentTypeId,
                    EntityType = f.EntityType,
                    EntityId = f.EntityId,
                    UploadedBy = f.UploadedBy,
                    UploadDate = f.UploadDate
                })
                .ToListAsync();

            return (files, totalCount);
        }
        public async Task<int> UploadFileAsync(FileUploadViewModel model, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("فایل انتخاب نشده یا خالی است.");
            }

            //var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            //var extension = Path.GetExtension(file.FileName).ToLower();
            //if (!allowedExtensions.Contains(extension))
            //{
            //    throw new ArgumentException("فرمت فایل مجاز نیست. فقط PDF و تصاویر (JPG, PNG) پذیرفته می‌شوند.");
            //}

            if (file.Length > 10 * 1024 * 1024)
            {
                throw new ArgumentException("اندازه فایل نمی‌تواند بیشتر از 10 مگابایت باشد.");
            }

            try
            {
                // دریافت نوع پیوست
                var attachmentType = await _context.AttachmentTypes
                    .FirstOrDefaultAsync(at => at.Id == model.AttachmentTypeId);
                if (attachmentType == null)
                {
                    throw new ArgumentException("نوع پیوست نامعتبر است.");
                }
                // اعتبارسنجی پسوند فایل با AllowedExtensions
                var fileExtension = Path.GetExtension(model.File.FileName).ToLower();
                var allowedExtensions = attachmentType.Extension.Split(',').Select(e => e.Trim().ToLower());
                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new ArgumentException($"پسوند فایل ({fileExtension}) با نوع پیوست ({attachmentType.Name}) مجاز نیست");
                }

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var fileUpload = new Domain.Entites.FileUpload
                {
                    // تنظیم نام فایل با پسوند از AttachmentType
                    FileName = model.File.FileName,
                    FileContent = memoryStream.ToArray(),
                    AttachmentTypeId = model.AttachmentTypeId,
                    EntityType = model.EntityType,
                    EntityId = model.EntityId,
                    UploadedBy = model.UploadedBy,
                    UploadDate = DateTime.Now
                };

                _context.FileUploads.Add(fileUpload);
                await _context.SaveChangesAsync();
                return fileUpload.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"خطا در ذخیره فایل: {ex.Message}", ex);
            }
        }
        public async Task<List<FileUploadViewModel>> GetFilesAsync(string userName)
        {
            return await _context.FileUploads
                .Where(f => f.EntityType == userName)
                .Select(f => new FileUploadViewModel
                {
                    Id = f.Id,
                    FileName = f.FileName,
                    AttachmentTypeId = f.AttachmentTypeId,
                    EntityType = f.EntityType,
                    EntityId = f.EntityId,
                    UploadedBy = f.UploadedBy,
                    UploadDate = f.UploadDate,
                    AttachmentType = f.AttachmentType.Name,
                }).ToListAsync();
        }
         public async Task<List<FileUploadViewModel>> GetFilesAsync(int id)
        {
            return await _context.FileUploads
                .Where(f => f.Id == id)
                .Select(f => new FileUploadViewModel
                {
                    Id = f.Id,
                    FileName = f.FileName,
                    AttachmentTypeId = f.AttachmentTypeId,
                    EntityType = f.EntityType,
                    EntityId = f.EntityId,
                    UploadedBy = f.UploadedBy,
                    UploadDate = f.UploadDate,
                    AttachmentType = f.AttachmentType.Name,
                }).ToListAsync();
        }

        public async Task<byte[]> DownloadFileAsync(int id)
        {
            var file = await _context.FileUploads.FindAsync(id);
            return file?.FileContent;
        }

        public async Task<FileUploadViewModel> GetFileByIdAsync(int id)
        {
            var result= await _context.FileUploads
           .Include(f => f.AttachmentType) // لود کردن اطلاعات AttachmentType
           .FirstOrDefaultAsync(f => f.Id == id );
            return new FileUploadViewModel
            {
            AttachmentTypeId= result.AttachmentTypeId,
            EntityId= result.EntityId,
            FileName= result.FileName,
            UploadedBy= result.UploadedBy,
            UploadDate= result.UploadDate,
            EntityType= result.EntityType,
            Id= result.Id,
            AttachmentType = result.AttachmentType.Extension,
            };
        }

        public async Task<bool> DeleteFileAsync(int id)
        {
            var file = await _context.FileUploads
        .FirstOrDefaultAsync(f => f.Id == id );
            if (file == null)
            {
                return false; // فایل پیدا نشد یا متعلق به کاربر نیست
            }

            _context.FileUploads.Remove(file);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
