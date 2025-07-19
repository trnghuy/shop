using Shop.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tên Sản phẩm")]
        public string Name { get; set; }
        public string Slug { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập Giá Sản phẩm")]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Chọn 1 thương hiệu")]
        public int BrandId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Chọn 1 loại")]
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public int Sold { get; set; }
        


        public CategoryModel Category { get; set; }
        public BrandModel Brand { get; set; }
        public RatingModel Ratings { get; set; }
        public string Image { get; set; } 
        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
    }
}
