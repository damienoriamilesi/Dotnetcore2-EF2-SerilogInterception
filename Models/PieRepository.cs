using BethaniePieShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BethaniePieShop.Models
{
    public class PieRepository : RepositoryBase<Pie>, IPieRepository
    {
        public PieRepository(AppDbContext context) : base(context)
        {
        }    
    }
}
