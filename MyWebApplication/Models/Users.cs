using System;
using System.Collections.Generic;

namespace MyWebApplication
{
    public partial class Users
    {
        public Users()
        {
            Comments = new HashSet<Comments>();
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
