using System;
using System.Collections.Generic;

namespace MyWebApplication
{
    public partial class Orders
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GuitarId { get; set; }
        public string Address { get; set; }
        public string DeliveryCost { get; set; }

        public virtual Guitars Guitar { get; set; }
        public virtual Users User { get; set; }
    }
}
