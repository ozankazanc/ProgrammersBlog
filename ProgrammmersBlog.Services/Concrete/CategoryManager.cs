using AutoMapper;
using ProgrammersBlog.Data.Abstract;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Services.Utilities;
using ProgrammersBlog.Shared.Utilities.Results.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.CompexTypes;
using ProgrammersBlog.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Services.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryManager(IUnitOfWork unitOfWork,IMapper Mapper)
        {
            
            _unitOfWork = unitOfWork;
            _mapper = Mapper;
        }
        /// <summary>
        /// Verilen CategoryAddDto ve CreatedByName parametlerine ait bilgiler ile yeni bir Category ekler.
        /// </summary>
        /// <param name="categoryAddDto">CategoryAddDto tipinde eklenecek kategori bilgileri</param>
        /// <param name="createdByName">string tipinde kullanıcının kullanıcı adı</param>
        /// <returns>Asenkron bir operasyon ile Task olarak ekleme işlemin sonucu DataResult tipinde döner.</returns>
        public async Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto categoryAddDto, string createdByName)
        {
            
            var category = _mapper.Map<Category>(categoryAddDto);
            category.CretedByName = createdByName;
            category.ModifiedByName = createdByName;
            var addedCategory = await _unitOfWork.Categories.AddASync(category);
            await _unitOfWork.SaveAsync();


            //await _unitOfWork.Categories.AddASync(category).ContinueWith(t=> _unitofwork.savechanges) için alttaki üç satır geçerli;
            //zincirleme olarak ekleme işlemi bittiği an save işlemi gerçekleşir ve hız kazandırır. 
            //veritabanı save işlemini gerçekleştiremeden return dönebilir.
            //Bu da belki veritabanına ekleme esnasında hata oluşsa dahi kullanıcıya başarıyla eklenmiştir ResultStatusu basılabilir.
            ///////////////////////////////////////////////////////////////////
            //IMapper kütüphanesini kullandığımız için bu alana gerek kalmadı./
            ///////////////////////////////////////////////////////////////////
            //await _unitOfWork.SaveAsync();
            //await _unitOfWork.Categories.AddASync(new Category
            //{
            //    Name = categoryAddDto.Name,
            //    Description = categoryAddDto.Description,
            //    Note = categoryAddDto.Note,
            //    IsActive = categoryAddDto.IsActive,
            //    CretedByName = createdByName,
            //    CreatedDate = DateTime.Now,
            //    ModifiedByName = createdByName,
            //    ModifiedDate = DateTime.Now,
            //    IsDeleted = false
            //}).ContinueWith(t => _unitOfWork.SaveAsync()); 
            //////////////////////////////////////////////////////////////
            
            return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.Add(addedCategory.Name),
                new CategoryDto 
                {
                    Category=addedCategory,
                    ResultStatus=ResultStatus.Success,
                    Message = Messages.Category.Add(addedCategory.Name)
                });
        }
        public async Task<IDataResult<CategoryDto>> GetAsync(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if(category!=null)
            {
                return new DataResult<CategoryDto>(ResultStatus.Success,
                new CategoryDto
                {
                    Category = category,
                    ResultStatus=ResultStatus.Success
                }) ;
            }
            return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.NotFound(isPlural:false),
                new CategoryDto 
                { 
                    Category=null,
                    ResultStatus=ResultStatus.Error,
                    Message= Messages.Category.NotFound(isPlural: false)
                });
        }
        public async Task<IDataResult<CategoryListDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(null); // ilişkili olan articles için parantez içine x=> x.Articles şeklinde kategoriye ait article da çekebiliriz.
            if(categories.Count>-1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, 
                    new CategoryListDto 
                    {
                        Categories=categories,
                        ResultStatus=ResultStatus.Success
                    });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: true), new CategoryListDto 
            {
                Categories=null,
                ResultStatus=ResultStatus.Error,
                Message= Messages.Category.NotFound(isPlural: true)
            });
        }
        public async Task<IDataResult<CategoryListDto>> GetAllbyNonDeletedAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(c=>!c.IsDeleted);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success,
                    new CategoryListDto
                    {
                        Categories = categories,
                        ResultStatus = ResultStatus.Success
                    });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: true), new CategoryListDto
            {
                Categories = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.NotFound(isPlural: true)
            });
        }
        public async Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            var oldCategory = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryUpdateDto.Id);
            var category = _mapper.Map<CategoryUpdateDto,Category>(categoryUpdateDto,oldCategory);
            category.ModifiedByName = modifiedByName;

            var updatedCategory = await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveAsync();
            return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.Update(updatedCategory.Name),
                new CategoryDto
                {
                    Category = updatedCategory,
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Category.Update(updatedCategory.Name)
                });
        }
        public async Task<IDataResult<CategoryDto>> DeleteAsync(int categoryId, string modifiedByName)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {
                category.IsDeleted = true;
                category.ModifiedByName = modifiedByName;
                category.ModifiedDate = DateTime.Now;
                var deletedCategory = await _unitOfWork.Categories.UpdateAsync(category);
                await _unitOfWork.SaveAsync();
                return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.Delete(category.Name),
                    new CategoryDto
                    {
                        Category = deletedCategory,
                        ResultStatus = ResultStatus.Success,
                        Message = Messages.Category.Delete(category.Name)
                    });
            }
            return new DataResult<CategoryDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural:false), new CategoryDto
            {
                Category = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.NotFound(isPlural: false)
            });
        }
        public async Task<IResult> HardDeleteAsync(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {
                await _unitOfWork.Categories.DeleteAsync(category);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, Messages.Category.HardDelete(category.Name));
            }
            return new Result(ResultStatus.Error, Messages.Category.HardDelete(category.Name));
        }
        public async Task<IDataResult<CategoryListDto>> GetAllbyNonDeletedAndActiveAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted && c.IsActive);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success,
                    new CategoryListDto
                    {
                        Categories = categories,
                        ResultStatus = ResultStatus.Success
                    });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: false), new CategoryListDto
            {
                Categories = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.NotFound(isPlural: false)
            });
        }
        
        /// <summary>
        /// Verilen ID parametresine ait kategorinin CategoryUpdateDto temsilini döner.
        /// </summary>
        /// <param name="categoryId">0'dan büyük integer bir ID değeri</param>
        /// <returns>Asenkron bir operasyon ile Task olarak işlem sonucunu DataResult tipinde geriye döner.</returns>
        public async Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDtoAsync(int categoryId)

        {
            var result = await _unitOfWork.Categories.AnyAsync(c=>c.Id==categoryId);
            if(result)
            {
                var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
                var categoryUpdateDto = _mapper.Map<CategoryUpdateDto>(category);
                return new DataResult<CategoryUpdateDto>(ResultStatus.Success, categoryUpdateDto);
            }
            else
            {
                return new DataResult<CategoryUpdateDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: false), null);
            }
        }
        public async Task<IDataResult<int>> CountAsync()
        {
            var categoriesCount = await _unitOfWork.Categories.CountAsync();
            if(categoriesCount>-1)
            {
                return new DataResult<int>(ResultStatus.Success, categoriesCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Success, "Beklenmeyen bir hata ile karşılaşıldı.", -1);
            }

        }
        public async Task<IDataResult<int>> CountByNonDeletedAsync()
        {
            var categoriesCount = await _unitOfWork.Categories.CountAsync(x=>!x.IsDeleted);
            if (categoriesCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, categoriesCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Success, "Beklenmeyen bir hata ile karşılaşıldı.", -1);
            }
        }
    }
}
