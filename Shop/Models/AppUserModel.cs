using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class AppUserModel : IdentityUser
    {
        public string RoleId { get; set; }
        public string Token { get; set; }
    }
}
