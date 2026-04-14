using Microsoft.EntityFrameworkCore;
using PartsCom.Domain.Entities;

namespace PartsCom.Infrastructure.Database;

internal static class DataSeeder
{
    public static async Task SeedAsync(PartsComDbContext context)
    {
        await SeedProductsAsync(context);
        await SeedNewsAsync(context);
    }

    private static async Task SeedProductsAsync(PartsComDbContext context)
    {
        if (await context.Products.AnyAsync())
        {
            return;
        }

        // Create categories
        List<ProductCategory> categories =
        [
            ProductCategory.Create("Procesory"),
            ProductCategory.Create("Karty Graficzne"),
            ProductCategory.Create("Płyty Główne"),
            ProductCategory.Create("Pamięć RAM"),
            ProductCategory.Create("Dyski SSD"),
            ProductCategory.Create("Zasilacze"),
            ProductCategory.Create("Obudowy"),
            ProductCategory.Create("Chłodzenie"),
            ProductCategory.Create("Monitory"),
            ProductCategory.Create("Peryferia"),
            ProductCategory.Create("Laptopy"),
            ProductCategory.Create("Smartfony"),
            ProductCategory.Create("Smartwatche"),
            ProductCategory.Create("Słuchawki"),
            ProductCategory.Create("Telewizory")
        ];

        await context.ProductCategories.AddRangeAsync(categories);
        await context.SaveChangesAsync();

        // Create tags
        var appleTag = ProductTag.Create("Apple");
        var laptopTag = ProductTag.Create("Laptop");
        var proTag = ProductTag.Create("Pro");
        var gamingTag = ProductTag.Create("Gaming");
        var intelTag = ProductTag.Create("Intel");
        var amdTag = ProductTag.Create("AMD");
        var nvidiaTag = ProductTag.Create("Nvidia");

        await context.ProductTags.AddRangeAsync(appleTag, laptopTag, proTag, gamingTag, intelTag, amdTag, nvidiaTag);
        await context.SaveChangesAsync();

        List<Product> products = [];

        // === PC Components ===

        // CPU
        ProductCategory cpuCat = categories.First(c => c.Name == "Procesory");
        products.Add(CreateProduct("Intel Core i9-14900K", "Flagowy procesor Intel 14. generacji, idealny do gier i zadań profesjonalnych. 24 rdzenie, 32 wątki.", 2899.00m, 50, cpuCat.Id, "https://images.unsplash.com/photo-1591799264318-7e840043d49c?q=80&w=2940&auto=format&fit=crop"));
        products.Add(CreateProduct("AMD Ryzen 7 7800X3D", "Król gamingu z technologią 3D V-Cache. Niesamowita wydajność w grach przy niskim poborze energii.", 1899.00m, 75, cpuCat.Id, "https://images.unsplash.com/photo-1555616635-640960031058?q=80&w=2874&auto=format&fit=crop"));
        products.Add(CreateProduct("Intel Core i5-13600K", "Najlepszy procesor w stosunku cena/wydajność. Świetny wybór do nowoczesnego komputera do gier.", 1399.00m, 120, cpuCat.Id, "https://plus.unsplash.com/premium_photo-1664194583959-1e353782b13e?q=80&w=2940&auto=format&fit=crop"));

        // GPU
        ProductCategory gpuCat = categories.First(c => c.Name == "Karty Graficzne");
        products.Add(CreateProduct("NVIDIA GeForce RTX 4090", "Absolutna bestia wydajności. Graj w 4K z włączonym Ray Tracingiem bez kompromisów.", 9200.00m, 10, gpuCat.Id, "https://images.unsplash.com/photo-1591488320449-011701bb6704?q=80&w=2940&auto=format&fit=crop"));
        products.Add(CreateProduct("AMD Radeon RX 7900 XTX", "Topowa karta graficzna od AMD. Ogromna moc obliczeniowa i 24GB VRAM w konkurencyjnej cenie.", 4800.00m, 25, gpuCat.Id, "https://images.unsplash.com/photo-1624705002806-5d72df96c312?q=80&w=2888&auto=format&fit=crop"));
        products.Add(CreateProduct("NVIDIA GeForce RTX 4070 Super", "Idealny balans między ceną a wydajnością. Świetna do grania w 1440p.", 3100.00m, 40, gpuCat.Id, "https://images.unsplash.com/photo-1555680202-c86f0e12f086?q=80&w=2940&auto=format&fit=crop"));
        products.Add(CreateProduct("NVIDIA GeForce RTX 4060 Ti", "Dobra wydajność w 1080p i 1440p w przystępnej cenie.", 1899.00m, 60, gpuCat.Id, "https://images.unsplash.com/photo-1555680202-c86f0e12f086?q=80&w=2940&auto=format&fit=crop"));

        // Motherboard
        ProductCategory moboCat = categories.First(c => c.Name == "Płyty Główne");
        products.Add(CreateProduct("ASUS ROG MAXIMUS Z790 HERO", "Płyta główna dla entuzjastów. Potężna sekcja zasilania, wsparcie dla PCIe 5.0 i DDR5.", 2999.00m, 15, moboCat.Id, "https://images.unsplash.com/photo-1518770660439-4636190af475?q=80&w=2940&auto=format&fit=crop"));
        products.Add(CreateProduct("MSI MAG B650 TOMAHAWK WIFI", "Solidna podstawa pod procesory Ryzen 7000. Dobre chłodzenie i bogate wyposażenie.", 999.00m, 60, moboCat.Id, "https://images.unsplash.com/photo-1563207153-f403bf289096?q=80&w=2940&auto=format&fit=crop"));

        // RAM
        ProductCategory ramCat = categories.First(c => c.Name == "Pamięć RAM");
        products.Add(CreateProduct("G.SKILL Trident Z5 RGB 32GB (2x16GB) 6000MHz", "Ekstremalnie szybka pamięć DDR5 z efektownym podświetleniem RGB.", 650.00m, 100, ramCat.Id, "https://images.unsplash.com/photo-1562976540-1502c2145186?q=80&w=2831&auto=format&fit=crop"));
        products.Add(CreateProduct("Kingston FURY Beast 32GB (2x16GB) 3200MHz DDR4", "Niezawodna pamięć DDR4 dla starszych platform, zapewniająca stabilność i wydajność.", 320.00m, 150, ramCat.Id, "https://images.unsplash.com/photo-1541029071515-84cc54f84dc5?q=80&w=2940&auto=format&fit=crop"));

        // SSD
        ProductCategory ssdCat = categories.First(c => c.Name == "Dyski SSD");
        products.Add(CreateProduct("Samsung 990 PRO 2TB", "Jeden z najszybszych dysków SSD na rynku. Prędkości odczytu do 7450 MB/s.", 899.00m, 80, ssdCat.Id, "https://images.unsplash.com/photo-1523473827530-9b6732d0dfa6?q=80&w=2940&auto=format&fit=crop"));
        products.Add(CreateProduct("Lexar NM790 4TB", "Ogromna pojemność w świetnej cenie. Idealny na gry i duże pliki.", 1100.00m, 40, ssdCat.Id, "https://images.unsplash.com/photo-1597872252165-482c12408906?q=80&w=2940&auto=format&fit=crop"));

        // Cooling
        ProductCategory coolingCat = categories.First(c => c.Name == "Chłodzenie");
        products.Add(CreateProduct("Arctic Liquid Freezer III 360", "Najwydajniejsze chłodzenie wodne AIO na rynku. Ciche i skuteczne.", 450.00m, 55, coolingCat.Id, "https://images.unsplash.com/photo-1587202372775-e229f172b9d7?q=80&w=2787&auto=format&fit=crop"));

        // Monitors
        ProductCategory monitorCat = categories.First(c => c.Name == "Monitory");
        products.Add(CreateProduct("Dell Alienware AW3423DWF", "Monitor QD-OLED, który zmienia zasady gry. Nieskończony kontrast i żywe kolory.", 3800.00m, 20, monitorCat.Id, "https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?q=80&w=2940&auto=format&fit=crop"));

        // Peripherals
        ProductCategory periphCat = categories.First(c => c.Name == "Peryferia");
        products.Add(CreateProduct("Logitech G Pro X Superlight 2", "Ultralekka mysz bezprzewodowa dla profesjonalistów. Precyzja i szybkość.", 649.00m, 100, periphCat.Id, "https://images.unsplash.com/photo-1615663245857-ac93bb7c39e7?q=80&w=2865&auto=format&fit=crop"));
        products.Add(CreateProduct("Keychron Q1 Pro", "Mechaniczna klawiatura customowa z możliwością konfiguracji. Aluminiowa obudowa.", 899.00m, 30, periphCat.Id, "https://images.unsplash.com/photo-1587829741301-dc798b91add1?q=80&w=2938&auto=format&fit=crop"));

        // === Apple & Consumer Electronics ===

        // Laptops
        ProductCategory laptopCat = categories.First(c => c.Name == "Laptopy");
        var macbookPro = Product.Create(
            name: "Apple MacBook Pro 14\" M3 Pro (2023)",
            description: @"Najnowszy MacBook Pro z chipem M3 Pro. Fenomenalna wydajność, wspaniały wyświetlacz Liquid Retina XDR i całodniowa bateria. Idealny dla profesjonalistów i twórców.",
            price: 8999m,
            stockQuantity: 50,
            productCategoryId: laptopCat.Id,
            productSubCategoryId: null
        );
        macbookPro.SetMainImage("https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/mbp-spacegray-select-202206?wid=904&hei=840&fmt=jpeg&qlt=90&.v=1664497359481");
        macbookPro.UpdateRating(4.7, 21671);
        products.Add(macbookPro);

        // Smartphones
        ProductCategory smartphoneCat = categories.First(c => c.Name == "Smartfony");
        var iphone = Product.Create(
            name: "Apple iPhone 15 Pro Max 256GB - Natural Titanium",
            description: "iPhone 15 Pro Max. Wykonany z tytanu, wyposażony w przełomowy chip A17 Pro, konfigurowalny przycisk Akcji oraz najpotężniejszy system aparatów w historii iPhone'a.",
            price: 6499m,
            stockQuantity: 100,
            productCategoryId: smartphoneCat.Id,
            productSubCategoryId: null
        );
        iphone.SetMainImage("https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone-15-pro-finish-select-202309-6-7inch-naturaltitanium?wid=5120&hei=2880&fmt=p-jpg&qlt=80&.v=1692845702708");
        iphone.UpdateRating(4.8, 15420);
        products.Add(iphone);

        // Headphones
        ProductCategory headphonesCat = categories.First(c => c.Name == "Słuchawki");
        var airpods = Product.Create(
            name: "Apple AirPods Pro (2. generacja) z etui MagSafe",
            description: "AirPods Pro oferują do 2x lepszą aktywną redukcję szumów, Adaptive Transparency oraz spersonalizowane Spatial Audio z dynamicznym śledzeniem ruchu głowy.",
            price: 1249m,
            stockQuantity: 200,
            productCategoryId: headphonesCat.Id,
            productSubCategoryId: null
        );
        airpods.SetMainImage("https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/MQD83?wid=1144&hei=1144&fmt=jpeg&qlt=90&.v=1660803972361");
        airpods.UpdateRating(4.6, 8932);
        products.Add(airpods);

        // Smartwatches
        ProductCategory smartwatchCat = categories.First(c => c.Name == "Smartwatche");
        var appleWatch = Product.Create(
            name: "Apple Watch Ultra 2 GPS + Cellular 49mm Titanium",
            description: "Najbardziej wytrzymały i wszechstronny Apple Watch. Stworzony z myślą o wytrzymałości, eksploracji i przygodzie.",
            price: 4299m,
            stockQuantity: 75,
            productCategoryId: smartwatchCat.Id,
            productSubCategoryId: null
        );
        appleWatch.SetMainImage("https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/MRSF3ref_VW_34FR+watch-49-titanium-702_VW_34FR_WF_CO?wid=750&hei=712&fmt=p-jpg&qlt=95&.v=1694507712045");
        appleWatch.UpdateRating(4.9, 5621);
        products.Add(appleWatch);

        // TVs
        ProductCategory tvCat = categories.First(c => c.Name == "Telewizory");
        var samsungTV = Product.Create(
            name: "Samsung 65\" OLED S95C 4K Smart TV (2023)",
            description: "Doświadcz genialnej jakości obrazu dzięki technologii Samsung OLED. Samoświecące piksele zapewniają idealną czerń i żywe kolory.",
            price: 9999m,
            stockQuantity: 25,
            productCategoryId: tvCat.Id,
            productSubCategoryId: null
        );
        samsungTV.SetMainImage("https://image-us.samsung.com/SamsungUS/home/television-home-theater/tvs/oled-tv/01102023/QN65S95CAFXZA_003_L-Perspective_Titan-Black-Gallery.jpg?$product-details-jpg$");
        samsungTV.UpdateRating(4.5, 1245);
        products.Add(samsungTV);

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        // Add product images for MacBook
        await context.ProductImages.AddRangeAsync(
            ProductImage.Create(macbookPro.Id, "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/mbp-spacegray-select-202206?wid=904&hei=840&fmt=jpeg&qlt=90&.v=1664497359481", 0),
            ProductImage.Create(macbookPro.Id, "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/mbp14-spacegray-gallery1-202301?wid=4000&hei=3072&fmt=jpeg&qlt=90&.v=1670630089680", 1),
            ProductImage.Create(macbookPro.Id, "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/mbp14-spacegray-gallery2-202301?wid=4000&hei=3072&fmt=jpeg&qlt=90&.v=1670630091498", 2)
        );

        // Add tags to products
        await context.ProductProductTags.AddRangeAsync(
            ProductProductTag.Create(macbookPro.Id, appleTag.Id),
            ProductProductTag.Create(macbookPro.Id, laptopTag.Id),
            ProductProductTag.Create(macbookPro.Id, proTag.Id),
            ProductProductTag.Create(iphone.Id, appleTag.Id),
            ProductProductTag.Create(airpods.Id, appleTag.Id),
            ProductProductTag.Create(appleWatch.Id, appleTag.Id)
        );

        await context.SaveChangesAsync();
    }

