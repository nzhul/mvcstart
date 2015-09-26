using App.Data;
using App.Data.Service;
using App.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers
{
	public class BaseController : Controller
	{
		private readonly IUoWData data;
		private readonly IItemsService itemsService;
		private readonly IArticlesService articlesService;
		private readonly IPagesService pagesService;

		public BaseController()
		{
			this.data = new UoWData();
			this.itemsService = new ItemsService(this.data);
			this.articlesService = new ArticlesService(this.data);
			this.pagesService = new PagesService(this.data);

			LayoutModel model = new LayoutModel();
			model.ItemCategories = this.itemsService.GetItemCategories();
			model.Articles = this.articlesService.GetArticles().Take(5);
			model.Pages = this.pagesService.GetPages();

			ViewBag.LayoutModel = model;
		}
	}
}