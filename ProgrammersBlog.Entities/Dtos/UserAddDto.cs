using Microsoft.AspNetCore.Http;
using ProgrammersBlog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProgrammersBlog.Entities.Dtos
{
    public class UserAddDto
    {
        [DisplayName("Kullanıcı Adı")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]  //display name alanında yazan değeri referans alıyor {0}.
        [MaxLength(50, ErrorMessage = "{0} {1} karakterden fazla olmamalıdır.")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        public string UserName { get; set; }

        [DisplayName("E-Mail Adresi")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]  //display name alanında yazan değeri referans alıyor {0}.
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden fazla olmamalıdır.")]
        [MinLength(10, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Şifre")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]  //display name alanında yazan değeri referans alıyor {0}.
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden fazla olmamalıdır.")]
        [MinLength(4, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        
        [DisplayName("Telefon Numarası")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]  //display name alanında yazan değeri referans alıyor {0}.
        [MaxLength(13, ErrorMessage = "{0} {1} karakterden fazla olmamalıdır.")] //+905555555555 13 karakter
        [MinLength(13, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DisplayName("Resim")]
        [Required(ErrorMessage = "Lütfen bir {0} seçiniz")]  //display name alanında yazan değeri referans alıyor {0}.
        [DataType(DataType.Upload)] //vermesekte çalışıyor.
        public IFormFile PictureFile { get; set; }
        public string Picture { get; set; }
    }
}
