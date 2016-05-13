using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            WebApplication1.Models.ViewModels.QueryForm qf = new Models.ViewModels.QueryForm
            {
                UserAction = string.Empty,
                Keyword = string.Empty,
                Gauge = string.Empty,
                Manufacturer = string.Empty,
                Start = 0
            };
            return View(qf);
        }

        [HttpGet]
        public ActionResult Results(WebApplication1.Models.ViewModels.QueryForm qf)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            
            qf.SearchResults = WebApplication1.Models.CloudSearchService.Query(qf.Keyword, qf.Gauge, qf.UserAction, qf.Manufacturer, true, qf.Start);
            return View(qf);
        }
    }
}