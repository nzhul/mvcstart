using App.Data;
using App.Data.Service;
using App.Models;
using App.Models.ViewModels;
using App.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers
{
	public class ItemsController : BaseController
	{
		private readonly IItemsService itemsService;
		private readonly IUoWData data;


		public ItemsController()
		{
			this.data = new UoWData();
			this.itemsService = new ItemsService(this.data);
		}

		[HttpGet]
		public ActionResult Index(int? categoryId)
		{
			IEnumerable<ItemViewModel> model = new List<ItemViewModel>();
			model = this.itemsService.GetItems(categoryId);

			return View(model);
		}


		[HttpGet]
		public ActionResult Details(int id)
		{
			ItemDetailsViewModel model = new ItemDetailsViewModel();

			model.TheItem = this.itemsService.GetItemById(id);
			model.SimilarItems = this.itemsService.GetItems(model.TheItem.ItemCategoryId).Where(r => r.Id != id);

			List<Image> images = model.TheItem.Images.ToList();
			Image defaultImage = images.Where(i => i.ImagePath.Contains("no-image")).FirstOrDefault();
			images.Remove(defaultImage);
			model.TheItem.Images = images;

			return View(model);
		}
	}
}