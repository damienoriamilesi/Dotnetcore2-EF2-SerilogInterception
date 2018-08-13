using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace BethaniePieShop.Models
{
    public class DbSeeder
    {
        private readonly AppDbContext _ctx;
        private readonly IHostingEnvironment _hosting;

        public DbSeeder(AppDbContext ctx1, IHostingEnvironment hosting)
        {
            _ctx = ctx1;
            _hosting = hosting;
        }

        public void Seed()
        {
            _ctx.Database.EnsureCreated();

            if(!_ctx.Pies.Any())
            {
                // Get data from a file
                // var path = Path.Combine(_hosting.ContentRootPath, "Data/data.json");

                _ctx.Pies.AddRange(
                        new Pie { Name = "Blueberry pie", LongDescription = "Our famous pie !", ImageThumbnailUrl = "https://shewearsmanyhats.com/wp-content/uploads/2014/07/blueberry-pie-3.jpg", Price = 111 },
                        new Pie { Name = "Scotch pie", LongDescription = "You'll love it", ImageThumbnailUrl = "https://www.scottbrothersbutchers.co.uk/wp-content/uploads/2016/04/individual-scotch-pies-82-front-02-600x600.jpg", Price = 222 },
                        new Pie { Name = "Cherry pie", LongDescription = "Cherry on the cake !!", ImageThumbnailUrl = "https://d1a6t1943usoj7.cloudfront.net/v1/14eebe48-76df-491a-8e8f-2bdb6d09bbec/w756/1497997055803-11395_l5_clafoutis_cherry_piesandtarts_gesine_11073_11395.jpg", Price = 333 },
                        new Pie { Name = "Baby blueberry pies", LongDescription = "Baby but amazing", ImageThumbnailUrl = "https://www.luckyleaf.com/wp-content/uploads/Baby-Blueberry-Pies.jpg", Price = 444 },
                        new Pie { Name = "Chicken pie", LongDescription = "Superelisfragilisticexpialidocious", ImageThumbnailUrl = "https://www.healthymummy.com/wp-content/uploads/2017/03/Chicken-Pies.jpg", Price = 555 }
                );

                _ctx.SaveChanges();
            }
        }
    }    
}