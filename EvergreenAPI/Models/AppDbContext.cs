using System;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;

namespace EvergreenAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Thumbnail> Thumbnails { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<DiseaseCategory> DiseaseCategories { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<MedicineCategory> MedicineCategories { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<DetectionHistory> DetectionHistories { get; set; }
        public DbSet<DetectionAccuracy> DetectionAccuracies { get; set; }
        public DbSet<Image> Images{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region Thumbnail seed
            modelBuilder.Entity<Thumbnail>().HasData(
                new Thumbnail { ThumbnailId = 1, Url = "https://www.fao.org.vn/wp-content/uploads/2019/08/benh-vang-la-greening.jpg", AltText = "Hình ảnh bệnh vàng lá" },
                new Thumbnail { ThumbnailId = 2, Url = "https://i0.wp.com/trongraulamvuon.com/wp-content/uploads/2013/11/sau-duc-than-hong.jpg", AltText = "Hình ảnh bệnh sâu đục thân" },
                new Thumbnail { ThumbnailId = 3, Url = "https://hoadepviet.com/wp-content/uploads/2016/11/benh-hoa-hong-1.jpg", AltText = "Hình ảnh bệnh lá úa sớm" },
                new Thumbnail { ThumbnailId = 4, Url = "https://www.wondriumdaily.com/wp-content/uploads/2017/07/thumbnail-7.jpg", AltText = "Tomato thumbnail 1" },
                new Thumbnail { ThumbnailId = 5, Url = "https://www.skh.com/wp-content/uploads/2021/04/FF-peppers-tomatoes-onions-thumbnail.jpg", AltText = "Tomato thumbnail 2" },
                new Thumbnail { ThumbnailId = 6, Url = "https://sp.apolloboxassets.com/vendor/product/productImages/2022-09-15/6JBvcArray_13.jpg", AltText = "Tomato thumbnail 3" },
                new Thumbnail { ThumbnailId = 7, Url = "https://api-static.bacsicayxanh.vn/pictures/0001571_chaetomium_500.jpeg", AltText = "Chaetomium" },
                new Thumbnail { ThumbnailId = 8, Url = "https://www.basudin.com/storage/settings/May2021/basudin.png", AltText = "Basudin" },
                new Thumbnail { ThumbnailId = 9, Url = "https://phanthuocvisinh.com/wp-content/uploads/2021/12/AT-Vaccino-CAN-500ml.jpg", AltText = "AT Vaccino" },
                new Thumbnail { ThumbnailId = 10, Url = "https://mygarden.vn/wp-content/uploads/2020/11/tri-sau-an-la-2.jpg", AltText = "Bắt sâu bệnh" },
                new Thumbnail { ThumbnailId = 11, Url = "https://xuannong.vn/images/bo-tri-tren-cay-mai.jpg", AltText = "Lặt bỏ lá bị nhiễm bệnh" },
                new Thumbnail { ThumbnailId = 12, Url = "https://trongrauthuycanhtphcm.files.wordpress.com/2018/05/aphidadults-and-nymphs.jpg", AltText = "Lau cây bằng nước xà phòng hoặc rượu." },
                new Thumbnail { ThumbnailId = 13, Url = "https://baotayninh.vn/image/fckeditor/upload/2017/20170808/images/tieu%20huy_TP.JPG", AltText = "Tiêu hủy các cây bị nhiễm bệnh" },
                new Thumbnail { ThumbnailId = 14, Url = "http://www.congtyhai.com/Data/Sites/1/News/1348/hopsan-75ec_250ml_1618299173.png", AltText = "Hopsan 75EC" },
                new Thumbnail { ThumbnailId = 15, Url = "https://congnghesinhhocwao.vn/wp-content/uploads/2022/06/Xu-ly-triet-de-nguyen-nhan-gaybenh-7-1.jpg", AltText = "Cách phòng ngừa bệnh vàng lá" },
                new Thumbnail { ThumbnailId = 16, Url = "https://cdn.tgdd.vn/Files/2017/10/30/1037058/9-cong-dung-va-han-che-cua-ca-chua-doi-voi-cuoc-song-hang-ngay-202103142026330054.jpg", AltText = "Nâng cao năng suất cà chua" },
                new Thumbnail { ThumbnailId = 17, Url = "https://suckhoedoisong.qltns.mediacdn.vn/324455921873985536/2021/9/25/tac-dung-cua-ca-chua-doi-voi-suc-khoe-1-1632310636-831-width640height427-1632567723926-16325677242441321628137.jpg", AltText = "Cà chua và sức khoẻ" }
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
                    ThumbnailId = 4,
                },
                new Blog
                {
                    BlogId = 2,
                    Title = "Something different",
                    Description = "Lorem ipsum 2",
                    Content = "Lorem ipsum dolor sit amet 2",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(2),
                    ViewCount = 3,
                    ThumbnailId = 5
                },
                new Blog
                {
                    BlogId = 3,
                    Title = "Another blog",
                    Description = "Lorem ipsum 3",
                    Content = "Lorem ipsum dolor sit amet 3",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(10),
                    ViewCount = 30,
                    ThumbnailId = 6
                },
                new Blog
                {
                    BlogId = 4,
                    Title = "Cách phòng ngừa bệnh vàng lá",
                    Description = "Các biện pháp phòng ngừa đơn giản như sử dụng thuốc trừ sâu tự nhiên, bón phân đầy đủ dinh dưỡng và tưới nước đúng cách sẽ giúp duy trì sức khỏe cho cây trồng và tránh được bệnh vàng lá.",
                    Content = "Bệnh vàng lá là một trong những bệnh trên cây trồng gây ra nhiều thiệt hại và làm giảm năng suất đáng kể. Bệnh này được gây ra bởi một loại vi khuẩn và phổ biến trên nhiều loại cây trồng, bao gồm lúa, hoa màu, cà chua và dưa hấu.",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(5),
                    ViewCount = 60,
                    ThumbnailId = 15
                },
                new Blog
                {
                    BlogId = 5,
                    Title = "Nâng cao nâng suất cà chua",
                    Description = "Bài blog này sẽ giới thiệu các biện pháp đơn giản để nâng cao năng suất trồng cà chua. Các đề cập trong bài sẽ giúp độc giả hiểu rõ hơn về các yếu tố ảnh hưởng đến năng suất cà chua và cách tối ưu hóa chúng để đạt được hiệu suất tối đa trong vụ mùa.",
                    Content = "Để nâng cao năng suất cà chua, ta cần chọn giống phù hợp và tạo điều kiện sống tốt cho cây trồng bằng cách cung cấp đủ nước và dinh dưỡng, kiểm soát côn trùng và bệnh tật, tạo ra môi trường thích hợp cho việc phát triển cây chủ, chăm sóc cây đúng cách và thu hoạch đúng thời điểm. Việc làm này sẽ giúp trồng cà chua đạt hiệu suất tối đa và đảm bảo chất lượng sản phẩm.",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(55),
                    ViewCount = 144,
                    ThumbnailId = 16
                },
                new Blog
                {
                    BlogId = 6,
                    Title = "Công dụng và sức khoẻ",
                    Description = "Các đề cập trong bài sẽ trình bày các thành phần dinh dưỡng của cà chua và tác động của chúng đến sức khỏe, đồng thời cung cấp những lợi ích của việc tiêu thụ cà chua đối với các khía cạnh khác nhau của sức khoẻ, như sức khỏe tim mạch, tiêu hóa, thị lực và phòng chống ung thư.",
                    Content = "Cà chua là một loại thực phẩm giàu dinh dưỡng, chứa nhiều vitamin và khoáng chất cần thiết cho cơ thể. Việc tiêu thụ cà chua đều đặn có thể giúp cải thiện sức khỏe tim mạch, giảm nguy cơ mắc bệnh ung thư, cải thiện sức khỏe tiêu hóa, tăng cường hệ miễn dịch và giảm cân hiệu quả. Cà chua cũng có tác dụng bảo vệ mắt và cải thiện sức khỏe da. Vì vậy, thường xuyên ăn cà chua là cách tốt nhất để tăng cường sức khỏe và phòng ngừa bệnh tật.",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(1),
                    ViewCount = 120,
                    ThumbnailId = 17
                });

