using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace VillaApi.Model
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string firstName { get; set; }
        
    }
}
