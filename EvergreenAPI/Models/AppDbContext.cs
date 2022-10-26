using Microsoft.EntityFrameworkCore;

namespace EvergreenAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<DiseaseCategory> DiseaseCategories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicineCategory> MedicineCategories { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<PlantCategory> PlantCategories { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
    }
}
