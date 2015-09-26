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
    public class ArticlesController : Controller
    {
        private readonly IArticlesService ArticlesService;
        private readonly IImagesService imagesService;
        private readonly IUoWData uoWData;

        public ArticlesController()
        {
            this.uoWData = new UoWData();
            this.ArticlesService = new ArticlesService(this.uoWData);
            this.imagesService = new ImagesService(this.uoWData);
        }

        public ActionResult Index()
        {
            IEnumerable<ArticleViewModel> model = this.ArticlesService.GetArticles();
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
                int newPageId = this.ArticlesService.CreateArticle(inputModel);
                if (newPageId > 0)
                {
                    TempData["message"] = "Статията беше добавена успешно!";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Index");
                }
            }

            TempData["message"] = "Невалидни данни за статията!<br/> Моля попълнете <strong>всички</strong> задължителни полета!";
            TempData["messageType"] = "danger";
            return View(inputModel);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            CreateArticleInputModel model = new CreateArticleInputModel();

            if (this.ArticlesService.ArticleExists(id))
            {
                model = this.ArticlesService.GetArticleInputModelById(id);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, CreateArticleInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                bool IsUpdateSuccessfull = this.ArticlesService.UpdateArticle(id, inputModel);
                if (IsUpdateSuccessfull)
                {
                    TempData["message"] = "Статията беше редактирана успешно!";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Index");
                }
            }
            TempData["message"] = "Невалидни данни за статията!<br/> Моля попълнете <strong>всички</strong> задължителни полета!";
            TempData["messageType"] = "danger";
            return View(inputModel);
        }

        public ActionResult Delete(int id)
        {

            bool isSuccessfull = this.ArticlesService.DeleteArticle(id);
            if (isSuccessfull)
            {
                TempData["message"] = "Успешно изтрихте статията!";
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