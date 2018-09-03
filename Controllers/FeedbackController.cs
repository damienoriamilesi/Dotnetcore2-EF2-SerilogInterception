using BethaniePieShop.Controllers;
using BethaniePieShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

public class FeedbackController : BaseController
{
    private readonly IFeedbackRepository _feedbackRepository;
    
    public FeedbackController(IFeedbackRepository feedbackRepository)
    {
        _feedbackRepository = feedbackRepository;
    }

    public virtual IActionResult Index()
    {
        return View();
    }


    [HttpPost]
    public virtual IActionResult Index(Feedback model)
    {
        try
        {
            if(ModelState.IsValid)
            {
                _feedbackRepository.Add(model);
                return RedirectToAction("FeedbackComplete");
            }   
            return View(model);
        }
        catch (System.Exception)
        {
            return BadRequest();
        }
    }

    public virtual IActionResult FeedbackComplete()
    {
        return View();
    }

}