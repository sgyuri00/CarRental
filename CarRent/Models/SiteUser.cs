using Microsoft.AspNetCore.Identity;

namespace CarRent.Models
{
    public class SiteUser : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? PlateNum { get; set; }
    }
}
