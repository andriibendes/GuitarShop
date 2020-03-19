using System;
using System.Collections.Generic;

namespace MyWebApplication
{
    public partial class Guitarcomment
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public int GuitarId { get; set; }

        public virtual Comments Comment { get; set; }
        public virtual Guitars Guitar { get; set; }
    }
}
