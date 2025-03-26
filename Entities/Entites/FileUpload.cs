using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Attributes;
namespace Domain.Entites
{
    [Auditable]

    public class FileUpload
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public int AttachmentTypeId { get; set; }
        public AttachmentType AttachmentType { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string UploadedBy { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
