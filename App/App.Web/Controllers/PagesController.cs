using App.Data;
using App.Data.Service;
using App.Models.ViewModels;
using System.Web.Mvc;

namespace App.Web.Controllers
{
	public class PagesController : BaseController
	{
		private readonly IUoWData data;
		private readonly IPagesService pagesService;
		public PagesController()
		{
			this.data = new UoWData();
			this.pagesService = new PagesService(this.data);
		}

		public ActionResult Index(int id)
		{
			PageViewModel model = this.pagesService.GetPageById(id);
			return View(model);
		}
	}
}