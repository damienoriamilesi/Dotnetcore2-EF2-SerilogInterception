using System.Collections.Generic;
using BethaniePieShop.Models;

namespace BethaniePieShop.ViewModels
{
    public class PieViewModel
    {
        public string Title { get; set; }
        public List<Pie> Pies { get; set; }
    }
}