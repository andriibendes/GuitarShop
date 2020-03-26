using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyWebApplication
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}

