using System;
using System.Collections.Generic;
using System.Text;

namespace ProgrammersBlog.Entities.Dtos
{
    public class ImageUploadedDto
    {
        public string FullName { get; set; }
        public string OldName { get; set; }
        public string Extension { get; set; } // uzantısı
        public string Path { get; set; } //resmin yolu
        public string FolderName { get; set; } // klasör ismi
        public long Size { get; set; } //resmin boyutu
    }
}
