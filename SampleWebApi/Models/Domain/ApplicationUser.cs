using Microsoft.AspNetCore.Identity;

namespace SampleWebApi.Models.Domain
{
    public class ApplicationUser:IdentityUser
    {
        public string? FullName { get; set; }
        public string? SchoolName { get; set; }

        public string? SchoolAddress { get; set; }
    }
}
