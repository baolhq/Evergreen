using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.Repositories
{
    public interface IBlogRepository
    {
        void SaveBlog(Blog b);

        Blog GetBlogById(int id);

        void DeleteBlog(Blog b);

        void UpdateBlog(Blog b);

        List<Blog> GetBlogs();

    }
}
