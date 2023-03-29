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
                    Title = "The Harmful Effects of Aphids",
                    Description = "Aphids are small, sap-sucking insects that can cause significant damage to plants. They reproduce quickly, and their numbers can grow rapidly, leading to significant damage to crops and ornamental plants. In this blog post, we will explore the harmful effects of aphids on plants and discuss how to prevent and control their infestation. ",
                    Content = "Damage Caused by Aphids\r\nAphids feed on the sap of plants, which can cause several issues. First, they weaken the plant, making it more susceptible to disease and other pests. " +
                    "Second, aphids excrete a sticky, sugary substance called honeydew, which can attract other insects, such as ants, and lead to the growth of sooty mold. Finally, aphids can transmit plant viruses from plant to plant, causing significant damage to crops.\r\n\r\nStunted Growth\r\nOne of the most noticeable effects of an aphid infestation is stunted plant growth. The sap-sucking insects remove vital nutrients from the plant, leading to stunted growth and wilting. In severe cases, the plant may even die.\r\n\r\nDeformed Leaves\r\nAphids can cause significant damage to leaves, leading to deformities and discoloration. They may curl or twist leaves, causing them to become distorted and unsightly. This damage can make it more difficult for the plant to carry out photosynthesis and produce food, leading to further weakening of the plant.\r\n\r\nSpread of Diseases\r\nAphids can transmit plant viruses from plant to plant, causing significant damage to crops. For example, the potato aphid can transmit the potato leaf roll virus, which can cause significant yield loss in potato crops. Aphids can also transmit viruses to ornamental plants, causing significant damage to gardens and landscapes.\r\n\r\nHoneydew and Sooty Mold\r\nAphids excrete a sugary substance called honeydew, which can attract other insects, such as ants, and lead to the growth of sooty mold. Sooty mold is a black, powdery substance that can grow on leaves and stems, reducing the plant's ability to carry out photosynthesis and produce food.\r\n\r\nPrevention and Control\r\nPreventing and controlling aphid infestations is essential to ensure healthy plants and crops. Here are some tips on how to prevent and control aphids:\r\n\r\nRegular Inspection\r\nInspecting plants regularly for aphids and other pests is an essential part of pest management. Catching an infestation early can help prevent significant damage and make control easier.\r\n\r\nCultural Controls\r\nCultural controls are practices that can help prevent and control aphid infestations. These include practices such as crop rotation, proper irrigation, and pruning. Maintaining healthy plants can help prevent aphid infestations.\r\n\r\nNatural Enemies\r\nMany natural enemies, such as ladybugs and lacewings, feed on aphids and can help control infestations. Attracting these natural enemies to your garden can help prevent and control aphid infestations.\r\n\r\nChemical Controls\r\nChemical controls, such as insecticides, can be used to control aphid infestations. However, it is essential to use these products responsibly and follow all label instructions carefully. Overuse of insecticides can lead to the development of resistance in aphids and other pests.\r\n\r\nConclusion\r\nAphids can cause significant damage to plants and crops, leading to stunted growth, deformed leaves, and the spread of diseases. Preventing and controlling aphid infestations is essential to ensure healthy plants and crops. Regular inspection, cultural controls, attracting natural enemies, and using chemical controls responsibly are all effective ways to prevent and control aphid infestations. By taking these steps, gardeners and farmers can protect their plants and ensure healthy yields",
                    LastModifiedDate = DateTime.Today,
                    ViewCount = 10,
                    ThumbnailId = 4,
                },
                new Blog
                {
                    BlogId = 2,
                    Title = "The Many Uses of Tomatoes",
                    Description = "Tomatoes are a versatile and nutritious fruit that can be used in a variety of ways. They are a great source of vitamins and minerals, including vitamin C, potassium, and lycopene, a powerful antioxidant. Here are some of the many uses of tomatoes:",
                    Content = "Cooking\r\nTomatoes are a staple in many cuisines around the world. They can be used in a variety of dishes, from salads and soups to pasta sauces and stews. Some popular tomato-based dishes include:\r\n\r\nPizza: Tomatoes are used as the base for pizza sauce, providing a tangy and slightly sweet flavor.\r\nGazpacho: This Spanish soup is made with tomatoes, cucumbers, peppers, and onions, and is served chilled.\r\nCaprese salad: This Italian salad features sliced tomatoes, fresh mozzarella, and basil, drizzled with olive oil and balsamic vinegar.\r\nChili: Tomatoes are a key ingredient in many chili recipes, providing a rich and slightly sweet flavor.\r\nBeauty\r\nTomatoes are not just good for eating - they can also be used to improve the health of your skin and hair. Here are a few ways you can incorporate tomatoes into your beauty routine:\r\n\r\nFace mask: Tomatoes contain vitamin C and antioxidants that can help brighten and tighten your skin. To make a tomato face mask, mash up a ripe tomato and apply it to your face for 15-20 minutes before rinsing off.\r\nHair treatment: The acidity in tomatoes can help remove buildup from your scalp and make your hair shinier. To use tomatoes as a hair treatment, blend a few tomatoes in a blender and apply the mixture to your hair and scalp. Leave on for 30 minutes before rinsing off.\r\nBody scrub: Tomatoes can also be used as an exfoliating body scrub. Mix together a mashed tomato with sugar or salt and use it to scrub your skin in the shower.\r\nGardening\r\nTomatoes are easy to grow and can be grown in a variety of environments, from outdoor gardens to indoor containers. Here are a few tips for growing your own tomatoes:\r\n\r\nChoose a sunny spot: Tomatoes need at least 6 hours of sunlight per day to thrive.\r\nPlant in well-draining soil: Tomatoes prefer soil that drains well and is rich in organic matter.\r\nWater regularly: Tomatoes need consistent watering, especially during hot and dry weather.\r\nSupport the plants: Tomatoes can grow quite tall and need support, such as stakes or cages, to keep them from falling over.\r\nHealth\r\nTomatoes are packed with nutrients that can benefit your health in a variety of ways. Here are a few of the health benefits of tomatoes:\r\n\r\nHeart health: The lycopene in tomatoes has been shown to reduce the risk of heart disease by reducing inflammation and cholesterol levels.\r\nCancer prevention: Lycopene has also been shown to reduce the risk of certain types of cancer, including prostate, lung, and stomach cancer.\r\nSkin health: The vitamin C in tomatoes can help improve the health of your skin by boosting collagen production and reducing inflammation.\r\nEye health: Tomatoes contain lutein and zeaxanthin, two antioxidants that are important for eye health and may help reduce the risk of age-related macular degeneration.\r\nConclusion\r\nTomatoes are a versatile and nutritious fruit that can be used in a variety of ways, from cooking to beauty to gardening. They are packed with vitamins and minerals that can benefit your health in many ways. So next time you're at the grocery store, be sure to pick up some tomatoes and try incorporating them into your routine in a new and creative way!",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(2),
                    ViewCount = 3,
                    ThumbnailId = 5
                },
                new Blog
                {
                    BlogId = 3,
                    Title = "Cultivating Land for Crops: A Guide to Getting Started",
                    Description = "Cultivating land for crops can be a rewarding and fulfilling endeavor, whether you're looking to start a small garden for personal use or grow crops on a larger scale for commercial purposes. However, it can also be a daunting task, especially if you're new to farming or gardening. In this guide, we'll cover some of the basics of cultivating land for crops, from preparing the soil to selecting the right crops.",
                    Content = "Step 1: Choose Your Plot of Land\r\nThe first step in cultivating land for crops is to choose the right plot of land. The ideal plot of land will have fertile soil, good drainage, and access to sunlight. It's also important to consider factors such as the size of the plot, its location, and the amount of time and effort you're willing to invest in it.\r\n\r\nStep 2: Prepare the Soil\r\nOnce you've chosen your plot of land, the next step is to prepare the soil. This involves removing any weeds or other unwanted plants, loosening the soil to improve drainage, and adding organic matter such as compost or manure to enrich the soil.\r\n\r\nStep 3: Select Your Crops\r\nOnce your soil is prepared, it's time to select the crops you want to grow. This will depend on a variety of factors, including your climate, soil type, and personal preferences. Some popular crops for beginners include tomatoes, peppers, lettuce, and herbs.\r\n\r\nStep 4: Plant Your Crops\r\nAfter you've selected your crops, it's time to plant them. Be sure to follow the instructions on the seed packets or plant labels, as different crops have different requirements for planting depth, spacing, and watering.\r\n\r\nStep 5: Maintain Your Crops\r\nOnce your crops are planted, it's important to maintain them properly. This includes watering them regularly, fertilizing them as needed, and monitoring them for pests and diseases.\r\n\r\nStep 6: Harvest Your Crops\r\nThe final step in cultivating land for crops is to harvest your crops. This can be done as soon as the fruits or vegetables are ripe, and it's important to do so in a timely manner to prevent them from rotting or spoiling.\r\n\r\nConclusion\r\nCultivating land for crops can be a fun and rewarding activity, whether you're growing a small garden for personal use or growing crops on a larger scale for commercial purposes. By following these basic steps, you can get started on your journey to growing your own fresh, healthy produce.",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(10),
                    ViewCount = 30,
                    ThumbnailId = 6
                },
                new Blog
                {
                    BlogId = 4,
                    Title = "How to Prevent Tomato Yellow Leaf Disease",
                    Description = "Tomatoes are a popular garden crop for many people, but they can be vulnerable to diseases that can ruin your harvest. One of the most common tomato diseases is the tomato yellow leaf disease, which can cause yellowing and wilting of the leaves, reduced fruit production, and even plant death. Fortunately, there are several measures you can take to prevent tomato yellow leaf disease and ensure a healthy tomato crop. In this blog post, we will discuss some of the best ways to prevent tomato yellow leaf disease.",
                    Content = "Choose Resistant Tomato Varieties\r\nOne of the easiest ways to prevent tomato yellow leaf disease is to choose tomato varieties that are resistant to the disease. Many tomato varieties have been bred to be resistant to tomato yellow leaf disease, as well as other common tomato diseases. Look for varieties labeled as \"TYLCV-resistant\" or \"TY-resistant,\" which means they are resistant to tomato yellow leaf disease. By choosing resistant varieties, you can greatly reduce the risk of your tomato plants getting infected with the disease.\r\n\r\nPractice Good Crop Rotation\r\nAnother effective way to prevent tomato yellow leaf disease is to practice good crop rotation. Tomato yellow leaf disease is caused by a virus that can survive in the soil for several years. If you plant tomatoes in the same spot year after year, the virus can build up in the soil, increasing the risk of your plants getting infected. To prevent this, rotate your tomato crops every year, planting them in a different location in your garden. This will help reduce the risk of your tomato plants getting infected with tomato yellow leaf disease.\r\n\r\nKeep Your Garden Clean\r\nKeeping your garden clean is another important way to prevent tomato yellow leaf disease. The virus that causes the disease can be carried by insects, such as whiteflies, which can spread the virus from infected plants to healthy ones. To reduce the risk of this happening, keep your garden free of debris, weeds, and other plant material that can harbor insects. Keep your garden clean and tidy, and remove any diseased plants or plant material immediately.\r\n\r\nUse Insecticidal Soap\r\nInsecticidal soap can also be effective in preventing tomato yellow leaf disease. Insecticidal soap is a natural and safe way to control insects, such as whiteflies, that can spread the virus that causes the disease. To use insecticidal soap, mix it according to the instructions on the label and apply it to your tomato plants as needed. Insecticidal soap can be a very effective way to prevent tomato yellow leaf disease, as well as other tomato diseases.\r\n\r\nUse Mulch\r\nUsing mulch around your tomato plants can also help prevent tomato yellow leaf disease. Mulch helps retain moisture in the soil, which can help keep your tomato plants healthy and reduce stress. Additionally, mulch can help prevent soil-borne diseases, such as tomato yellow leaf disease, by creating a barrier between the soil and the plants. Use a layer of organic mulch, such as straw or shredded leaves, around your tomato plants to help prevent tomato yellow leaf disease.\r\n\r\nConclusion\r\nTomato yellow leaf disease can be a frustrating and devastating problem for tomato growers, but there are several effective ways to prevent it. By choosing resistant tomato varieties, practicing good crop rotation, keeping your garden clean, using insecticidal soap, and using mulch, you can greatly reduce the risk of your tomato plants getting infected with tomato yellow leaf disease. With these measures in place, you can enjoy a healthy and abundant tomato crop all season long.",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(5),
                    ViewCount = 60,
                    ThumbnailId = 15
                },
                new Blog
                {
                    BlogId = 5,
                    Title = "Improving Tomato Yield: Tips and Strategies",
                    Description = "Tomatoes are one of the most widely grown vegetables in the world, with a production of more than 182 million tonnes globally in 2020. Despite this high level of production, farmers and gardeners are always looking for ways to improve tomato yield. In this blog post, we will discuss some tips and strategies for increasing tomato yield.",
                    Content = "Choosing the Right Variety\r\nChoosing the right variety of tomato is the first step towards improving yield. Different varieties have different growth habits, resistance to disease, and yield potential. When selecting a tomato variety, consider the following factors:\r\n\r\nClimate: Choose a variety that is well-suited to your climate. Some tomatoes are more heat-tolerant, while others prefer cooler temperatures.\r\nGrowth habit: Indeterminate tomatoes grow and produce fruit continuously throughout the season, while determinate tomatoes produce fruit all at once and then stop. Choose a variety that suits your growing conditions and harvesting preferences.\r\nDisease resistance: Look for varieties that are resistant to common tomato diseases in your area, such as fusarium wilt, verticillium wilt, and tomato mosaic virus.\r\nYield potential: Some tomato varieties are bred specifically for high yields. Look for varieties with a track record of high yields.\r\nSoil Preparation\r\nTomatoes thrive in well-drained, fertile soil. Prepare your soil by tilling or digging to a depth of at least 12 inches, removing rocks and other debris. Add compost or aged manure to the soil to improve its nutrient content and water-holding capacity. Soil pH should be between 6.0 and 7.0 for optimal growth and fruit production.\r\n\r\nProper Planting Techniques\r\nProper planting techniques can help increase tomato yield. Here are some tips to keep in mind:\r\n\r\nPlant tomatoes in a sunny location: Tomatoes need at least six to eight hours of direct sunlight each day.\r\nPlant at the right time: Tomatoes are warm-season plants and should be planted after the last frost in the spring. In warmer regions, they can be planted in the fall for a second harvest.\r\nPlant deep: Plant tomato seedlings deep, burying the stem up to the first set of leaves. This encourages root growth and a stronger plant.\r\nSpace plants properly: Tomatoes should be spaced at least 18 to 24 inches apart to allow for good air circulation and to prevent the spread of disease.\r\nMulch: Mulching around tomato plants helps to retain moisture, suppress weeds, and regulate soil temperature.\r\nWatering and Fertilizing\r\nProper watering and fertilizing can help increase tomato yield. Here are some tips to keep in mind:\r\n\r\nWater regularly: Tomatoes need consistent moisture to produce fruit. Water deeply once or twice a week, depending on rainfall and temperature.\r\nFertilize regularly: Tomatoes are heavy feeders and require regular fertilization to produce high yields. Use a balanced fertilizer with equal amounts of nitrogen, phosphorus, and potassium. Fertilize every three to four weeks throughout the growing season.\r\nAvoid over-fertilization: Too much fertilizer can result in excessive foliage growth at the expense of fruit production.\r\nPest and Disease Control\r\nPest and disease control are important factors in increasing tomato yield. Here are some tips to keep in mind:\r\n\r\nMonitor regularly: Keep an eye on your tomato plants for signs of pests and disease. Early detection is key to preventing damage.\r\nUse natural pest control methods: Consider using natural methods, such as handpicking pests or using beneficial insects like ladybugs and praying mantises to control pest populations.\r\nPractice crop rotation: Avoid planting tomatoes in the same location year after year to prevent the buildup of soil-borne diseases.\r\nUse fungicides: If disease is detected, use fungicides according",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(55),
                    ViewCount = 144,
                    ThumbnailId = 16
                },
                new Blog
                {
                    BlogId = 6,
                    Title = "Septoria Prevention: How to Keep Your Plants Healthy",
                    Description = "Septoria is a fungal disease that affects many types of plants, including tomatoes, potatoes, and peppers. It can cause serious damage to crops, leading to reduced yields and even crop loss. Fortunately, there are steps you can take to prevent Septoria from affecting your plants. In this blog post, we will discuss the key methods for preventing Septoria in your garden.",
                    Content = "Plant Resistant Varieties\r\nOne of the most effective ways to prevent Septoria is to plant resistant varieties of crops. Many plant breeders have developed varieties that are resistant to Septoria, which can greatly reduce the risk of infection. When selecting seeds or plants, look for those that are labeled as resistant to Septoria. This will give you a good starting point for keeping your plants healthy.\r\n\r\nProper Plant Spacing\r\nAnother important factor in preventing Septoria is proper plant spacing. Crowded plants can create a humid environment that is ideal for the growth of fungal diseases like Septoria. By spacing your plants properly, you can ensure good air circulation, which will help to prevent fungal growth. Be sure to follow the spacing recommendations for each type of plant you are growing.\r\n\r\nCrop Rotation\r\nCrop rotation is another important method for preventing Septoria. This involves planting different crops in the same area each year. By rotating your crops, you can prevent the buildup of pathogens in the soil that can lead to fungal diseases like Septoria. Be sure to avoid planting crops from the same family in the same area for at least three years.\r\n\r\nMulching\r\nMulching is another effective method for preventing Septoria. By adding a layer of organic material like straw, leaves, or grass clippings around your plants, you can help to keep the soil moist and prevent the growth of fungal spores. Be sure to avoid using infected plant debris in your mulch, as this can spread the disease.\r\n\r\nWater Management\r\nProper water management is also important for preventing Septoria. Overwatering can create a humid environment that is ideal for fungal growth, so be sure to water your plants only when necessary. Avoid watering from above, as this can spread fungal spores. Instead, water at the base of the plant.\r\n\r\nFungicides\r\nIf you do experience an outbreak of Septoria, you may need to use fungicides to control the disease. Fungicides are chemical treatments that can kill or inhibit the growth of fungal spores. There are many different types of fungicides available, including both organic and synthetic options. Be sure to follow the instructions carefully when using fungicides, as they can be harmful to both plants and humans if not used correctly.\r\n\r\nConclusion\r\nSeptoria can be a serious problem for gardeners, but by taking the right preventative measures, you can keep your plants healthy and productive. By planting resistant varieties, spacing your plants properly, rotating your crops, mulching, managing water, and using fungicides as needed, you can prevent Septoria from taking hold in your garden. With a little effort and attention, you can enjoy healthy and productive plants year after year.",
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
                    Name = "Diseases Caused By Weather"
                },
                new DiseaseCategory
                {
                    DiseaseCategoryId = 2,
                    Name = "Diseases Caused By Pests"
                },
                new DiseaseCategory
                {
                    DiseaseCategoryId = 3,
                    Name = "Diseases Caused By Viruses"
                },
                new DiseaseCategory
                {
                    DiseaseCategoryId = 4,
                    Name = "Healthy Leaf"
                }

                );
            #endregion

            #region MedicineCategory seed

            modelBuilder.Entity<MedicineCategory>().HasData(
              new MedicineCategory { MedicineCategoryId = 1, Name = "Pesticide" },
              new MedicineCategory { MedicineCategoryId = 2, Name = "Fertilizer" },
              new MedicineCategory { MedicineCategoryId = 3, Name = "Fungicide" },
              new MedicineCategory { MedicineCategoryId = 4, Name = "Healthy Leaf" }
            );

            #endregion

            #region Disease seed

            modelBuilder.Entity<Disease>().HasData(
                new Disease
                {
                    DiseaseId = 1,
                    Name = "Early Blight",
                    Identification = "Early blight is identified by the appearance of a few (5 to 10 in most cases) circular brown spots on a leaf. " +
                    "The spots are up to a half-inch in diameter, with concentric rings or ridges that form a target-like pattern surrounded by a yellow halo." +
                    " As the disease progresses, spots merge together and may kill the whole leaf. Over time, the stem and fruit may also be infected, showing dark and sunken spots." +
                    " Cankers with a similar dark and sunken target-like appearance are often found at or above the soil line on the stem",
                    
                    
                    Affect = "Under favorable conditions (e.g., warm weather with short or abundant dews), significant defoliation of lower leaves may occur, leading to sunscald of the fruit." +
                    " As the disease progresses, symptoms may migrate to the plant stem and fruit. Stem lesions are dark, slightly sunken and concentric in shape. " +
                    "Seedlings can develop small, dark, partially sunken lesions which grow and elongate into circular or oblong lesions. " +
                    "Basal girdling and death of seedlings may occur, a symptom known as collar rot. " +
                    "It brings significant damage to tomato leaves, stems, and fruits almost yearly in West Virginia. " +
                    "Early blight can also affect potato foliage.",
                    DiseaseCategoryId = 3,
                    ThumbnailId = 1,
                    MedicineId = 1,
                    TreatmentId = 1
                },
                new Disease
                {
                    DiseaseId = 2,
                    Name = "Septoria",
                    Identification = "The first symptoms appear as small, water-soaked, circular spots 1/16 to 1/8\" in diameter on the undersides of older leaves. " +
                    "The centers of these spots then turn gray to tan and have a dark-brown margin. The spots are distinctively circular and are often quite numerous. " +
                    "As the spots age, they sometimes enlarge and often coalesce. A diagnostic feature of this disease is the presence of many dark-brown, pimple-like structures called pycnidia (fruiting bodies of the fungus) that are readily visible in the tan centers of the spots. When spots are numerous, affected leaves turn yellow and eventually shrivel up, brown, and drop off. " +
                    "Defoliation usually starts on the oldest leaves and can quickly spread progressively up the plant toward the new growth. Significant losses can result from early leaf-drop and often leads to the subsequent sunscalding of the fruit when plants are prematurely defoliated.",
                    
                    
                    Affect = "If untreated, Septoria leaf spot will cause the leaves to turn yellow and eventually to dry out and fall off. " +
                    "This will weaken the plant, send it into decline, and cause sun scalding of the unprotected, exposed tomatoes. " +
                    "Without leaves, the plant will not continue producing and maturing tomatoes. " +
                    "Septoria leaf spot spreads rapidly.",
                    DiseaseCategoryId = 2,
                    ThumbnailId = 2,
                    MedicineId = 2,
                    TreatmentId = 2
                },
                new Disease
                {
                    DiseaseId = 3,
                    Name = "Yellow Curl",
                    Identification = "The most obvious symptoms in tomato plants are small leaves that become yellow between the veins. " +
                    "The leaves also curl upwards and towards the middle of the leaf. Plants are stunted or dwarfed. New growth only produced after infection is reduced in size. Leaflets are rolled upwards and inwards. " +
                    "Leaves are often bent downwards, stiff, thicker than normal, have a leathery texture, show interveinal chlorosis and are wrinkled. " +
                    "Young leaves are slightly chlorotic (yellowish)",

                    Affect = "Tomato yellow leaf curl can infect over 30 different kinds of plants, but it is mainly known to cause devastating losses of up to 100 per cent in the yield of tomatoes. Both field and glasshouse grown tomatoes are susceptible." +
                    "Tomato yellow leaf curl can infect over 30 different kinds of plants, but it is mainly known to cause devastating losses of up to 100 per cent in the yield of tomatoes. " +
                    "Both field and glasshouse grown tomatoes are susceptible. In seedlings, the shoots become shortened and give the young plants a bushy appearance. In mature plants only new growths produced after infection is reduced in size.",
                    DiseaseCategoryId = 1,
                    ThumbnailId = 3,
                    MedicineId = 3,
                    TreatmentId = 3
                },
                new Disease
                {
                    DiseaseId = 4,
                    Name = "Healthy Leaf",
                    Identification = "Leaves are green evenly, not bent down, no ring spots, yellow spots appear. Notice that the leaves on the tomato plant are 10 inches long on an average stem." +
                    "Small leaflets are three inches long. " +
                    "Look at the leaves and notice the serrated, or wavy and pointed, edging along the entire leaf.",
                    DiseaseCategoryId = 4,
                    ThumbnailId = 4,
                    MedicineId = 5,
                    TreatmentId = 5
                },
                new Disease
                {
                    DiseaseId = 5,
                    Name = "Aphid Bugs",

                    Identification = "Aphids are tiny, soft-bodied insects usually found on the underside of leaves and feeding on new, soft growth. " +
                    "They can be green, pink, purple, gray, or black in color. When squished, the bodies usually release a similarly colored pigment. " +
                    "Aphids are usually found in colonies producing honeydew.",

                    Affect = "Aphids remove sap from the plant with their piercing-sucking mouthparts. Tomato plants can tolerate large numbers of aphids without suffering yield loss. " +
                    "However, severe infestations can cause leaves to curl and may stunt plants. " +
                    "Decreased leaf area can increase sun scald to the fruit.",
                    DiseaseCategoryId = 2,
                    ThumbnailId = 5,
                    MedicineId = 4,
                    TreatmentId = 4
                }
                
            );

            #endregion

            #region Medicine seed

            modelBuilder.Entity<Medicine>().HasData(
                new Medicine
                {
                    MedicineId = 1,
                    Name = "Bonide Liquid Copper Fungicide Concentrate",

                    Uses = "Control common plant diseases in your lawn and home garden with Captain Jack’s Ready-to-Spray Liquid Copper Fungicide. " +
                    "Approved for organic gardening, Copper Fungicide can be used on a variety of listed fruits, vegetables, nuts, herbs and flowers, and can even be applied up to the day of harvest. " +
                    "Captain Jack’s Copper Fungicide is effective against common fungal diseases including downy mildew, black rot, leaf spot, powdery mildew, blight, peach leaf curl and more. " +
                    "This product arrives conveniently ready-to-mix. For best results, mix according to label and apply using a plant sprayer. Apply thoroughly to the tops and undersides of leaves and all plant surfaces on affected plants. " +
                    "Repeat every 7-10 days as needed. Please see product label for full use instructions.",
                    MedicineCategoryId = 2,
                    ThumbnailId = 7,
                    


                },
                new Medicine
                {
                    MedicineId = 2,
                    Name = "Manzate Pro Stick T&O",

                    Uses = "Step 1: Determine how much Manzate Pro Stick T&O you need by first calculating the square footage of the area you will be treating. To do this, measure (in feet) and multiply the area length times the width (length x width = square footage). " +
                    "Depending on the disease, you will use 4 to 8 oz. of Manzate Pro-Stick T&O per 3-5 gallons of water per 1,000 sq. ft. of turfgrass. For ornamentals, the rate is 1 to 2 lbs. of product per 5 gallons of water per acre or 1 to 2 lbs. of product per 100 gallons of water." +
                    "\r\nStep 2: In a sprayer of your choice, add half the amount of water needed for the application, followed by the appropriate amount of Manzate Pro-Stick T&O based on your calculations. Fill with the remaining half of water and agitate well to ensure the product is properly mixed." +
                    "\r\nStep 3: Spray the fungicide onto the leaf of the plant, placing the sprayer on a fan spray setting for even coverage. Repeat applications can be made at 10 to 21-day intervals if necessary.",
                    MedicineCategoryId = 3,
                    ThumbnailId = 8,
                    

                },
                new Medicine
                {
                    MedicineId = 3,
                    Name = "pH Dat",

                    Uses = "- Use in case the soil pH fluctuates below the required level and harms plants." +
                    "\r\n- Use in the stage of soil preparation, before flowering and after harvest." +
                    "\r\n- Industrial plants, fruit trees: Mix 1 liter with 200 liters of water, water 2-4 liters for 1 root." +
                    "\r\n- Rice, vegetables: 1 liter/1000m2, dilute and water evenly on the soil.",
                    MedicineCategoryId = 2,
                    ThumbnailId = 9,
                    

                },
                new Medicine
                {
                    MedicineId = 4,
                    Name = "Ametox (Abamectin 1.8%)",

                    Uses = "Abamectin is a mixture of avermectins containing about 80% avermectin B1a and 20% avermectin B1b. " +
                    "These two components B1a and B1b have very similar biological and toxicological properties. " +
                    "The avermectins are insecticidal/miticidal compounds derived from the soil bacterium Streptomyces avermitilis. " +
                    "Abamectin is a natural fermentation product of this bacterium. It acts as an insecticide by affecting the nervous system of and paralyzing insects." +
                    " Abamectin is used to control insect and mite pests of citrus pear and nut tree crops and it is used by homeowners for control of fire ants.",
                    MedicineCategoryId = 1,
                    ThumbnailId = 14,
                
                },
                new Medicine
                {
                    MedicineId = 5,
                    Name = "Healthy Leaf",
                    Uses = "None",
                    MedicineCategoryId = 4,
                    ThumbnailId = 14,

                }
            );

            #endregion

            #region Treatment seed
            modelBuilder.Entity<Treatment>().HasData(
                new Treatment { TreatmentId = 1, 
                TreatmentName = "Remove Infected Leaves and  Soil Improvement", 

                Method = "Remove infected leaves during the growing season and remove all " +
                "infected plant parts at the end of the season. Prune or stake plants to improve air circulation and reduce fungal problems." +
                "\r\nMake sure to disinfect your pruning shears (one part bleach to 4 parts water) after each cut." +
                "Drip irrigation and soaker hoses can be used to help keep the foliage dry.\r\nFor best control, apply copper-based fungicides early," +
                " two weeks before disease normally appears or when weather forecasts predict a long period of wet weather. " +
                "Alternatively, begin treatment when disease first appears, and repeat every 7-10 days for as long as needed.", 
                ThumbnailId = 10,
                    
                },
                new Treatment { TreatmentId = 2, 
                TreatmentName = "Pulling out weeds",

                Method = "Eliminate initial source of infection by removing infected plant debris and weeds, and use disease-free seeds." +
                "\r\nIf complete removal of plant debris is not possible, destroy by deep plowing immediately after harvest and follow with a one-year rotation" +
                " with non-solanaceous crop.",
                ThumbnailId = 11,
                    
                },
                new Treatment { TreatmentId = 3,
                TreatmentName = "Add Magnesium for Plants",

                Method = "Tomatoes that don’t have enough magnesium will develop yellow leaves with green veins. If you’re sure of a magnesium deficiency," +
                " try a homemade Epsom salt mixture. Combine two tablespoons of Epsom salt with a gallon of water and spray the mixture on the plant.", 
                ThumbnailId = 12,
                    
                },
                new Treatment
                {
                    TreatmentId = 4,
                    TreatmentName = "Neem oil or Insecticidal soap",

                    Method = "Use natural sprays like neem oil or insecticidal soap to wash away aphids." +
                    "\r\n- Add two tablespoons of castile soap into one gallon of water, then spray aphids directly." +
                    "\r\n- Add two teaspoons of neem oil to one gallon of water, then spray aphids directly." +
                    "\r\nBe sure to cover both the top and bottom of leaves when spraying aphids. This will help ensure that you get all the aphids on your plants." +
                    "\r\nApply yellow stick boards around your tomato plants to attract aphids. Aphids are attracted to yellow and will travel towards it, then stick inside its glue coating.",
                    ThumbnailId = 12,

                },
                 new Treatment
                 {
                     TreatmentId = 5,
                     TreatmentName = "Healthy Leaf",
                     Method = "None",
                     ThumbnailId = 12,

                 }

            );
            #endregion


        }
    }
}