    private static async Task SeedNewsAsync(PartsComDbContext context)
    {
        if (await context.NewsPosts.AnyAsync())
        {
            return;
        }

        List<NewsPost> news =
        [
            NewsPost.Create("Premiera procesorów Intel Core Ultra", "Intel prezentuje nową generację procesorów mobilnych i desktopowych z zaawansowanym NPU do obsługi AI.", "https://images.unsplash.com/photo-1591799264318-7e840043d49c?q=80&w=2940&auto=format&fit=crop", "Marek"),
            NewsPost.Create("NVIDIA RTX 5090 - co wiemy?", "Przecieki na temat nowej generacji kart graficznych Blackwell. Czy wydajność skoczy o 50%?", "https://images.unsplash.com/photo-1591488320449-011701bb6704?q=80&w=2940&auto=format&fit=crop", "Admin"),
            NewsPost.Create("Jak wybrać idealną klawiaturę?", "Przewodnik po przełącznikach mechanicznych i rozmiarach klawiatur. Red, Blue czy Brown?", "https://images.unsplash.com/photo-1587829741301-dc798b91add1?q=80&w=2938&auto=format&fit=crop", "TechGuru"),
            NewsPost.Create("Apple prezentuje iPhone 16", "Nowa generacja iPhone'ów z ulepszonym aparatem i wydajniejszym chipem A18.", "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone-15-pro-finish-select-202309-6-7inch-naturaltitanium?wid=5120&hei=2880&fmt=p-jpg&qlt=80&.v=1692845702708", "TechNews")
        ];

        await context.NewsPosts.AddRangeAsync(news);
        await context.SaveChangesAsync();
    }

    private static Product CreateProduct(string name, string description, decimal price, int stock, Guid categoryId, string imageUrl)
    {
        var product = Product.Create(name, description, price, stock, categoryId, null);
        product.SetMainImage(imageUrl);
        return product;
    }
}
