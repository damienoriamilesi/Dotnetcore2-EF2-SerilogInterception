using System.Linq;
using System.Threading;
using BethaniePieShop.Controllers;
using BethaniePieShop.Models;
using BethaniePieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

public class PieController : BaseController
{
    private readonly IPieRepository _pieRepository;
    public PieController(IPieRepository pieRepository)
    {
        _pieRepository = pieRepository;
    }

    public virtual IActionResult Index()
    {
        var model = new PieViewModel();
        model.Title = "Welcome to Bethany's Pie Shop";
        var pies = _pieRepository.GetAll();
        model.Pies = pies.ToList();

        return View(model);
    }

    [HttpPost]
    public virtual IActionResult Index(PieViewModel viewModel)
    {
        return View(viewModel);
    }

    public virtual IActionResult Details(int id)
    {
        //Thread.Sleep(5000);

        var pie = _pieRepository.Get(id);
        if (pie == null)
        {
            return NotFound();
        }
        return View(pie);
    }
}