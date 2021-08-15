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
    class ArticleManager : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArticleManager(IUnitOfWork UnitOfWork, IMapper Mapper)
        {
            _unitOfWork = UnitOfWork;
            _mapper = Mapper;
        }

        public async Task<IDataResult<ArticleDto>> GetAsync(int articleId)
        {
            var article = await _unitOfWork.Articles.GetAsync(c => c.Id == articleId, a => a.User, c => c.Category);
            if(article!=null)
            {
                return new DataResult<ArticleDto>(ResultStatus.Success,
                    new ArticleDto
                    {
                        Article = article,
                        ResultStatus=ResultStatus.Success
                    });
            }
            return new DataResult<ArticleDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural:false),null);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllAsync()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(null,a => a.User, c => c.Category);
            if(articles.Count>-1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural: true), new ArticleListDto
            {
                Articles = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.NotFound(isPlural: true)
            });
        }

        public async Task<IDataResult<ArticleListDto>> GetAllbyCategoryAsync(int categoryId)
        {
            var result = await _unitOfWork.Categories.AnyAsync(x => x.Id == categoryId);
            if(result)
            {
                var articles = await _unitOfWork.Articles.GetAllAsync(a => a.CategoryId == categoryId && !a.IsDeleted && a.IsActive, a => a.User, a => a.Category);
                if (articles.Count > -1)
                {
                    return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                    {
                        Articles = articles,
                        ResultStatus = ResultStatus.Success
                    });
                }
                return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural: true), null);
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural: true), null);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllbyNonDeletedAsync()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(a=>!a.IsDeleted, a => a.User, a => a.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural: true), null);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllbyNonDeletedandActiveAsync()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(a => !a.IsDeleted && a.IsActive, a => a.User, a => a.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural: true), null);
        }

        public async Task<IResult> AddAsync(ArticleAddDto articleAddDto, string createdByName)
        {
            var article = _mapper.Map<Article>(articleAddDto);
            article.CretedByName = createdByName;
            article.ModifiedByName = createdByName;
            article.UserId = 1;
            await _unitOfWork.Articles.AddASync(article);
            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, Messages.Article.Add(article.Title));
        }
        
        public async Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName)
        {
            var article = _mapper.Map<Article>(articleUpdateDto);
            article.CretedByName = modifiedByName;
            article.ModifiedByName = modifiedByName;
            article.UserId = 1;
            await _unitOfWork.Articles.UpdateAsync(article).ContinueWith(t => _unitOfWork.SaveAsync());
            return new Result(ResultStatus.Success, Messages.Article.Update(article.Title));
        }

        public async Task<IResult> DeleteAsync(int articleId, string modifiedByName)
        {
            var result = await _unitOfWork.Articles.AnyAsync(a => a.Id == articleId);
            if (result)
            {
                var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId);
                article.IsDeleted = true;
                article.ModifiedByName = modifiedByName;
                article.ModifiedDate = DateTime.Now;
                await _unitOfWork.Articles.UpdateAsync(article);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, Messages.Article.Delete(article.Title));
            }
            return new Result(ResultStatus.Success,Messages.Article.NotFound(isPlural:false));
        }

        public async Task<IResult> HardDeleteAsync(int articleId)
        {
            var result = await _unitOfWork.Articles.AnyAsync(a => a.Id == articleId);
            if (result)
            {
                var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId);
                await _unitOfWork.Articles.DeleteAsync(article);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, Messages.Article.HardDelete(article.Title));
            }
            return new Result(ResultStatus.Success, Messages.Article.NotFound(isPlural:false));
        }

        public async Task<IDataResult<int>> CountAsync()
        {
            var articleCount = await _unitOfWork.Articles.CountAsync();
            if(articleCount>-1)
            {
                return new DataResult<int>(ResultStatus.Success, articleCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Success, "Beklenmeyen bir hata ile karşılaşıldı.", -1);
            }
        }

        public async Task<IDataResult<int>> CountByNonDeletedAsync()
        {
            var articleCount = await _unitOfWork.Articles.CountAsync(x=>!x.IsDeleted);
            if (articleCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, articleCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Success, "Beklenmeyen bir hata ile karşılaşıldı.", -1);
            }
        }
    }
}
