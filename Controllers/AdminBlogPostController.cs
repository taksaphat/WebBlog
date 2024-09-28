using Microsoft.AspNetCore.Mvc;
using WebBlog.Models.ViewModels;
using WebBlog.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBlog.Models.Domain;

namespace WebBlog.Controllers
{
    public class AdminBlogPostController : Controller
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ITagRepository _tagRepository;
        public AdminBlogPostController(IBlogPostRepository blogPostRepository, ITagRepository tagRepository)
        {
            _blogPostRepository = blogPostRepository;
            _tagRepository = tagRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await _blogPostRepository.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,

            };
            var listTags = new List<Tag>();
            foreach (var item in addBlogPostRequest.SelectedTags)
            {
                var selectedId = Guid.Parse(item);
                var tags = await _tagRepository.GetAsync(selectedId);
                if (tags != null)
                {
                    listTags.Add(tags);
                }
            }

            blogPost.Tags = listTags;
            await _blogPostRepository.AddAsync(blogPost);
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var blogList = await _blogPostRepository.GetAllAsync();

            return View(blogList);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var post = await _blogPostRepository.GetAsync(id);
            var tagModel = await _tagRepository.GetAllAsync();
            if (post != null)
            {
                var model = new EditBlogPostRequest
                {
                    Id = post.Id,
                    Heading = post.Heading,
                    PageTitle = post.PageTitle,
                    Content = post.Content,
                    Author = post.Author,
                    FeaturedImageUrl = post.FeaturedImageUrl,
                    UrlHandle = post.UrlHandle,
                    ShortDescription = post.ShortDescription,
                    PublishedDate = post.PublishedDate,
                    Visible = post.Visible,
                    Tags = tagModel.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                    SelectedTags = post.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            var blogPostModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                PublishedDate = editBlogPostRequest.PublishedDate,
                UrlHandle = editBlogPostRequest.UrlHandle,
                Visible = editBlogPostRequest.Visible,
            };

            var tags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await _tagRepository.GetAsync(tag);
                    if (foundTag != null)
                    {
                        tags.Add(foundTag);
                    }
                }
            }

            blogPostModel.Tags = tags;

            var updatedBlog = await _blogPostRepository.UpdateAsync(blogPostModel);
            if (updatedBlog != null)
            {
                return RedirectToAction("Edit");
            }
            return RedirectToAction("Edit"); // test commit
        }
    }
}
