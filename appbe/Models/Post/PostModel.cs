using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace appbe.Models.Post
{
    [Table("Post")]
    public class PostModel
    {
        [Key]
        public int Id { set; get; }

        [Required(ErrorMessage = "Phải có tiêu đề bài viết")]
        [Display(Name = "Tiêu đề")]
        [StringLength(160, MinimumLength = 2, ErrorMessage = "{0} dài {1} đến {2}")]
        public string Title { set; get; }
        [Display(Name = "Mô tả ngắn")]
        public string Description { set; get; }
        [Display(Name = "Nội dung")]
        public string Content { set; get; }
        [Display(Name = "Xuất bản")]
        public bool Published { set; get; }
        [Display(Name = "Ngày tạo sản phẩm")]
        public DateTime? DateCreated { set; get; }
        [Display(Name = "Ngày cập nhật sản phẩm")]
        public DateTime? DateUpdated { set; get; }
        [Column(TypeName = "nvarchar(100)")]
        public string? ImageName { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        [NotMapped]
        public string? ImageSrc { get; set; }
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public CategoryModel? Category { get; set; }
    }
}
