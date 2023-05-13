using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Leo.Model.Domains
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
