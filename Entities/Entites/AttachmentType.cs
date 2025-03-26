using Domain.Attributes;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entites
{
    [Auditable]
    public class AttachmentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Extension { get; set; } // مثلاً: ".pdf", ".jpg"
        public List<FileUpload> FileUploads { get; set; }
    }
}
