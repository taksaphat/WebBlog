﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebBlog.Repositories;

namespace WebBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            var imgUrl = await _imageRepository.UploadAsync(file);

            if (imgUrl == null)
            {
                return Problem("Something went wrong",null, (int)HttpStatusCode.InternalServerError);
            }
            return new JsonResult(new { link = imgUrl });
        }
    }
}
