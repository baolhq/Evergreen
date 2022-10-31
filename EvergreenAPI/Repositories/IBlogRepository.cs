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
        bool SaveBlog(Blog b);

        Blog GetBlogById(int id);

        bool DeleteBlog(Blog b);

        bool UpdateBlog(Blog b);

        ICollection<Blog> GetBlogs();
        bool BlogExist(int id);
        bool Save();

    }
}
