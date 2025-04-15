using Microsoft.AspNetCore.Identity;

namespace Fertilizer360.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }
}
