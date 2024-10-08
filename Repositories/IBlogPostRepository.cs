﻿using WebBlog.Models.Domain;

namespace WebBlog.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost?> GetAsync(Guid id);
        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);
        Task<BlogPost> AddAsync(BlogPost blog);
        Task<BlogPost?> UpdateAsync(BlogPost blog);
        Task<BlogPost?> DeleteAsync(Guid id);
    }
}
