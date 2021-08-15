using ProgrammersBlog.Data.Abstract;
using ProgrammersBlog.Data.Concrete.EntityFramework.Contexts;
using ProgrammersBlog.Data.Concrete.EntityFramework.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Data.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProgrammersBlogContext _context;
        private readonly EfArticleRepository _efArticleRepository;
        private readonly EfCategoryRepository _efCategoryRepository;
        private readonly EfCommentRepository _efCommentRepository;
        
        public UnitOfWork(ProgrammersBlogContext context)
        {
            _context = context;
        }
        public IArticleRepository Articles => _efArticleRepository ?? new EfArticleRepository(_context);

        public ICategoryRepository Categories => _efCategoryRepository ?? new EfCategoryRepository(_context);

        public ICommentRepository Comments => _efCommentRepository ?? new EfCommentRepository(_context);
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

    }
}
