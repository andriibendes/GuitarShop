using Microsoft.AspNetCore.Identity;

namespace MyWebApplication
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}

