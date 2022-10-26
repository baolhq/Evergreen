using EvergreenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        

        public void DeleteBlog(Blog b)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var b1 = context.Blogs.SingleOrDefault(
                        c => c.BlogId == b.BlogId);
                    context.Blogs.Remove(b1);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }





        public Blog GetBlogById(int id)
        {
            Blog b = new Blog();
            try
            {
                using (var context = new AppDbContext())
                {
                    b = context.Blogs.SingleOrDefault(x => x.BlogId == id);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return b;
        }





        public List<Blog> GetBlogs()
        {
            var listBlogs = new List<Blog>();
            try
            {
                using (var context = new AppDbContext())
                {
                    listBlogs = context.Blogs.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listBlogs;
        }




        public void SaveBlog(Blog b)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    context.Blogs.Add(b);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(b);
                throw new Exception(e.Message);
            }
        }

        public void UpdateBlog(Blog b)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    context.Entry<Blog>(b).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
