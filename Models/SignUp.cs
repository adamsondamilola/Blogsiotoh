using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Blogsiotoh.Models
{
	public class SignUp
	{
        [Required(ErrorMessage = "Please Enter Email")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please Enter First Name")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [StringLength(6, ErrorMessage = "The {0} must be {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Please Confirm Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [StringLength(6, ErrorMessage = "The {0} must be {1} characters long.", MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "Password do not match.")]
        public string? Password2 { get; set; }
    }
}

