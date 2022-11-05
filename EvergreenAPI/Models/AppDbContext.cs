using System;
using Microsoft.EntityFrameworkCore;

namespace EvergreenAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Image> Images { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<DiseaseCategory> DiseaseCategories { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<MedicineCategory> MedicineCategories { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<PlantCategory> PlantCategories { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Treatment> Treatments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Blog seed

            modelBuilder.Entity<Blog>().HasData(
                new Blog
                {
                    BlogId = 1,
                    Title = "Hello world with C#",
                    Description = "Lorem ipsum",
                    Content = "Lorem ipsum dolor sit amet",
                    LastModifiedDate = DateTime.Today,
                    ViewCount = 10
                },
                new Blog
                {
                    BlogId = 2,
                    Title = "Something different",
                    Description = "Lorem ipsum 2",
                    Content = "Lorem ipsum dolor sit amet 2",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(2),
                    ViewCount = 3
                },
                new Blog
                {
                    BlogId = 3,
                    Title = "Another blog",
                    Description = "Lorem ipsum 3",
                    Content = "Lorem ipsum dolor sit amet 3",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(10),
                    ViewCount = 30
                });

            #endregion

            #region Account seed

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    AccountId = 1,
                    Username = "admin",
                    Password = "Admin@",
                    FullName = "Admin",
                    Email = "admin@gmail.com",
                    Role = "admin",
                    IsBlocked = false,
                    Professions = null
                },
                new Account
                {
                    AccountId = 2,
                    Username = "anv",
                    Password = "123123",
                    FullName = "Nguyen Van A",
                    Email = "anv@gmail.com",
                    Role = "user",
                    IsBlocked = false,
                    Professions = null
                },
                new Account
                {
                    AccountId = 3,
                    Username = "pbc",
                    Password = "ll123",
                    FullName = "Phan Bao Chau",
                    Email = "pbc@gmail.com",
                    Role = "professor",
                    IsBlocked = false,
                    Professions = "Toan"
                }
            );

            #endregion

            #region Images seed
            modelBuilder.Entity<Image>().HasData(
                new Image { ImageId = 1, Url = "https://www.fao.org.vn/wp-content/uploads/2019/08/benh-vang-la-greening.jpg", AltText = "Hình ảnh bệnh vàng lá" },
                new Image { ImageId = 2, Url = "https://i0.wp.com/trongraulamvuon.com/wp-content/uploads/2013/11/sau-duc-than-hong.jpg", AltText = "Hình ảnh bệnh sâu đục thân" },
                new Image { ImageId = 3, Url = "https://hoadepviet.com/wp-content/uploads/2016/11/benh-hoa-hong-1.jpg", AltText = "Hình ảnh bệnh lá úa sớm" }
            );
            #endregion

            #region Disease seed

            modelBuilder.Entity<DiseaseCategory>().HasData(
                new DiseaseCategory
                {
                    DiseaseCategoryId = 1,
                    Name = "Bệnh trên lá"
                },
                new DiseaseCategory
                {
                    DiseaseCategoryId = 2,
                    Name = "Bệnh trên thân"
                }
            );

            #endregion

            #region Disease seed

            modelBuilder.Entity<Disease>().HasData(
                new Disease
                {
                    DiseaseId = 1,
                    Name = "Vàng lá",
                    Identification = "Lá bị vàng",
                    Affect = "Giảm sản lượng",
                    DiseaseCategoryId = 1,
                    ImageId = 1
                },
                new Disease
                {
                    DiseaseId = 2,
                    Name = "Sâu đục thân",
                    Identification = "Các lỗ nhỏ trên vỏ cây",
                    Affect = "Làm cây chết khô",
                    DiseaseCategoryId = 2,
                    ImageId = 2
                },
                new Disease
                {
                    DiseaseId = 3,
                    Name = "Đốm vòng (Úa sớm)",
                    Identification = "Vết bệnh xuất hiện đầu tiên ở các lá già phía dưới, lan dần lên các lá phía trên. Vết bệnh hình tròn hay có cạnh, màu nâu sẫm, có các vòng tròn đồng tâm màu đen, nhiều vết bệnh liên kết nhau thành vết lớn lan khắp lá, làm lá vàng khô và rụng sớm.",
                    Affect = "Sợi nấm và bào tử nấm lưu tồn trên tàn dư cây bệnh, cây ký chủ ít nhất là một năm, bào tử theo gió và côn trùng lan truyền gây bệnh trên cây trồng.",
                    DiseaseCategoryId = 1,
                    ImageId = 3
                }
            );

