using System.Collections.Generic;

namespace BethaniePieShop.Models
{
    public interface IRepository<T> where T : class
    {
        IList<T> GetAll();
        T Get(int id);

        void Add(T entity);
    }
}