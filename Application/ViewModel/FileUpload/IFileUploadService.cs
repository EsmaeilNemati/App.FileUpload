using Domain.Entites;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModel.FileUpload
{
    public interface IFileUploadService
    {
        Task<(List<FileUploadViewModel> Files, int TotalCount)> GetFilesAsync(string userName, int pageNumber, int pageSize); // تغییر برای پیجینگ
        Task<int> UploadFileAsync(FileUploadViewModel model, IFormFile file); // تغییر اینجا
        Task<List<FileUploadViewModel>> GetFilesAsync(string userName);
        Task<List<FileUploadViewModel>> GetFilesAsync(int id);
        Task<byte[]> DownloadFileAsync(int id);
        Task<FileUploadViewModel> GetFileByIdAsync(int id);
        Task<bool> DeleteFileAsync(int id); // متد جدید برای حذف
    }
}

