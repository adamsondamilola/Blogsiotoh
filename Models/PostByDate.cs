using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Blogsiotoh.Models
{
	public class PostByDate
	{
        [Required(ErrorMessage = "Please Select From Date")]
        [Display(Name = "From")]
        public DateTime DateFrom { get; set; }

        [Required(ErrorMessage = "Please Select To Date")]
        [Display(Name = "To")]
        public DateTime DateTo { get; set; }
    }
}

