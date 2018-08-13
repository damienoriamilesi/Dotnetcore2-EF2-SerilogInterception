using System.Collections.Generic;
using BethaniePieShop.Models;

namespace BethaniePieShop.ViewModels
{
    public class PieViewModel
    {
        public string Title { get; set; }
        public IList<Pie> Pies { get; set; }
    }
}