using BethaniePieShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

public class FeedbackController : Controller
{
    private readonly IFeedbackRepository _feedbackRepository;
    
    public FeedbackController(IFeedbackRepository feedbackRepository)
    {
        _feedbackRepository = feedbackRepository;
    }

    public IActionResult Index()
    {
        return View();
    }


    [HttpPost]
    public IActionResult Index(Feedback model)
    {
        try
        {
            _feedbackRepository.Add(model);
            return RedirectToAction("FeedbackComplete");
        }
        catch (System.Exception)
        {
            return BadRequest();
        }
    }

    public IActionResult FeedbackComplete()
    {
        return View();
    }

}