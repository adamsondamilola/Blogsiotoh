using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blogsiotoh.Models;
using Blogsiotoh.Services;

namespace Blogsiotoh.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly IPostServices postServices;

  

    public HomeController(ILogger<HomeController> logger, IPostServices _postServices)
    {
        _logger = logger;
        postServices = _postServices;
    }

    public async Task<IActionResult> Index()
    {
        List<Blog> blogs = new List<Blog>();
        blogs = postServices.Posts();
        List<Blog> apiBlogs = new List<Blog>();
        apiBlogs = await postServices.GetApiPostsAsync();
        ViewBag.posts = blogs;
        ViewBag.apiPosts = apiBlogs;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

