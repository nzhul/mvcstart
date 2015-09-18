using App.Data;
using App.Data.Service;
using App.Models;
using App.Models.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Areas.Administration.Controllers
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

        public ActionResult SaveUploadedFile(HttpPostedFileBase file)
        {
            this.imagesService.UploadGalleryImage(file);

            return Json(new { Message = "Succ: " + "fNameVariable" });
        }

        [HttpGet]
        public ActionResult DeleteGalleryImage(int id)
        {
            bool isSuccessfull = this.imagesService.DeleteGalleryImage(id);
            if (isSuccessfull)
            {
                TempData["message"] = "Снимката беше изтрита успешно!";
                TempData["messageType"] = "success";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Проблем с изтриването на картинката!<br/> Моля свържете се с администратор!";
                TempData["messageType"] = "danger";
                return RedirectToAction("Index");
            }
        }
    }
}