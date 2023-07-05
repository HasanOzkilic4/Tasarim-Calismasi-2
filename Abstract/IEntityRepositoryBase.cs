using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Abstract
{
    public interface IEntityRepositoryBase<T>
    {
        void Add(T entity);
        void Delete(string userName);
        void Update(string currentUsername, T entity);
        Task<List<T>> GetAll(Func<T, bool> filter = null);
        Task<T> Get(string userName);

    }
}
