using System.ComponentModel.DataAnnotations;

namespace Leo.Model.DTOs
{
    public class CustomersDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public IFormFile ImageUrl { get; set; }
    }
}
