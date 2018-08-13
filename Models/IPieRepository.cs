using System.Collections.Generic;
using BethaniePieShop.Models;

public interface IPieRepository : IRepository<Pie>
{

}

public interface IRepository<T> where T : class
{
    IList<T> GetAll();
    T Get(int id);

    void Add(T entity);
}