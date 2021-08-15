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
            //Mvc ile �al��t�g�m�z� belirtmek i�in addcontrollerwithVivew()
            //Sayfa �al��t�g� esnada html taraf�nda yap�lan bir de�i�ikli�i sayfayay� tekrardan runtime etmeden g�rmek istiyorsak AddrazorRuntimecomplication() //bunun i�in nuget y�kledik.
            //Sayfalara g�nderilecek json modeller i�in yap�land�rma AddJsonOption()
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(opt=> 
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                // opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                //�stteki �ekilde yap�y� olu�turursak ajax ile d�nen modelde resultstatus de�erlerini kontrol etmek i�in elle "Success" girerek
                //kar��la�t�rmak gerekiyor. Ancak JsonStringEnumConvertera parametre vermez isek "success" i�in 0 "error" i�in 1 vererek kullanabiliyoruz.
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; 
                // mesela kategori s�n�f�n� �ekti�imizde ili�kili oldu�u makaleler s�n�f�da getirilmesi i�in kullan�yoruz.
                //ancak �uan bugl� ama bu y�zden controller i�erisinde kullan�caz.
            });
            services.AddSession();
            services.AddAutoMapper(typeof(CategoryProfile),typeof(ArticleProfile),typeof(UserProfile));
            services.LoadMyServices(connectionString:Configuration.GetConnectionString("LocalDB")); //jsondan connectionstringi �ekiyoruz.
            services.AddScoped<IImageHelper, ImageHelper>(); // servislere y�klemek gerekiyor.
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Admin/User/Login/");
                options.LogoutPath = new PathString("/Admin/User/Logout/");
                options.Cookie = new CookieBuilder
                {
                    Name = "ProgrammersBlog",
                    HttpOnly = true,//false olursa front end taraf�nda javascript ile cookie bilgilerine eri�ilebilir.
                    SameSite = SameSiteMode.Strict, // 
                    SecurePolicy = CookieSecurePolicy.SameAsRequest //Normalde kesinlikle always olmas� gerikiyor proje i�in bu de�eri verdik.
                };
                options.SlidingExpiration = true; //ki�i siteye cookie tan�mlama s�resi bitmeden girerse s�f�rl�yor ve yeniden s�re tan�ml�yor.
                options.ExpireTimeSpan = System.TimeSpan.FromDays(7); //verilen s�re
                options.AccessDeniedPath = new PathString("/Admin/User/AccessDenied");
                //normal bir kulla�c� login olup yetkisi olmayan bir sayfaya giri� yapmaya �al���rsa
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages(); // bulunmayan bir sayfa olursa 404 not found uyar�s� geliyor.
            }

            app.UseSession();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //Kullan�c� home indexe giderse farkl� bir sayfa, e�er admin home indexe giderse farkl� bir sayfa g�r�cek.
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
