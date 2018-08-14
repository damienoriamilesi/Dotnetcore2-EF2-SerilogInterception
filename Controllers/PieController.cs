using BethaniePieShop.Models;
using BethaniePieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

public class PieController : Controller
{
    private readonly IPieRepository _pieRepository;
    public PieController(IPieRepository pieRepository)
    {
        _pieRepository = pieRepository;
    }

    public IActionResult Index()
    {
        var model = new PieViewModel();
        model.Title = "Welcome to Bethany's Pie Shop";
        var pies = _pieRepository.GetAll();
        model.Pies = pies;

        return View(model);
    }

    public IActionResult Details(int id)
    {
        var pie = _pieRepository.Get(id);
        if (pie == null)
        {
            return NotFound();
        }
        return View(pie);
    }


}