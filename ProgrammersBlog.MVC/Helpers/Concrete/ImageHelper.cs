using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.MVC.Helpers.Abstract;
using ProgrammersBlog.Shared.Utilities.Extensions;
using ProgrammersBlog.Shared.Utilities.Results.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.CompexTypes;
using ProgrammersBlog.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammersBlog.MVC.Helpers.Concrete
{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _wwwroot;
        private readonly string imgFolder = "img";

        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
            _wwwroot = _env.WebRootPath; // wwwrootun yolu
        }

        public IDataResult<ImageDeleteDto> Delete(string pictureName)
        {
           
            var filetoDelete = Path.Combine($"{_wwwroot}/{imgFolder}/", pictureName);
            if (System.IO.File.Exists(filetoDelete))
            {
                var fileInfo = new FileInfo(filetoDelete); // pathte bulunan resimdeki bilgilere ulaşabileceğiz.
                var imageDeletedDto = new ImageDeleteDto
                {
                    FullName = pictureName,
                    Extension = fileInfo.Extension,
                    Path = fileInfo.FullName,// path ve dosya uzantısını birlikte veriyor.
                    Size = fileInfo.Length

                };
                System.IO.File.Delete(filetoDelete);
                return new DataResult<ImageDeleteDto>(ResultStatus.Success,imageDeletedDto);
            }
            else
            {
                return new DataResult<ImageDeleteDto>(ResultStatus.Error, "Böyle bir resim bulunamadı.",null);
            }
        }

        public async Task<IDataResult<ImageUploadedDto>> UploadUserImage(string userName, IFormFile pictureFile, string folderName="userImages")
        {
            if(!Directory.Exists($"{_wwwroot}/{imgFolder}/{folderName}"))
            {
                Directory.CreateDirectory($"{_wwwroot}/{imgFolder}/{folderName}"); // eğer resmin yükleneceği klasör yoksa
            }
            string oldFileName = Path.GetFileNameWithoutExtension(pictureFile.FileName);
            string fileExtension = Path.GetExtension(pictureFile.FileName);
            DateTime dateTime = DateTime.Now;
            
            string newFileName = $"{userName}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}";
            var path = Path.Combine($"{_wwwroot}/{imgFolder}/{folderName}",newFileName);

            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
            }
            return new DataResult<ImageUploadedDto>(ResultStatus.Success, $"{userName} adlı kullanıcının resmi başarıyla  yüklenmiştir.",
                new ImageUploadedDto 
                {
                    FullName=$"{folderName}/{newFileName}",
                    OldName=oldFileName,
                    Extension=fileExtension,
                    FolderName=folderName,
                    Path=path,
                    Size=pictureFile.Length
                });
            #region
            /*
            string wwwroot = _env.WebRootPath; // wwwrootun yolunu vericek.
            //string fileName = Path.GetFileNameWithoutExtension(userAddDto.Picture.FileName); // .jpg .png gibi uzantıları almadan ismi çekiyoruz.
            //resim ismini verirken sonuna resmin ismini eklemek istersek fileName içinde kullanabiliriz.
            string fileExtension = Path.GetExtension(pictureFile.FileName); //burdanda uzantıyı aldık.
            DateTime dateTime = DateTime.Now;
            //alt satırda oluşucak deger soyle gözukucektir : OzanKazanc_587_5_38_12_3_10_2021.png
            string fileName = $"{userName}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}";
            var path = Path.Combine($"{wwwroot}/img", fileName);

            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
            }
            return fileName;*/
            #endregion // UserControllerdaki hali
        }
    }
}
