using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Blogsiotoh.Models;
using Blogsiotoh.Utilities;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Hosting;

namespace Blogsiotoh.Services
{
	public class PostServices : IPostServices
    {
        private readonly BlogContext db;

        public PostServices(BlogContext _db)
        {
            db = _db;
        }
        public List<Blog> Posts()
        {
            List<Blog> blogs = new List<Blog>();
            blogs = db.Blogs.OrderByDescending(y => y.Id).ToList();
            return blogs;
        }

        public List<Blog> RecentPost()
        {
            List<Blog> blogs = new List<Blog>();
            blogs = db.Blogs.OrderByDescending(y => y.Id).Take(10).ToList();
            return blogs;
        }

        public List<Blog> UserRecentPost(int UserId)
        {
            List<Blog> blogs = new List<Blog>();
            blogs = db.Blogs.Where(y => y.UserId == UserId).OrderByDescending(y => y.Id).Take(10).ToList();
            return blogs;
        }

        public List<Blog> PostDetails(string PostId)
        {
            List<Blog> blogs = new List<Blog>();
            blogs = db.Blogs.Where(y => y.PostId == PostId).OrderByDescending(y => y.Id).Take(1).ToList();
            return blogs;
        }

        public List<Blog> UserPosts(int UserId)
        {
            List<Blog> blogs = new List<Blog>();
            blogs = db.Blogs.Where(y => y.UserId == UserId).OrderByDescending(y => y.Id).ToList();
            return blogs;
        }

        public async Task<List<Blog>> SortUserPostsByDate(PostByDate postByDate, int UserId)
        {
            List<Blog> blogs = new List<Blog>();
            blogs = db.Blogs.Where(y => y.UserId == UserId && (y.DateCreated.Date >= postByDate.DateFrom.Date && y.DateCreated.Date <= postByDate.DateTo.Date)).OrderByDescending(y => y.Id).ToList();
            return blogs;
        }

        public async Task<Response> CreatePost(Post post, string UserId)
        {
            Response response = new Response();
            response.Status = 0;

            post.PostId = RandomKeys.String(25);

            if (post.Title.Length < 1 || post.Title.Length > 200)
            {
                response.Message = "Post title should not be empty and not more than 200 characters";
            }

            else if (post.Content.Length < 1 || post.Content.Length > 5000)
            {
                response.Message = "Post body should not be empty and not more than 5000 characters";
            }

            else if (post.Image == null || post.Image == null)
            {
                response.Message = "Select an Image";
            }
            else if (post.Image.Count < 1)
            {
                response.Message = "Select an Image";
            }
            else
            {

                //check and save image
                string path = Path.Combine("wwwroot/", "Documents");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string ImagePath = null;

                List<string> uploadedFiles = new List<string>();
                foreach (IFormFile UploadFile in post.Image)
                {
                    string fileName = Path.GetFileName(UploadFile.FileName);
                    string fileExt = Path.GetFileName(UploadFile.ContentType);
                    ImagePath = fileName;

                    if (fileExt == ".png" || fileExt == ".jpeg" || fileExt == ".jpg")
                    {
                        response.Message = "Image type not supported";
                    }
                    else
                    {
                        response.Status = 1;
                        using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                        {
                            UploadFile.CopyTo(stream);
                            uploadedFiles.Add(fileName);
                        }
                    }
                }

                //save to database
                if(response.Status == 1)
                {
                    Blog blog = new Blog
                    {
                        Clicks = 0,
                        UserId = Int32.Parse(UserId),
                        PostId = post.PostId,
                        Image = ImagePath,
                        Title = post.Title,
                        Content = post.Content,
                        DateCreated = DateTime.Now,
                        DateUpdate = DateTime.Now
                    };
                    db.Add(blog);
                    db.SaveChanges();
                    response.Message = "Post Created!";
                }                

            }

            return response;
        }

        public async Task<List<Blog>> GetApiPostsAsync()
        {
            List<Blog> blog = new List<Blog>();
            string Uri = "https://mocki.io/v1/d33691f7-1eb5-45aa-9642-8d538f6c5ebd";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpClient httpClient = new HttpClient();
                    var response_ = await httpClient.GetAsync(Uri);
                    PostApi response = new PostApi();

                    if (response_.IsSuccessStatusCode)
                    {
                        string result = response_.Content.ReadAsStringAsync().Result;
                        response = Newtonsoft.Json.JsonConvert.DeserializeObject<PostApi>(result);
                        // response = JsonSerializer.DeserializeAsync<List<PostApi>>(result_);
                        if (response != null)
                        {
                             foreach(var x in response.data)
                            {
                                //check if already saved, else save new post
                                var blogs_ = db.Blogs.Where(y => y.Title == x.title && y.Content == x.description).FirstOrDefault();
                                if(blogs_ == null)
                                {
                                    Blog blogs = new Blog
                                    {
                                        Clicks = 0,
                                        UserId = 0,
                                        PostId = RandomKeys.String(25),
                                        Title = x.title,
                                        Content = x.description,
                                        Status = 1,
                                        DateCreated = x.publication_date,
                                        DateUpdate = DateTime.Now
                                    };
                                    db.Add(blogs);
                                    db.SaveChanges();
                                }
                                
                            }
                            
                        }
                    }
                    else
                    {
                        return null;
                    }
                }

                blog = db.Blogs.Where(y => y.Status == 1).OrderByDescending(y => y.Id).ToList();

            }
            catch (Exception ex)
            {
                return null;
            }
            return blog;
        }


    }
}

