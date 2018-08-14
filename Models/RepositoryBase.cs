using System.Collections.Generic;
using System.Linq;
using BethaniePieShop.Models;
using Microsoft.EntityFrameworkCore;

public abstract class RepositoryBase<T> where T : class
{
    private readonly AppDbContext _context;

    public RepositoryBase(AppDbContext context)
    {
        _context = context;
    }

    public T Get(int pieId)
    {
        //return GetAllPies().FirstOrDefault(x=>x.Id == pieId);

        return _context.Find<T>(pieId);
    }

    public IList<T> GetAll()
    {
        // return new List<Pie>{
        //     new Pie { Id = 1, ShortDescription = "Blueberry pie", LongDescription = "", ImageThumbnailUrl = "https://shewearsmanyhats.com/wp-content/uploads/2014/07/blueberry-pie-3.jpg", Price = 111 },
        //     new Pie { Id = 2, ShortDescription = "Scotch pie", LongDescription = "", ImageThumbnailUrl = "https://www.scottbrothersbutchers.co.uk/wp-content/uploads/2016/04/individual-scotch-pies-82-front-02-600x600.jpg", Price = 222 },
        //     new Pie { Id = 3, ShortDescription = "Cherry pie", LongDescription = "", ImageThumbnailUrl = "https://d1a6t1943usoj7.cloudfront.net/v1/14eebe48-76df-491a-8e8f-2bdb6d09bbec/w756/1497997055803-11395_l5_clafoutis_cherry_piesandtarts_gesine_11073_11395.jpg", Price = 333 },
        //     new Pie { Id = 4, ShortDescription = "baby blueberry pies", LongDescription = "", ImageThumbnailUrl = "https://www.luckyleaf.com/wp-content/uploads/Baby-Blueberry-Pies.jpg", Price = 444 },
        //     new Pie { Id = 5, ShortDescription = "Chicken pie", LongDescription = "", ImageThumbnailUrl = "https://www.healthymummy.com/wp-content/uploads/2017/03/Chicken-Pies.jpg", Price = 555 }
        // };

        return _context.Set<T>().ToList();
    }

    public void Add(T entity){
        _context.Set<T>().Add(entity);
        _context.SaveChanges();
    }
}
