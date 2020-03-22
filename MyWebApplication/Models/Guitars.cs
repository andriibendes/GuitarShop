using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyWebApplication
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RangeUntilCurrentYearAttribute : RangeAttribute
    {
        public RangeUntilCurrentYearAttribute(int minimum) : base(minimum, DateTime.Now.Year)
        {
        }
    }
    public partial class Guitars
    {
        public Guitars()
        {
            Guitarcomment = new HashSet<Guitarcomment>();
            Guitarmusician = new HashSet<Guitarmusician>();
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "The field cannot be empty!")]
        [Remote(action: "VerifyName", controller: "Guitars")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field cannot be empty!")]
        [Display(Name = "Info")]
        public string Info { get; set; }

        [Required(ErrorMessage = "The field cannot be empty!"), Range(0, 999999, ErrorMessage = "Price is not correct!")]
        [Display(Name = "Price")]
        public int Cost { get; set; }

        [Required(ErrorMessage = "The field cannot be empty!"), RangeUntilCurrentYear(1900, ErrorMessage = "Year is not correct!")]
        [Display(Name = "Year")]
        public int Year { get; set; }

        public int FormId { get; set; }
        public int MaterialId { get; set; }
        public int TypeId { get; set; }
        public int BrandId { get; set; }

        [Display(Name = "Brand")]
        public virtual Brands Brand { get; set; }

        [Display(Name = "Form")]
        public virtual Forms Form { get; set; }

        [Display(Name = "Material")]
        public virtual Materials Material { get; set; }

        [Display(Name = "Type")]
        public virtual Types Type { get; set; }
        public virtual ICollection<Guitarcomment> Guitarcomment { get; set; }
        public virtual ICollection<Guitarmusician> Guitarmusician { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
