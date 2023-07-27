using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Blogsiotoh.Models
{
	public class Login
	{
        [Required(ErrorMessage = "Please Enter Email")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [StringLength(6, ErrorMessage = "The {0} must be {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
    }
}

