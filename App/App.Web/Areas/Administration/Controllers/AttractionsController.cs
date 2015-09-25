using App.Data;
using App.Data.Service;
using App.Models.InputModels;
using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Areas.Administration.Controllers
{
    public class AttractionsController : Controller
    {
        private readonly IArticlesService attractionsService;
        private readonly IImagesService imagesService;
        private readonly IUoWData uoWData;

        public AttractionsController()
        {
            this.uoWData = new UoWData();
            this.attractionsService = new ArticlesService(this.uoWData);
            this.imagesService = new ImagesService(this.uoWData);
        }

        public ActionResult Index()
        {
            IEnumerable<ArticleViewModel> model = this.attractionsService.GetArticles();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateArticleInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                int newPageId = this.attractionsService.CreateArticle(inputModel);
                if (newPageId > 0)
                {
                    TempData["message"] = "Атракцията беше добавена успешно!";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Index");
                }
            }

            TempData["message"] = "Невалидни данни за атракцията!<br/> Моля попълнете <strong>всички</strong> задължителни полета!";
            TempData["messageType"] = "danger";
            return View(inputModel);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            CreateArticleInputModel model = new CreateArticleInputModel();

            if (this.attractionsService.ArticleExists(id))
            {
                model = this.attractionsService.GetArticleInputModelById(id);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, CreateArticleInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                bool IsUpdateSuccessfull = this.attractionsService.UpdateArticle(id, inputModel);
                if (IsUpdateSuccessfull)
                {
                    TempData["message"] = "Атракцията беше редактирана успешно!";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Index");
                }
            }
            TempData["message"] = "Невалидни данни за атракцията!<br/> Моля попълнете <strong>всички</strong> задължителни полета!";
            TempData["messageType"] = "danger";
            return View(inputModel);
        }

        public ActionResult Delete(int id)
        {

            bool isSuccessfull = this.attractionsService.DeleteArticle(id);
            if (isSuccessfull)
            {
                TempData["message"] = "Успешно изтрихте атракцията!";
                TempData["messageType"] = "success";
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult UploadPhotos(UploadArticlePhotoModel uploadData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.imagesService.UploadImages(uploadData);
                }

                TempData["message"] = "Снимката беше <strong>добавена</strong> успешно!";
                TempData["messageType"] = "success";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = "Неуспешно качване на снимка!<br/> Моля свържете се с администратор!";
                TempData["messageType"] = "danger";
                return RedirectToAction("Index");
            }
        }
    }
}