using Microsoft.AspNetCore.Hosting.Server;
using Shop.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class SliderModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên Slider")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Status {  get; set; }
        public string Image {  get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }
    }
}
