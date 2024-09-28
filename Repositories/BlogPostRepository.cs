using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WebBlog.Data;
using WebBlog.Models.Domain;

namespace WebBlog.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BlogDbContext _dbContext;

        public BlogPostRepository(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blog)
        {
            await _dbContext.AddAsync(blog);
            await _dbContext.SaveChangesAsync();
            return blog;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _dbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await _dbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blog)
        {
            var existBlog = await _dbContext.BlogPosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == blog.Id);

            if (existBlog != null)
            {
                existBlog.Id = blog.Id;
                existBlog.Heading = blog.Heading;
                existBlog.PageTitle = blog.PageTitle;
                existBlog.Content = blog.Content;
                existBlog.Author = blog.Author;
                existBlog.ShortDescription = blog.ShortDescription; 
                existBlog.FeaturedImageUrl = blog.FeaturedImageUrl;
                existBlog.UrlHandle = blog.UrlHandle;
                existBlog.PublishedDate = blog.PublishedDate;
                existBlog.Visible = blog.Visible;
                existBlog.Tags = blog.Tags;

                await _dbContext.SaveChangesAsync();
                return existBlog;
            }
            return null;
        }
    }
}
