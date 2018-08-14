using System.Collections.Generic;
using System.Linq;
using BethaniePieShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BethaniePieShop.Models
{
    public class FeedbackRepository : RepositoryBase<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(AppDbContext context) : base(context)
        {
        }
    }
}