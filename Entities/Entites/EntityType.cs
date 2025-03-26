using Domain.Attributes;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entites
{
    [Auditable]

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<FileUpload> FileUploads { get; set; }
    }
}
