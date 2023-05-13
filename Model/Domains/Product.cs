using System.ComponentModel.DataAnnotations;

namespace Leo.Model.Domains
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        [StringLength(100)] 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
