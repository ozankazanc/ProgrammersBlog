using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.MVC.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammersBlog.MVC.Areas.Admin.ViewComponents
{
    public class AdminMenuViewComponent :ViewComponent
    {
        /// <summary>
        /// Bu sayfa içerisinde layoutta bulunan menüyü rollere göre özelleştirme işlemi yapıyoruz.
        /// Shared altında Component klasörü ve onun altında da AdminMenu Klasörü oluşturup, Default adında bir sayfa oluşturduktan sonra
        /// Menuyu bu sayfa içerisine atıyoruz. 
        /// Default sayfası içerisinde ise bir if bloğu ile kullanıcının rolünü kontrol edebiliyoruz.
        /// </summary>
        private readonly UserManager<User> _userManager;
        public AdminMenuViewComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public ViewViewComponentResult Invoke()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result; // login olmuş kullanıcıyı getiriyor.
            var roles = _userManager.GetRolesAsync(user).Result;
            return View(new UserWithRolesViewModel
            {
                User = user,
                Roles = roles
            });
        }
    }
}
