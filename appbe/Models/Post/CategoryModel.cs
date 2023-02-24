using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace appbe.Models.Post
{
    [Table("Category")]
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
 
        [StringLength(100, MinimumLength = 1, ErrorMessage = "{0} dài {1} đến {2}")]
        [Display(Name = "Tên danh mục")]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Nội dung danh mục")]
        public string? Description { get; set; }
        
    }
}
