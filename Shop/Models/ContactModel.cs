using Shop.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class ContactModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tiêu đề")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập bản đồ")]
        public string Map { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập SĐT")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập thông tin liên hệ")]
        public string Description { get; set; }


    }
}
