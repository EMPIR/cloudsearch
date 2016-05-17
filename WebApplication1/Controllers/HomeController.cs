using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                Keyword = string.Empty,
                mAction = string.Empty,
                Barrellength = string.Empty,
                Brand = string.Empty,
                Caliber = string.Empty,
                Category = string.Empty,
                Color = string.Empty,
                Department = string.Empty, 
                Finish = string.Empty,
                Rating = string.Empty,
                Type = string.Empty,
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
            
            qf.SearchResults = WebApplication1.Models.CloudSearchService.Query(qf.Keyword, qf.mAction, qf.Barrellength,qf.Brand,qf.Caliber,qf.Category,qf.Color,qf.Department,qf.Finish,qf.Rating,qf.Type, true, qf.Start);
            return View(qf);
        }

        [HttpGet]
        public async Task<ActionResult> AjaxSearch()
        {
            var model = await this.GetFullAndPartialViewModel(string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,0);

            return this.View(model);
        }

        [HttpGet]
        public async Task<ActionResult> GetSearchResults(string Keyword,
            string mAction, string Barrellength, string Brand, string Caliber, string Category, string Color, string Department,
            string Finish, string Rating, string Type, int Start)
        {
            var model = await this.GetFullAndPartialViewModel(Keyword, mAction, Barrellength, Brand, Caliber, Category, Color,
                Department, Finish, Rating, Type, Start);
            return PartialView("AjaxResults", model);
        }

        private async Task<WebApplication1.Models.ViewModels.QueryForm> GetFullAndPartialViewModel(string Keyword,
            string mAction, string Barrellength, string Brand, string Caliber, string Category, string Color, string Department,
            string Finish, string Rating, string Type, int Start)  
        {
            WebApplication1.Models.ViewModels.QueryForm qf = new Models.ViewModels.QueryForm();
            qf.Keyword = Keyword;
            qf.mAction = mAction;
            qf.Barrellength = Barrellength;
            qf.Brand = Brand;
            qf.Caliber = Caliber;
            qf.Category = Category;
            qf.Color = Color;
            qf.Department = Department;
            qf.Finish = Finish;
            qf.Rating = Rating;
            qf.Type = Type;
            qf.Start = Start;
            qf.SearchResults = WebApplication1.Models.CloudSearchService.Query(qf.Keyword, qf.mAction, qf.Barrellength, qf.Brand, qf.Caliber, qf.Category, qf.Color, qf.Department, qf.Finish, qf.Rating, qf.Type, true, qf.Start);
            return qf;
        }
    }
}