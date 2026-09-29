using EmprestimoLivroApi.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmprestimoLivroApi.Infrastructure.DataAccess
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly EmprestimoLivroApiDbContext _dbContext;

        public UnitOfWork(EmprestimoLivroApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit() => await _dbContext.SaveChangesAsync();
    }
}
