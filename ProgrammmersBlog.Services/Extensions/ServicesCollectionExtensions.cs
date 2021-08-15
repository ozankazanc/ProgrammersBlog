using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProgrammersBlog.Data.Abstract;
using ProgrammersBlog.Data.Concrete;
using ProgrammersBlog.Data.Concrete.EntityFramework.Contexts;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Services.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProgrammersBlog.Services.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection LoadMyServices(this IServiceCollection serviceCollection,string connectionString)
        {
            serviceCollection.AddDbContext<ProgrammersBlogContext>(options => options.UseSqlServer(connectionString));

            //Altta girdiğimiz optiona ait propertyler default olarak 
            //normal bir şifre nasıl olması gerekiyorsa o zorluklar atanmıştır.
            //proje esnasında rrahat şifreler vermek adına değerleri değiştiriyoruz.
            serviceCollection.AddIdentity<User, Role>(options =>
            {
                //User password options
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

                //User name,mail ext. options
                options.User.AllowedUserNameCharacters= "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._@+";
                options.User.RequireUniqueEmail = true;


            }).AddEntityFrameworkStores<ProgrammersBlogContext>();

            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<ICategoryService, CategoryManager>();
            serviceCollection.AddScoped<IArticleService, ArticleManager>();
            serviceCollection.AddScoped<ICommentService, CommentManager>();

            return serviceCollection;
        }
    }
}
