using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập email"),EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password), Required(ErrorMessage = "Vui lòng nhập mật khẩu")]   
        public string Password { get; set; }
    }
}
