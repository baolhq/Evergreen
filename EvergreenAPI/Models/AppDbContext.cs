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
        public DbSet<Treatment> Treatments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Images seed
            modelBuilder.Entity<Image>().HasData(
                new Image { ImageId = 1, Url = "https://www.fao.org.vn/wp-content/uploads/2019/08/benh-vang-la-greening.jpg", AltText = "Hình ảnh bệnh vàng lá" },
                new Image { ImageId = 2, Url = "https://i0.wp.com/trongraulamvuon.com/wp-content/uploads/2013/11/sau-duc-than-hong.jpg", AltText = "Hình ảnh bệnh sâu đục thân" },
                new Image { ImageId = 3, Url = "https://hoadepviet.com/wp-content/uploads/2016/11/benh-hoa-hong-1.jpg", AltText = "Hình ảnh bệnh lá úa sớm" },
                new Image { ImageId = 4, Url = "https://www.wondriumdaily.com/wp-content/uploads/2017/07/thumbnail-7.jpg", AltText = "Tomato thumbnail 1" },
                new Image { ImageId = 5, Url = "https://www.skh.com/wp-content/uploads/2021/04/FF-peppers-tomatoes-onions-thumbnail.jpg", AltText = "Tomato thumbnail 2" },
                new Image { ImageId = 6, Url = "https://sp.apolloboxassets.com/vendor/product/productImages/2022-09-15/6JBvcArray_13.jpg", AltText = "Tomato thumbnail 3" },
                new Image { ImageId = 7, Url = "https://api-static.bacsicayxanh.vn/pictures/0001571_chaetomium_500.jpeg", AltText = "Chaetomium" },
                new Image { ImageId = 8, Url = "https://www.basudin.com/storage/settings/May2021/basudin.png", AltText = "Basudin" },
                new Image { ImageId = 9, Url = "https://phanthuocvisinh.com/wp-content/uploads/2021/12/AT-Vaccino-CAN-500ml.jpg", AltText = "AT Vaccino" },
                new Image { ImageId = 10, Url = "https://mygarden.vn/wp-content/uploads/2020/11/tri-sau-an-la-2.jpg", AltText = "Bắt sâu bệnh" },
                new Image { ImageId = 11, Url = "https://xuannong.vn/images/bo-tri-tren-cay-mai.jpg", AltText = "Lặt bỏ lá bị nhiễm bệnh" }
            );
            #endregion

            #region Blog seed

            modelBuilder.Entity<Blog>().HasData(
                new Blog
                {
                    BlogId = 1,
                    Title = "Hello world with C#",
                    Description = "Lorem ipsum",
                    Content = "Lorem ipsum dolor sit amet",
                    LastModifiedDate = DateTime.Today,
                    ViewCount = 10,
                    ImageId = 4,
                },
                new Blog
                {
                    BlogId = 2,
                    Title = "Something different",
                    Description = "Lorem ipsum 2",
                    Content = "Lorem ipsum dolor sit amet 2",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(2),
                    ViewCount = 3,
                    ImageId = 5
                },
                new Blog
                {
                    BlogId = 3,
                    Title = "Another blog",
                    Description = "Lorem ipsum 3",
                    Content = "Lorem ipsum dolor sit amet 3",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(10),
                    ViewCount = 30,
                    ImageId = 6
                });

            #endregion

            #region Account seed

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    AccountId = 1,
                    Username = "admin",
                    Password = "Admin@",
                    Role = "Admin",
                    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwibmJmIjoxNjY3NjUwNjM2LCJleHAiOjE2Njc2NTI0MzYsImlhdCI6MTY2NzY1MDYzNiwiaXNzIjoiYmFvbGhxLmdpdGh1Yi5jb20iLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MDAxIn0.r2GcLny8UrBAOwCA8FcPj1hY3Zdq69IdfHjebWlFqDs"
                },
                new Account
                {
                    AccountId = 2,
                    Username = "test01",
                    Password = "123123",
                    Role = "User",
                    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3QwMSIsInJvbGUiOiJVc2VyIiwibmJmIjoxNjY3NjUxMDI4LCJleHAiOjE2Njc2NTI4MjgsImlhdCI6MTY2NzY1MTAyOCwiaXNzIjoiYmFvbGhxLmdpdGh1Yi5jb20iLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MDAxIn0.AWCcGJhaKRot6ZKxyFPUM-uIry3nR91_-0_834EtZ_o"
                },
                new Account
                {
                    AccountId = 3,
                    Username = "pbc",
                    Password = "121212",
                    Role = "Professor",
                    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InBiYyIsInJvbGUiOiJQcm9mZXNzb3IiLCJuYmYiOjE2Njc2OTE2OTcsImV4cCI6MTY2NzY5MzQ5NywiaWF0IjoxNjY3NjkxNjk3LCJpc3MiOiJiYW9saHEuZ2l0aHViLmNvbSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEifQ.gBY9L51QgnFnrHjyS_wKxq6dLVZsU2dzFQqKiwGcYEs"
                }
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
                    MedicineId = 7,
                    Name = "Chaetumium",
                    Uses = "Trị bệnh vàng lá",
                    MedicineCategoryId = 1,
                    ImageId = 7
                },
                new Medicine
                {
                    MedicineId = 8,
                    Name = "Basudin",
                    Uses = "Trừ sâu trong thân cây",
                    MedicineCategoryId = 2,
                    ImageId = 8
                },
                new Medicine
                {
                    MedicineId = 9,
                    Name = "AT Vaccino",
                    Uses = "Trị lá úa sớm",
                    MedicineCategoryId = 1,
                    ImageId = 9
                }
            );

            #endregion

            #region Treatment seed
            modelBuilder.Entity<Treatment>().HasData(
                new Treatment { TreatmentId = 1, Method = "Bắt sâu", DiseaseId = 2, ImageId = 10 },
                new Treatment { TreatmentId = 2, Method = "Lặt bỏ lá bị nhiễm bệnh", DiseaseId = 3, ImageId = 11 }
            );
            #endregion
        }
    }
}
