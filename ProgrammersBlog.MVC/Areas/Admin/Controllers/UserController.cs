using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.MVC.Areas.Admin.Models;
using ProgrammersBlog.MVC.Helpers.Abstract;
using ProgrammersBlog.Shared.Utilities.Extensions;
using ProgrammersBlog.Shared.Utilities.Results.CompexTypes;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProgrammersBlog.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IImageHelper _imageHelper;

        public UserController(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, IImageHelper imageHelper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _imageHelper = imageHelper;
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(new UserListDto { 
                User = users,
                ResultStatus=ResultStatus.Success
            });

            

        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View("UserLogin");
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
                if (user!=null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, userLoginDto.Password, userLoginDto.RememberMe, false); //false değeri yanlış değer 5 kere girerse hesap kitleniyor.
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "E-Mail adresiniz ve ya şifreniz yanlıştır.");
                        return View("UserLogin");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "E-Mail adresiniz ve ya şifreniz yanlıştır.");
                    return View("UserLogin");
                }
            }
            else
            {
                return View("UserLogin");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userListDto = JsonSerializer.Serialize(new UserListDto
            {
                User = users,
                ResultStatus = ResultStatus.Success
            },new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(userListDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_userAddPartial");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            if(ModelState.IsValid)
            {
                var uploadedImageDtoResult = await _imageHelper.UploadUserImage(userAddDto.UserName,userAddDto.PictureFile);
                userAddDto.Picture = uploadedImageDtoResult.ResultStatus == ResultStatus.Success ? uploadedImageDtoResult.Data.FullName : "userImages/defaultUser.png";
                var user = _mapper.Map<User>(userAddDto);
                var result = await _userManager.CreateAsync(user, userAddDto.Password);
                if(result.Succeeded)
                {
                    var userAddAjaxModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserDto = new UserDto
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"{user.UserName} adlı kullanıcı başarıyla eklenmiştir.",
                            User = user
                        },
                        UserAddPartial = await this.RenderViewToStringAsync("_userAddPartial", userAddDto)
                        
                    });
                    return Json(userAddAjaxModel);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    var userAddAjaxErrorModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserAddDto= userAddDto,
                        UserAddPartial=await this.RenderViewToStringAsync("_userAddPartial",userAddDto)
                    });
                    return Json(userAddAjaxErrorModel);
                }
            }
            else
            {
                //hata modeldde olursa
                var userAddAjaxErrorModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                {
                    UserAddDto = userAddDto,
                    UserAddPartial = await this.RenderViewToStringAsync("_userAddPartial", userAddDto)
                });
                return Json(userAddAjaxErrorModel);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> Delete (int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            
            var result = await _userManager.DeleteAsync(user);
            if(result.Succeeded)
            {
                var deletedUser = JsonSerializer.Serialize(new UserDto
                {
                    User=user,
                    ResultStatus = ResultStatus.Success,
                    Message = $"{user.UserName} adlı kullanıcı başarıyla silinmiştir."
                });
                return Json(deletedUser);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var errors in result.Errors)
                {
                    errorMessages += $"*{errors.Description}\n";
                }
                var deletedUserErrorModel = JsonSerializer.Serialize(new UserDto
                {
                    ResultStatus=ResultStatus.Error,
                    Message = $"{user.UserName} adlı kullanıcıyı silerken hata oluştu.",
                    User=user
                });
                return Json(deletedUserErrorModel);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<PartialViewResult> Update(int userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var userUpdateDto = _mapper.Map<UserUpdateDto>(user);

            return PartialView("_UserUpdatePartial", userUpdateDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            if(ModelState.IsValid)
            {
                bool isNewPictureUploaded = false;
                var olduser = await _userManager.Users.FirstOrDefaultAsync(x=>x.Id==userUpdateDto.Id);
                var oldUserPicture = olduser.Picture;
                if (userUpdateDto.PictureFile!=null)
                {
                    var uploadedImageDtoResult = await _imageHelper.UploadUserImage(userUpdateDto.UserName, userUpdateDto.PictureFile);
                    userUpdateDto.Picture = uploadedImageDtoResult.ResultStatus == ResultStatus.Success ? uploadedImageDtoResult.Data.FullName : "userImages/defaultUser.png";
                    if (oldUserPicture != "userImages/DefaultUser.png")
                    {
                        isNewPictureUploaded = true; // veritabanına resim gidip güncellendi mi?
                    }
                }
                
                var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, olduser);
                var result = await _userManager.UpdateAsync(updatedUser);
                
                if(result.Succeeded)
                {
                    if(isNewPictureUploaded)
                    {
                        _imageHelper.Delete(oldUserPicture);
                    }
                    var updateAjaxModal = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserDto = new UserDto
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"{updatedUser.UserName} adlı kullanıcı başarıyla güncellenmiştir.",
                            User = updatedUser
                        },
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                    });
                    return Json(updateAjaxModal);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    var updateErrorAjaxModal = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserUpdateDto=userUpdateDto,
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                    });
                    return Json(updateErrorAjaxModal);
                }

            }
            else
            {
                var updateErrorModelStateAjaxModal = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                {
                    UserUpdateDto = userUpdateDto,
                    UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                });
                return Json(updateErrorModelStateAjaxModal);
            }
        }

        [Authorize(Roles = "Admin,Editor")]
        
        [HttpGet]
        public ViewResult AccessDenied()
        {
            return View(); 
        }

        [Authorize]
        [HttpGet]
        public async Task<ViewResult> ChangeDetails()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var updateDto = _mapper.Map<UserUpdateDto>(user);
            return View(updateDto);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeDetails(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                bool isNewPictureUploaded = false;
                var olduser = await _userManager.GetUserAsync(HttpContext.User);
                var oldUserPicture = olduser.Picture;
                if (userUpdateDto.PictureFile != null)
                {
                    var uploadedImageDtoResult = await _imageHelper.UploadUserImage(userUpdateDto.UserName, userUpdateDto.PictureFile);
                    userUpdateDto.Picture = uploadedImageDtoResult.ResultStatus == ResultStatus.Success ? uploadedImageDtoResult.Data.FullName : "userImages/defaultUser.png";
                    
                    if (oldUserPicture!= "userImages/DefaultUser.png")
                    {
                        isNewPictureUploaded = true; // veritabanına resim gidip güncellendi mi?
                    }
                   
                }

                var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, olduser);
                var result = await _userManager.UpdateAsync(updatedUser);

                if (result.Succeeded)
                {
                    if (isNewPictureUploaded)
                    {
                        _imageHelper.Delete(oldUserPicture);
                    }
                    TempData.Add("SuccessMessage", "Bilgileniriniz başarıyla güncellenmiştir.");
                    return View(userUpdateDto);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(userUpdateDto); //eğer bir hata olursa validation summary kısmında hatalar gözükecek.
                }
            }
            else
            {
                return View(userUpdateDto); //eğer bir hata olursa validation summary kısmında hatalar gözükecek.
            }
        }

        [Authorize]
        [HttpGet]
        public ViewResult PasswordChange()
        {
            return View();
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PasswordChange(UserPasswordChangeDto userPasswordChangeDto)
        {
           if(ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var isVerified = await _userManager.CheckPasswordAsync(user, userPasswordChangeDto.CurrentPassword);

                if (isVerified)
                {
                    var result = await _userManager.ChangePasswordAsync(user, userPasswordChangeDto.CurrentPassword, userPasswordChangeDto.NewPassword);
                    if (result.Succeeded)
                    {
                        await _userManager.UpdateSecurityStampAsync(user); // önemli bir değişiklik yaptgımızda kullanıyoruz.
                        await _signInManager.SignOutAsync();
                        await _signInManager.PasswordSignInAsync(user, userPasswordChangeDto.NewPassword, true, false); // true cookie tarafından hatırlansın mı
                        TempData.Add("SuccessMessage", "Şifreniz başarıyla değiştirilmiştir.");
                        return View();
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(userPasswordChangeDto);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Lütfen girmiş olduğunuz şifrenizi kontrol ediniz.");
                    return View(userPasswordChangeDto);
                }
            }
            else
            {
              return View(userPasswordChangeDto);
            }
        }
    }
}
