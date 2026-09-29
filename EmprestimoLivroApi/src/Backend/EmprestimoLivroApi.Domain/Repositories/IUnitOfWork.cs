using System;
using System.Collections.Generic;
using System.Text;

namespace EmprestimoLivroApi.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
