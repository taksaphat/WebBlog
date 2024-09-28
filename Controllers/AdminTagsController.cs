using Microsoft.AspNetCore.Mvc;
using WebBlog.Data;
using WebBlog.Models.Domain;
using WebBlog.Models.ViewModels;
using WebBlog.Repositories;

namespace WebBlog.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository _tagRepository;
        public AdminTagsController(ITagRepository tagRepository) 
        {
            _tagRepository = tagRepository;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> SaveTag(AddTagRequest addTags)
        {
            var tags = new Tag
            {
                Name = addTags.Name,
                DisplayName = addTags.DisplayName,
            };
            //var name = tags.Name;
            //var displayName = tags.DisplayName;
            await _tagRepository.AddAsync(tags);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var allTags = await _tagRepository.GetAllAsync();
            return View(allTags);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await _tagRepository.GetAsync(id);
            if (tag != null)
            {
                var editTagReq = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagReq);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTag) 
        { 
            var tag = new Tag { Id = editTag.Id, Name = editTag.Name,DisplayName = editTag.DisplayName };
            var updatedTag = await _tagRepository.UpdateAsync(tag);
            //return View(updatedTag);
            //var existingTag = _blogDbContext.Tags.Find(tag.Id);
            //if (existingTag != null)
            //{
            //    existingTag.Name = tag.Name;
            //    existingTag.DisplayName = tag.DisplayName;
            //    _blogDbContext.SaveChanges();
            //    return RedirectToAction("Edit", new {id = editTag.Id});
            //}
            return RedirectToAction("List", new { id = updatedTag.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTag)
        {
            var deletedTag = await _tagRepository.DeleteAsync(editTag.Id);
            if (deletedTag != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new {id = editTag.Id});
        }
    }

}
