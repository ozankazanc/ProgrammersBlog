using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProgrammersBlog.Entities.Dtos
{
    public class CategoryAddDto
    {
        [DisplayName("Kategori Adı")]
        [Required(ErrorMessage ="{0} boş geçilmemelidir.")]  //display name alanında yazan değeri referans alıyor {0}.
        [MaxLength(70,ErrorMessage = "{0} {1} karakterden fazla olmamalıdır.")]
        [MinLength(3,ErrorMessage ="{0} {1} karakterden az olmamalıdır.")]
        public string Name { get; set; }
        
        [DisplayName("Kategori Açıklaması")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]  //display name alanında yazan değeri referans alıyor {0}.
        [MaxLength(500, ErrorMessage = "{0} {1} karakterden fazla olmamalıdır.")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        public string Description { get; set; }

        [DisplayName("Kategori Özel Not Alanı")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]  //display name alanında yazan değeri referans alıyor {0}.
        [MaxLength(500, ErrorMessage = "{0} {1} karakterden fazla olmamalıdır.")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        public string Note { get; set; }
        
        [DisplayName("Aktiflik Durumu")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]  //display name alanında yazan değeri referans alıyor {0}.
        public bool IsActive { get; set; }
    }
}
