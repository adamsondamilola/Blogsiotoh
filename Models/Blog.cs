using System;
namespace Blogsiotoh.Models
{
	public class Blog
	{
		public int Id { get; set; }
        public int? UserId { get; set; }
        public int Clicks { get; set; }
        public string? PostId { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public string? Content { get; set; }
        public int Status { get; set; } //API 1, else 0
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdate { get; set; }
    }
}

