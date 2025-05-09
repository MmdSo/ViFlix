using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Context;

namespace ViFlix.Data.Repository
{
    public interface IGenericRepository <TEntity> : IAsyncDisposable where TEntity : BaseEntitiies
    {
        IEnumerable<TEntity> GetAll();
        IAsyncEnumerable<TEntity> GetAllAsync();
        Task<TEntity> GetEntityByIdAsync(long? Id);
        TEntity GetEntityById(long? Id);
        Task<long> AddEntity(TEntity entity);
        void DeleteEntity(TEntity entity);
        void EditEntity(TEntity entity);
        Task SaveChanges();
    }
}
