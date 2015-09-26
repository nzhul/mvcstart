using App.Data;
using App.Data.Service;
using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers
{
	public class GalleryController : BaseController
	{
		private readonly IImagesService imagesService;
		private readonly IUoWData data;

		public GalleryController()
		{
			this.data = new UoWData();
			this.imagesService = new ImagesService(data);
		}

		// GET: Administration/Gallery
		public ActionResult Index()
		{
			IEnumerable<Image> images = this.imagesService.GetGalleryImage();
			return View(images);
		}
	}
}