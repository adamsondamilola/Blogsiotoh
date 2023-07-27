using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Blogsiotoh.Models
{
	public class Post
	{
        public string? PostId { get; set; }
        [Required(ErrorMessage = "Please Enter Title")]
        [Display(Name = "Title")]
        public string? Title { get; set; }


        [Required(ErrorMessage = "Please Upload an Image")]
        [Display(Name = "Image")]
        public List<IFormFile>? Image { get; set; }


        [Required(ErrorMessage = "Please Enter Content")]
        [Display(Name = "Content")]
        public string? Content { get; set; }
    }
}

