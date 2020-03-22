using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MyWebApplication
{
    public class CountryNameValidator : ValidationAttribute
    {
        public CountryNameValidator()
        {
        }
        public string GetErrorMessage() => $"Country name is not correct!";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string n = value.ToString();
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            List<string> countries = new List<string>();
            foreach (CultureInfo culture in cultures)
            {
                RegionInfo region = new RegionInfo(culture.Name);
                if (!(countries.Contains(region.EnglishName)))
                {
                    countries.Add(region.EnglishName);
                }
            }
            foreach (string name in countries)
            { 
                if (n == name)
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(GetErrorMessage());
        }

    }

    public partial class Brands
    {
        public Brands()
        {
            Guitars = new HashSet<Guitars>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "The field cannot be empty!"), RegularExpression("[A-Z][a-z]+", ErrorMessage = "The name is not correct!")]
        [Remote(action: "VerifyName", controller: "Brands")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The field cannot be empty!"), CountryNameValidator()]
        [Display(Name = "Country")]
        public string Country { get; set; }
        public virtual ICollection<Guitars> Guitars { get; set; }
    }
}
