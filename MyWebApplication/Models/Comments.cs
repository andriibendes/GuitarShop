using System;
using System.Collections.Generic;

namespace MyWebApplication
{
    public partial class Comments
    {
        public Comments()
        {
            Guitarcomment = new HashSet<Guitarcomment>();
        }

        public int Id { get; set; }
        public string Info { get; set; }
        public int? Likes { get; set; }
        public int? Dislikes { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<Guitarcomment> Guitarcomment { get; set; }
    }
}