            #endregion

            #region MedicineCategory seed

            modelBuilder.Entity<MedicineCategory>().HasData(
              new MedicineCategory { MedicineCategoryId = 1, Name = "Thuốc phun" },
              new MedicineCategory { MedicineCategoryId = 2, Name = "Thuốc pha" }
            );

            #endregion

            #region Medicine seed

            modelBuilder.Entity<Medicine>().HasData(
                new Medicine
                {
                    MedicineId = 1,
                    Name = "Chaetumium",
                    Uses = "Trị bệnh vàng lá",
                    MedicineCategoryId = 1
                },
                new Medicine
                {
                    MedicineId = 2,
                    Name = "Basudin",
                    Uses = "Trừ sâu trong thân cây",
                    MedicineCategoryId = 2
                },
                new Medicine
                {
                    MedicineId = 3,
                    Name = "Aviso",
                    Uses = "Trị lá úa sớm",
                    MedicineCategoryId = 1
                }
            );

            #endregion

            #region PlantCategory seed

            modelBuilder.Entity<PlantCategory>().HasData(
                new PlantCategory { PlantCategoryId = 1, Name = "Cây lương thực" },
                new PlantCategory { PlantCategoryId = 2, Name = "Cây rau" }
            );

            #endregion

            #region Plant seed

            modelBuilder.Entity<Plant>().HasData(
                new Plant
                {
                    PlantId = 1,
                    Name = "Cà chua",
                    Description = "Cà chua được phát triển trên toàn thế giới do sự tăng trưởng tối ưu của nó trong nhiều điều kiện phát triển khác nhau. Các loại cà chua được trồng trọt phổ biến nhất là loại quả đường kính khoảng 5–6 cm. Hầu hết các giống được trồng đề cho ra trái cây màu đỏ, nhưng một số giống cho quả vàng, cam, hồng, tím, xanh lá cây, đen hoặc màu trắng. Đặc biệt có loại cà chua nhiều màu và có sọc.",
                    PlantCategoryId = 2
                },
                new Plant
                {
                    PlantId = 2,
                    Name = "Lúa",
                    Description = "Lúa là một trong năm loại cây lương thực chính của thế giới, cùng với bắp, lúa mì, sắn và khoai tây. Theo quan niệm xưa lúa cũng là một trong sáu loại lương thực chủ yếu trong Lục cốc.",
                    PlantCategoryId = 1
                },
                new Plant
                {
                    PlantId = 3,
                    Name = "Dưa chuột",
                    Description = "Dưa chuột là một cây trồng phổ biến trong họ bầu bí, là loại rau ăn quả thương mại quan trọng, nó được trồng lâu đời trên thế giới và trở thành thực phẩm của nhiều nước. Những nước dẫn đầu về diện tích gieo trồng và năng suất là: Trung Quốc, Nga, Nhật Bản, Mỹ, Hà Lan, Thổ Nhĩ Kỳ, Ba Lan, Ai Cập và Tây Ban Nha.",
                    PlantCategoryId = 2
                }
            );

            #endregion

            #region Treatment seed
            modelBuilder.Entity<Treatment>().HasData(
                new Treatment { TreatmentId = 1, Method = "Bắt sâu", DiseaseId = 2 },
                new Treatment { TreatmentId = 2, Method = "Lặt bỏ lá bị nhiễm bệnh", DiseaseId = 3 }
            );
            #endregion

            #region Message seed
            modelBuilder.Entity<Message>().HasData(
                new Message
                {
                    MessageId = 1,
                    ReplyToId = 0,
                    Content = "Hello",
                    CreatedOn = DateTime.Today - TimeSpan.FromMinutes(20),
                    AccountId = 1
                },
                new Message
                {
                    MessageId = 2,
                    ReplyToId = 0,
                    Content = "Hi",
                    CreatedOn = DateTime.Today - TimeSpan.FromMinutes(13),
                    AccountId = 2
                },
                new Message
                {
                    MessageId = 3,
                    ReplyToId = 1,
                    Content = "How can I help?",
                    CreatedOn = DateTime.Today - TimeSpan.FromMinutes(8),
                    AccountId = 3
                }
            );
            #endregion

        }
    }
}
