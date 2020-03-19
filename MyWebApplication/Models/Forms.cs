﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyWebApplication
{
    public partial class Forms
    {
        public Forms()
        {
            Guitars = new HashSet<Guitars>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "The field cannot be empty!"), RegularExpression("[A-Z][a-z]+", ErrorMessage = "The field is not correct!")]
        [Display(Name = "Форма")]

        public string Name { get; set; }

        public virtual ICollection<Guitars> Guitars { get; set; }
    }
}
