using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProgrammersBlog.Entities.Dtos
{
    public class UserPasswordChangeDto
    {
        [DisplayName("Şu Anki Şifreniz")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]  //display name alanında yazan değeri referans alıyor {0}.
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden fazla olmamalıdır.")]
        [MinLength(4, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        
        [DisplayName("Yeni Şifreniz")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]  //display name alanında yazan değeri referans alıyor {0}.
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden fazla olmamalıdır.")]
        [MinLength(4, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        
        [DisplayName("Yeni Şifrenizin Tekrarı")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]  //display name alanında yazan değeri referans alıyor {0}.
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden fazla olmamalıdır.")]
        [MinLength(4, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage ="Girmiş olduğunuz yeni şifreler birbirleriyle uyuşmuyor.")]
        public string RepeatPassword { get; set; }
    }
}
