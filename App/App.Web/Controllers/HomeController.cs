using System.Web.Mvc;

namespace App.Web.Controllers
{
	public class HomeController : BaseController
	{
		public HomeController()
		{
		}

		public ActionResult Index()
		{
			return View();
		}
	}
}