using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyWebApplication
{
    public partial class Musicians
    {
        public Musicians()
        {
            Guitarmusician = new HashSet<Guitarmusician>();
        }

        public int Id { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "The field cannot be empty!"), RegularExpression("[A-Z][a-z]+ [A-Z][a-z]+", ErrorMessage = "The name is not correct!")]
        public string Name { get; set; }

        public virtual ICollection<Guitarmusician> Guitarmusician { get; set; }
    }
}
