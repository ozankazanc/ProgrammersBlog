using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.MVC.Areas.Admin.Models;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.CompexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammersBlog.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;
        public HomeController(ICategoryService categoryService, IArticleService articleService, ICommentService commentService, UserManager<User> userManager)
        {
            _categoryService = categoryService;
            _articleService = articleService;
            _commentService = commentService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var categoriesCount = await _categoryService.CountByNonDeletedAsync();
            var articlesCount = await _articleService.CountByNonDeletedAsync();
            var commentCount = await _commentService.CountByNonDeletedAsync();
            var userCount = await _userManager.Users.CountAsync();
            var articles = await _articleService.GetAllAsync();
            
            if (categoriesCount.ResultStatus == ResultStatus.Success
                &&articlesCount.ResultStatus==ResultStatus.Success
                && articles.ResultStatus == ResultStatus.Success
                && commentCount.ResultStatus == ResultStatus.Success
                && userCount > -1 && articles.ResultStatus == ResultStatus.Success)
            {
                return View(new DashboardViewModel
                {
                    CategoriesCount=categoriesCount.Data,
                    ArticlesCount=articlesCount.Data,
                    CommentCount=commentCount.Data,
                    UsersCount=userCount,
                    Articles=articles.Data
                });
            }
            return NotFound();
        }
    }
}
