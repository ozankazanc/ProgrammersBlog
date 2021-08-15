using System;
using System.Collections.Generic;
using System.Text;

namespace ProgrammersBlog.Services.Utilities
{
    public static class Messages
    {
        public static class Category
        {
            public static string NotFound(bool isPlural)// çoğul mu tekil mi
            {
                if (isPlural) return "Hiç bir kategori bulunamadı."; //Get() Tekil
                else return "Böyle bir kategori bulunamadı."; //GetAll() Çoğul
            }
            public static string Add(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla eklenmiştir.";
            }
            public static string Update (string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla güncellenmiştir.";
            }
            public static string Delete(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla silinmiştir.";
            }
            public static string HardDelete(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla veritabanından silinmiştir.";
            }

        }
        public static class Article
        {
            public static string NotFound(bool isPlural)// çoğul mu tekil mi
            {
                if (isPlural) return "Makaleler bulunamadı."; //Get() Tekil
                else return "Böyle bir makale bulunamadı."; //GetAll() Çoğul
            }
            public static string Add(string articleName)
            {
                return $"{articleName} başlıklı makale başarıyla eklenmiştir.";
            }
            public static string Update(string articleName)
            {
                return $"{articleName} başlıklı makale başarıyla güncellenmiştir.";
            }
            public static string Delete(string articleName)
            {
                return $"{articleName} başlıklı makale başarıyla silinmiştir.";
            }
            public static string HardDelete(string articleName)
            {
                return $"{articleName} başlıklı makale başarıyla veritabanından silinmiştir.";
            }


        }
    }
}
