using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.MVC.Areas.Admin.Models;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Shared.Utilities.Extensions;
using ProgrammersBlog.Shared.Utilities.Results.CompexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProgrammersBlog.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAllbyNonDeletedAsync();
            return View(result.Data);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_CategoryAddPartial");
        }
        [HttpPost]
        public async Task<IActionResult> Add(CategoryAddDto categoryAddDto)
        {
            if(ModelState.IsValid)
            {
                var result = await _categoryService.AddAsync(categoryAddDto, "Ozan Kazanç");
                if(result.ResultStatus==ResultStatus.Success)
                {
                    var categoryAddAjaxModel = JsonSerializer.Serialize(
                        new CategoryAddAjaxViewModel 
                        {
                            CategoryDto=result.Data,
                            CategoryAddPartial=await this.RenderViewToStringAsync("_categoryAddPartial",categoryAddDto)
                        });
                    return Json(categoryAddAjaxModel);
                }
            }
            var categoryAddAjaxErrorModel = JsonSerializer.Serialize(
                        new CategoryAddAjaxViewModel
                        {
                            CategoryAddPartial = await this.RenderViewToStringAsync("_categoryAddPartial", categoryAddDto)
                            //Json mesajı içerisinde hata mesajı dönecek.
                        });
            return Json(categoryAddAjaxErrorModel);
        }

        public async Task<JsonResult> GetAllCategories()
        {
            var result = await _categoryService.GetAllbyNonDeletedAsync();
            
            var categories = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(categories);
        }
        [HttpPost]
        public async Task<JsonResult> Delete(int categoryId)
        {
            var result = await _categoryService.DeleteAsync(categoryId,"Ozan Kazanç");
            var deletedCategory = JsonSerializer.Serialize(result.Data);
            return Json(deletedCategory);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int categoryId)
        {
            var result = await _categoryService.GetCategoryUpdateDtoAsync(categoryId);
            if(result.ResultStatus==ResultStatus.Success)
            {
                return PartialView("_CategoryUpdatePartial", result.Data);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateDto categoryUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.UpdateAsync(categoryUpdateDto, "Ozan Kazanç");
                if (result.ResultStatus == ResultStatus.Success)
                {
                    var categoryUpdateAjaxModel = JsonSerializer.Serialize(
                        new CategoryUpdateAjaxViewModel
                        {
                            CategoryDto = result.Data,
                            CategoryUpdatePartial = await this.RenderViewToStringAsync("_categoryUpdatePartial", categoryUpdateDto)
                        });
                    return Json(categoryUpdateAjaxModel);
                }
            }
            var categoryUpdateAjaxErrorModel = JsonSerializer.Serialize(
                        new CategoryUpdateAjaxViewModel
                        {
                            CategoryUpdatePartial = await this.RenderViewToStringAsync("_categoryUpdatePartial", categoryUpdateDto)
                            //Json mesajı içerisinde hata mesajı dönecek.
                        });
            return Json(categoryUpdateAjaxErrorModel);
        }


    }
}
