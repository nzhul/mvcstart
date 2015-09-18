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
    public class PagesController : BaseController
    {
        private readonly IPagesService pagesService;
        private readonly IUoWData data;

        public PagesController()
        {
            this.data = new UoWData();
            this.pagesService = new PagesService(data);
        }

        public ActionResult Index()
        {
            IEnumerable<PageViewModel> model = this.pagesService.GetPages();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreatePageInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                int newPageId = this.pagesService.CreatePage(inputModel);
                if (newPageId > 0)
                {
                    TempData["message"] = "Страницата беше добавена успешно!";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Index");
                }
            }

            TempData["message"] = "Невалидни данни за страницата!<br/> Моля попълнете <strong>всички</strong> задължителни полета!";
            TempData["messageType"] = "danger";
            return View(inputModel);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            CreatePageInputModel model = new CreatePageInputModel();

            if (this.pagesService.PageExists(id))
            {
                model = this.pagesService.GetPageInputModelById(id);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, CreatePageInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                bool IsUpdateSuccessfull = this.pagesService.UpdatePage(id, inputModel);
                if (IsUpdateSuccessfull)
                {
                    TempData["message"] = "Страницата беше редактирана успешно!";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Index");
                }
            }
            TempData["message"] = "Невалидни данни за страницата!<br/> Моля попълнете <strong>всички</strong> задължителни полета!";
            TempData["messageType"] = "danger";
            return View(inputModel);
        }

        public ActionResult Delete(int id)
        {

            bool isSuccessfull = this.pagesService.DeletePage(id);
            if (isSuccessfull)
            {
                TempData["message"] = "Успешно изтрихте страницата!";
                TempData["messageType"] = "success";
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}
