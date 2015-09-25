using App.Data;
using App.Data.Service;
using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers
{
    public class AttractionsController : BaseController
    {
        private readonly IUoWData data;
        private readonly IArticlesService attractionsService;

        public AttractionsController()
        {
            this.data = new UoWData();
            this.attractionsService = new ArticlesService(this.data);
        }

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<ArticleViewModel> model = new List<ArticleViewModel>();
            model = this.attractionsService.GetArticles();
            return View(model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            ArticleViewModel model = this.attractionsService.GetArticleById(id);
            return View(model);
        }
    }
}