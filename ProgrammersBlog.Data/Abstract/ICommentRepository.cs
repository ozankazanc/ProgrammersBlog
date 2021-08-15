using System;
using System.Collections.Generic;
using System.Text;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Shared.Data.Abstract;

namespace ProgrammersBlog.Data.Abstract
{
    public interface ICommentRepository : IEntityRepository<Comment>
    {
    }
}