            #endregion

            #region DiseaseCategory seed
            modelBuilder.Entity<DiseaseCategory>().HasData(
                new DiseaseCategory
                {
                    DiseaseCategoryId = 1,
                    Name = "Loại 1"
                },
                new DiseaseCategory
                {
                    DiseaseCategoryId = 2,
                    Name = "Loại 2"
                },
                new DiseaseCategory
                {
                    DiseaseCategoryId = 3,
                    Name = "Loại 3"
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
                    ThumbnailId = 1
                },
                new Disease
                {
                    DiseaseId = 2,
                    Name = "Sâu đục thân",
                    Identification = "Các lỗ nhỏ trên vỏ cây",
                    Affect = "Làm cây chết khô",
                    DiseaseCategoryId = 2,
                    ThumbnailId = 2
                },
                new Disease
                {
                    DiseaseId = 3,
                    Name = "Đốm vòng (Úa sớm)",
                    Identification = "Vết bệnh xuất hiện đầu tiên ở các lá già phía dưới, lan dần lên các lá phía trên. Vết bệnh hình tròn hay có cạnh, màu nâu sẫm, có các vòng tròn đồng tâm màu đen, nhiều vết bệnh liên kết nhau thành vết lớn lan khắp lá, làm lá vàng khô và rụng sớm.",
                    Affect = "Sợi nấm và bào tử nấm lưu tồn trên tàn dư cây bệnh, cây ký chủ ít nhất là một năm, bào tử theo gió và côn trùng lan truyền gây bệnh trên cây trồng.",
                    DiseaseCategoryId = 1,
                    ThumbnailId = 3
                },
                new Disease
                {
                    DiseaseId = 4,
                    Name = "Héo rũ trên cây trồng",
                    Identification = "Lá trên cây héo ngày càng nhiều, vàng lá và rụng.",
                    Affect = "Phá hại các giai đoạn sinh trưởng của cây nhưng nặng nhất là vào cuối giai đoạn sinh trưởng.",
                    DiseaseCategoryId = 2,
                    ThumbnailId = 2
                },
                new Disease
                {
                    DiseaseId = 5,
                    Name = "Bạc lá",
                    Identification = " Xuất hiện nhiều đốm màu vàng rọng trên lá, cuối cùng chuyển sang màu nâu.",
                    Affect = "Làm giảm khả năng quang hợp, cây lúa sinh trưởng kém, nếu lá đòng cháy sẽ gây ra hiện tượng lép lửng, ảnh hưởng đến năng xuất và chất lượng gạo.",
                    DiseaseCategoryId = 2,
                    ThumbnailId = 3
                },
                new Disease
                {
                    DiseaseId = 6,
                    Name = "Thối rễ",
                    Identification = "Cây bị còi cọc, kiểm tra rễ cây có những vùng bị đen.",
                    Affect = "Sự vận chuyển của nước và chất dinh dưỡng từ rễ đến ngọn sẽ bị cản trở, hoặc bị mầm bệnh xâm nhập truyền khắp cây, khiến cây chết dần dần.",
                    DiseaseCategoryId = 2,
                    ThumbnailId = 1
                },
                new Disease
                {
                    DiseaseId = 7,
                    Name = "Khảm lá",
                    Identification = "Lá cây có những đốm màu vàng và những đường sọc.",
                    Affect = "Bệnh nặng cây còi cọc, đọt bị sượng, cây bị chùn lại, cây phát triển chậm, hoa bị vàng nhỏ và rụng, cây rất ít trái, trái nhỏ và vặn vẹo (dị dạng) và có vị đắng. Cuối cùng cây có thể bị chết.",
                    DiseaseCategoryId = 2,
                    ThumbnailId = 3
                },
                new Disease
                {
                    DiseaseId = 8,
                    Name = "Cháy lá",
                    Identification = "Đốm lá thay đổi thành màu nâu đen.",
                    Affect = "Trong vườn ươm, đây có thể là bệnh hại nghiêm trọng nhất vì chúng gây thiệt hại đến 40 – 50%. Trên cây lớn chúng gây chết lá, cành và rụng lá dẫn đến hiện tượng làm giảm năng suất",
                    DiseaseCategoryId = 2,
                    ThumbnailId = 1
                },
                new Disease
                {
                    DiseaseId = 9,
                    Name = "Đốm phấn",
                    Identification = "Xuất hiện nhiều vết nấm mốc trắng thường ở dưới lá.",
                    Affect = "Bệnh gây hại nặng ở giai đoạn cây trổ hoa đến mang trái khiến cây cho năng suất thấp và chất lượng trái kém, có thể khiến cây bị chết.",
                    DiseaseCategoryId = 2,
                    ThumbnailId = 2
                },
                new Disease
                {
                    DiseaseId = 10,
                    Name = "Rệp Aphid",
                    Identification = "Trên thân và lá cây sẽ có nhiều rệp xanh hoặc vàng nhỏ.",
                    Affect = "Rệp aphid trưởng thành, ấu trùng rệp gây hại trên các phần còn non của lan như lá, chồi và nụ hoa, chúng hút một lượng lớn chất dinh dưỡng từ nhựa cây, khiến cho cây thiếu dinh dưỡng; chất bài tiết của nó dễ gây nấm, đồng thời làm phát bệnh thối đen và truyền virus.",
                    DiseaseCategoryId = 2,
                    ThumbnailId = 3
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
                    MedicineCategoryId = 1,
                    ThumbnailId = 7,
                    DiseaseId = 2
                },
                new Medicine
                {
                    MedicineId = 2,
                    Name = "Basudin",
                    Uses = "Trừ sâu trong thân cây",
                    MedicineCategoryId = 2,
                    ThumbnailId = 8,
                    DiseaseId = 2
                },
                new Medicine
                {
                    MedicineId = 3,
                    Name = "AT Vaccino",
                    Uses = "Trị lá úa sớm",
                    MedicineCategoryId = 1,
                    ThumbnailId = 9,
                    DiseaseId = 3
                },
                new Medicine
                {
                    MedicineId = 4,
                    Name = "HOPSAN 75EC",
                    Uses = "Trừ sâu hại",
                    MedicineCategoryId = 1,
                    ThumbnailId = 14,
                    DiseaseId = 2

                }
            );

            #endregion

            #region Treatment seed
            modelBuilder.Entity<Treatment>().HasData(
                new Treatment { TreatmentId = 1, Method = "Bắt sâu", DiseaseId = 2, ThumbnailId = 10 },
                new Treatment { TreatmentId = 2, Method = "Lặt bỏ lá bị nhiễm bệnh", DiseaseId = 3, ThumbnailId = 11 },
                new Treatment { TreatmentId = 3, Method = "Lau cây bằng nước xà phòng hoặc rượu.", DiseaseId = 10, ThumbnailId = 12 }
            );
            #endregion
        }
    }
}
