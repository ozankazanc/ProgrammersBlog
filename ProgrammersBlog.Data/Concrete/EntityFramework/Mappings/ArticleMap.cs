using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgrammersBlog.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProgrammersBlog.Data.Concrete.EntityFramework.Mappings
{
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(a=> a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();
            builder.Property(a => a.Title).HasMaxLength(100);
            builder.Property(a => a.Title).IsRequired(true);
            builder.Property(a => a.Content).IsRequired();
            builder.Property(a => a.Content).HasColumnType("NVARCHAR(MAX)");
            builder.Property(a => a.Date).IsRequired();
            builder.Property(a => a.SeoAuthor).IsRequired();
            builder.Property(a => a.SeoAuthor).HasMaxLength(100);
            builder.Property(a => a.SeoDescription).HasMaxLength(150);
            builder.Property(a => a.SeoDescription).IsRequired();
            builder.Property(a => a.SeoTags).HasMaxLength(70);
            builder.Property(a => a.ViewsCount).IsRequired();
            builder.Property(a => a.CommentCount).IsRequired();
            builder.Property(a => a.ViewsCount).IsRequired();
            builder.Property(a => a.Thumbnail).IsRequired();
            builder.Property(a => a.Thumbnail).HasMaxLength(250);
            
            builder.Property(a => a.CretedByName).IsRequired();
            builder.Property(a => a.CretedByName).HasMaxLength(50);
            builder.Property(a => a.ModifiedByName).IsRequired();
            builder.Property(a => a.ModifiedByName).HasMaxLength(50);
            builder.Property(a => a.CreatedDate).IsRequired();
            builder.Property(a => a.ModifiedDate).IsRequired();
            builder.Property(a => a.IsActive).IsRequired();
            builder.Property(a => a.IsDeleted).IsRequired();
            builder.Property(a => a.Note).HasMaxLength(500);

            builder.HasOne<Category>(a => a.Category).WithMany(c=>c.Articles).HasForeignKey(a=> a.CategoryId);
            builder.HasOne<User>(a => a.User).WithMany(c => c.Articles).HasForeignKey(a => a.UserId);

            builder.ToTable("Articles");
            //FLUENT API İLK BAŞTA DEĞERLER NULL GEÇİLEBİLİR OLSA DAHİ TABLOYA BİR DEĞER İSTER. BU YÜZDEN BÜTÜN TABLOLARDA DEĞER EKLİYORUZ.
            //Burda bir veri var mı? yoksa oluştur.

            #region
          /*  builder.HasData(
           new Article
           {
               Id = 1,
               CategoryId = 1,
               Title = "C# 9.0 ve .NET 5 Yenilikleri",
               Content = "Split Query Benim için en önemli özelliklerden biri Split Query " +
               "EF Core 3.0 sürümü ile karşılaştıralım.EF Core 3.0 sürümünde her bir LINQ sorgusu için tek " +
               "SQL sorgusu oluşturur ancak EF Core 5.0 sürümünde her bir LINQ sorgusu için birden fazla SQL sorgusu üretibilir.Peki birden fazla " +
               "SQL sorgusunun üretilmesi nasıl bir avantaj sağlayacak ?Ağır sorgularda özellikle Join kullandığımız sorgularda kaç tane Join " +
               "komutunuz varsa her biri farklı bir çekirdekte çalışmak isteyecektir.Kaynaklar o an yetersiz ise sorgunuzun response süresi " +
               "yükselecektir.Ancak tek bir SQL sorgusu yerine birden fazla SQL sorgusu üretip ayrı ayrı çalıştırdıktan sonra alınan cevapları" +
               " merge etmek şüphesiz ağır sorgularda avantajlı olacaktır." + "Basit bir Include işlemi ile üretilen SQL sorgularını karşılaştıralım.",
               Thumbnail = "Default.jpg",
               IsActive = true,
               IsDeleted = false,
               SeoDescription = "C# 9.0 ve .NET 5 Yenilikleri",
               SeoTags = "C#, C# 9.0, .NET 5",
               SeoAuthor = "Ozan Kazanç",
               UserId = 1,
               CretedByName = "InitialCreate",
               CreatedDate = DateTime.Now,
               ModifiedByName = "InitialCreate",
               ModifiedDate = DateTime.Now,
               Note = "C# 9.0 ve .NET 5 Yenilikleri",
               ViewsCount = 100,
               CommentCount = 1

           },
            new Article
            {
                Id = 2,
                CategoryId = 3,
                Title = "C++ 11 ve 19 Yenilikleri",
                Content = "Günümüz dünyasında pek çok şey artık belli standartlara göre yapılmaktadır. Peki nedir bu standart dediğimiz şey?" +
                " “Standartlar, ürünleri hizmetler ve üreticiler için kriterler belirleyen belgelenmiş ve genellikle gönüllü anlaşmalardır. " +
                "Standartlar, ürünlerin ve hizmetlerin amacına uygunluğunu, kıyaslanabilirliğini ve rekabet edebilirliğini sağlar.“[1] Örneğin" +
                " Türk Standartları Enstitüsü (TSE) tarafından “TS 5676” kodu ile 1988’de onaylanan bir standart, su kirliliği kontrolü ve" +
                " zehirlilik testleri ile ilgili çeşitli kuralları kapsamaktadır.[2] Bu ve bunun gibi pek çok standart, çeşitli işleri" +
                " yaparken karşımıza çıkmaktadır.",
                Thumbnail = "Default.jpg",
                IsActive = true,
                IsDeleted = false,
                SeoDescription = "C++ 11 ve 19 Yenilikleri",
                SeoTags = "C++, C++ 11,C++ 19",
                SeoAuthor = "Ozan Kazanç",
                UserId = 1,
                CretedByName = "InitialCreate",
                CreatedDate = DateTime.Now,
                ModifiedByName = "InitialCreate",
                ModifiedDate = DateTime.Now,
                Note = "C++ 11 ve 19 Yenilikleri",
                ViewsCount = 55,
                CommentCount = 1
            },
            new Article
            {
                Id = 3,
                CategoryId = 2,
                Title = "JavaScript Yenilikler",
                Content = "Son zamanlarda yoğun bir şekilde React projeleri yazıyorum.Bunların bir kısmı webpack ile" +
                "bir kısmı browserify ile paketlenen projeler," +
                "aralarındaki ortak ve keyifli nokta ise babel sayesinde javascript’in en son eklenen," +
                "daha tarayıcıların bile desteklemediği özelliklerini kullanabiliyor olmak.Javascript," +
                "her sene iteratif çıkacak olan özellikler sayesinde,o sıkıcı diyebileceğimiz halinden," +
                "yazması en keyifli dillerden biri haline geldi.Önerilen özelliklere ve durumlarına TC39’un Github sayfasından bakabilirsiniz.",
                Thumbnail = "Default.jpg",
                IsActive = true,
                IsDeleted = false,
                SeoDescription = "Jquery AJAX Json",
                SeoTags = "Jquery, AJAX, Json",
                SeoAuthor = "Ozan Kazanç",
                UserId = 1,
                CretedByName = "InitialCreate",
                CreatedDate = DateTime.Now,
                ModifiedByName = "InitialCreate",
                ModifiedDate = DateTime.Now,
                Note = "JavaScript Yenilikler",
                ViewsCount = 32,
                CommentCount = 1
            }
           ); */
            #endregion



        }
    }
}
