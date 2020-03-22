using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyWebApplication
{
    public partial class Materials
    {
        public Materials()
        {
            Guitars = new HashSet<Guitars>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "The field cannot be empty!"), RegularExpression("[A-Z][a-z]+", ErrorMessage = "The field is not correct!")]
        [Remote(action: "VerifyName", controller: "Materials")]
        [Display(Name = "Material")]
        public string Name { get; set; }

        public virtual ICollection<Guitars> Guitars { get; set; }
    }
}
