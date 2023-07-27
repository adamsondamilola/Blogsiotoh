using System;
using Blogsiotoh.Models;

namespace Blogsiotoh.Services
{
	
        public interface IPostServices
    {
        Task<Response> CreatePost(Post post, string UserId);
        Task<List<Blog>> SortUserPostsByDate(PostByDate postByDate, int UserId);
        List<Blog> Posts();
        List<Blog> RecentPost();
        List<Blog> PostDetails(string PostId);
        List<Blog> UserRecentPost(int UserId);
        List<Blog> UserPosts(int UserId);
        Task<List<Blog>> GetApiPostsAsync();

    }


}

