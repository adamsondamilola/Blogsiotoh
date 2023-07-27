using System;
using System.Collections.Generic;

namespace Blogsiotoh.Models
{
	public class PostApi
	{
        public List<Data>? data { get; set; }
    }
    public class Data
    {
        public string? title { get; set; }
        public string? description { get; set; }
        public DateTime publication_date { get; set; }
    }
}

