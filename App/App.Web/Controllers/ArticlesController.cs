using App.Data;
using App.Data.Service;
using App.Models.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace App.Web.Controllers
{
	public class ArticlesController : BaseController
	{
		private readonly IUoWData data;
		private readonly IArticlesService articlesService;

		public ArticlesController()
		{
			this.data = new UoWData();
			this.articlesService = new ArticlesService(this.data);
		}

		[HttpGet]
		public ActionResult Index()
		{
			IEnumerable<ArticleViewModel> model = new List<ArticleViewModel>();
			model = this.articlesService.GetArticles();
			return View(model);
		}

		[HttpGet]
		public ActionResult Details(int id)
		{
			ArticleViewModel model = this.articlesService.GetArticleById(id);
			return View(model);
		}
	}
}