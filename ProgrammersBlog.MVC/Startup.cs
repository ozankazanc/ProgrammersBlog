using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProgrammersBlog.MVC.AutoMapper.Profiles;
using ProgrammersBlog.MVC.Helpers.Abstract;
using ProgrammersBlog.MVC.Helpers.Concrete;
using ProgrammersBlog.Services.AutoMapper.Profiles;
using ProgrammersBlog.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProgrammersBlog.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            //Mvc ile çalýþtýgýmýzý belirtmek için addcontrollerwithVivew()
            //Sayfa çalýþtýgý esnada html tarafýnda yapýlan bir deðiþikliði sayfayayý tekrardan runtime etmeden görmek istiyorsak AddrazorRuntimecomplication() //bunun için nuget yükledik.
            //Sayfalara gönderilecek json modeller için yapýlandýrma AddJsonOption()
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(opt=> 
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                // opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                //Üstteki þekilde yapýyý oluþturursak ajax ile dönen modelde resultstatus deðerlerini kontrol etmek için elle "Success" girerek
                //karþýlaþtýrmak gerekiyor. Ancak JsonStringEnumConvertera parametre vermez isek "success" için 0 "error" için 1 vererek kullanabiliyoruz.
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; 
                // mesela kategori sýnýfýný çektiðimizde iliþkili olduðu makaleler sýnýfýda getirilmesi için kullanýyoruz.
                //ancak þuan buglý ama bu yüzden controller içerisinde kullanýcaz.
            });
            services.AddSession();
            services.AddAutoMapper(typeof(CategoryProfile),typeof(ArticleProfile),typeof(UserProfile));
            services.LoadMyServices(connectionString:Configuration.GetConnectionString("LocalDB")); //jsondan connectionstringi çekiyoruz.
            services.AddScoped<IImageHelper, ImageHelper>(); // servislere yüklemek gerekiyor.
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Admin/User/Login/");
                options.LogoutPath = new PathString("/Admin/User/Logout/");
                options.Cookie = new CookieBuilder
                {
                    Name = "ProgrammersBlog",
                    HttpOnly = true,//false olursa front end tarafýnda javascript ile cookie bilgilerine eriþilebilir.
                    SameSite = SameSiteMode.Strict, // 
                    SecurePolicy = CookieSecurePolicy.SameAsRequest //Normalde kesinlikle always olmasý gerikiyor proje için bu deðeri verdik.
                };
                options.SlidingExpiration = true; //kiþi siteye cookie tanýmlama süresi bitmeden girerse sýfýrlýyor ve yeniden süre tanýmlýyor.
                options.ExpireTimeSpan = System.TimeSpan.FromDays(7); //verilen süre
                options.AccessDeniedPath = new PathString("/Admin/User/AccessDenied");
                //normal bir kullaýcý login olup yetkisi olmayan bir sayfaya giriþ yapmaya çalýþýrsa
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages(); // bulunmayan bir sayfa olursa 404 not found uyarýsý geliyor.
            }

            app.UseSession();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //Kullanýcý home indexe giderse farklý bir sayfa, eðer admin home indexe giderse farklý bir sayfa görücek.
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
                    );
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
