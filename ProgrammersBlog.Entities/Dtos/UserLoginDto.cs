using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProgrammersBlog.Entities.Dtos
{
    public class UserLoginDto
    {
        
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
        [DisplayName("Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
