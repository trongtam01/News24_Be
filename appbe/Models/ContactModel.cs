using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace appbe.Models
{
    [Table("Contact")]
    public class ContactModel
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        [Required(ErrorMessage = "Phải nhập  {0}")]
        [Display(Name = "Họ Tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Phải nhập  {0}")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Phải là địa chỉ email")]
        [Display(Name = "Địa chỉ email")]
        public string Email { get; set; }

        public DateTime DateSent { get; set; }

        [StringLength(100)]
        [Display(Name = "Số điện thoại")]
        public string Subject { get; set; }

        [Display(Name = "Nội dung")]
        public string Message { get; set; }

    }
}
