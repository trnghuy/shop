using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class ProductDetailsViewModel
    {
        public ProductModel ProductDetails { get; set; }
        public string Comment { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập tên")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        public string Email { get; set; }
    }
}
