using System;
using System.Collections.Generic;

namespace MyWebApplication
{
    public partial class Guitarmusician
    {
        public int Id { get; set; }
        public int MusicianId { get; set; }
        public int GuitarId { get; set; }

        public virtual Guitars Guitar { get; set; }
        public virtual Musicians Musician { get; set; }
    }
}
