using System;
namespace Blogsiotoh.Models
{
	public class User
	{
		public int Id { get; set; }
		public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public int status { get; set; } // 0 means blocked, else 1
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdate { get; set; }

    }
}

