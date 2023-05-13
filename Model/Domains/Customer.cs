using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Leo.Model.Domains
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string ImageUrl { get; set; }
        [JsonIgnore]
        public virtual ICollection<Product>? Products { get; set; }
        
    }
}
