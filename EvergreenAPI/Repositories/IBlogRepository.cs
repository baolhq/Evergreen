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
        ICollection<Image> GetImages();
        ICollection<Blog> GetBlogs();
        Blog GetBlog(int id);
        bool BlogExist(int id);
        bool CreateBlog(Blog b);
        bool DeleteBlog(Blog b);
        bool UpdateBlog(Blog b);
        bool Save();
        List<Blog> Search(string search);

    }
}
