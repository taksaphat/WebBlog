﻿using Microsoft.AspNetCore.Mvc;
using WebBlog.Repositories;

namespace WebBlog.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository _blogPostRepository;
        public BlogsController(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var blog = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);
            return View(blog);
        }
    }
}
