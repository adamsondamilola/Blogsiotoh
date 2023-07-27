using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blogsiotoh.Models;
using Blogsiotoh.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blogsiotoh.Controllers
{
    public class PostController : Controller
    {
        public readonly BlogContext db;
        private readonly IPostServices postServices;

        public PostController(BlogContext _db, IPostServices _postServices)
        {
            db = _db;
           postServices = _postServices;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            if (!IfLogged()) return RedirectToAction("Index", "Home");
            string UserId = HttpContext.Session.GetString("Id");
            List<Blog> blogs = new List<Blog>();
            blogs = postServices.UserPosts(Int32.Parse(UserId));
            ViewBag.posts = blogs;
            ViewBag.status = null;
            ViewBag.message = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(PostByDate postByDate)
        {
            ViewBag.status = 0;
            ViewBag.message = null;
            ViewBag.posts = null;
            if (!IfLogged()) return RedirectToAction("Index", "Home");
            int UserId = Int32.Parse(HttpContext.Session.GetString("Id"));
            List<Blog> blogs = new List<Blog>();
            blogs = await postServices.SortUserPostsByDate(postByDate, UserId);
            if(blogs.Count() < 1)
            {
                blogs = postServices.UserPosts(UserId);
                ViewBag.message = "No post found";
            }
            ViewBag.posts = blogs;
            return View();
        }

        [Route("Post/View/{PostId}")]
        public IActionResult View(string PostId)
        {
            List<Blog> blogs = new List<Blog>();
            blogs = postServices.PostDetails(PostId);
            ViewBag.posts = blogs;
            ViewBag.recent = postServices.RecentPost();
            ViewBag.status = null;
            ViewBag.message = null;
            return View();
        }
       


        public IActionResult Create()
        {
            if (!IfLogged()) return RedirectToAction("Index", "Home");
            string UserId = HttpContext.Session.GetString("Id");
            List<Blog> blogs = new List<Blog>();
            blogs = postServices.UserRecentPost(Int32.Parse(UserId));
            ViewBag.recent = blogs;
            ViewBag.status = null;
            ViewBag.message = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Post post, string UserId)
        {
            Response response = new Response();
            response.Status = 0;
            if (IfLogged())
            {
                UserId = HttpContext.Session.GetString("Id");
                response = await postServices.CreatePost(post, UserId);
            }
            else
            {
                response.Message = "You must be logged in to create post";
            }

            List<Blog> blogs = new List<Blog>();
            blogs = postServices.UserRecentPost(Int32.Parse(UserId));
            ViewBag.recent = blogs;

            ViewBag.status = response.Status;
            ViewBag.message = response.Message;
            return View();
        }

        public bool IfLogged()
        {
            if (HttpContext.Session.GetString("Id") == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}

