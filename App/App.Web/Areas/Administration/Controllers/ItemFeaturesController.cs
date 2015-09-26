using App.Data;
using App.Data.Service;
using App.Models.InputModels;
using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Areas.Administration.Controllers
{
    public class ItemFeaturesController : BaseController
    {
        private readonly IItemsService itemsService;
        private readonly IUoWData uoWData;
        public ItemFeaturesController()
        {
            this.uoWData = new UoWData();
            this.itemsService = new ItemsService(this.uoWData);
        }


        public ActionResult Index()
        {
            IEnumerable<ItemFeatureViewModel> model = this.itemsService.GetItemFeatures();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateItemFeatureInputModel featureInput)
        {
            if (ModelState.IsValid)
            {
                int result = this.itemsService.CreateItemFeature(featureInput);
                if (result > 0)
                {
                    TempData["message"] = "Успешно добавихте ново характеристика!";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Index");
                }
            }

            TempData["message"] = "Невалидни данни за характеристикато!<br/> Моля попълнете <strong>всички</strong> задължителни полета!";
            TempData["messageType"] = "danger";
            return View(featureInput);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            CreateItemFeatureInputModel model = this.itemsService.GetItemFeatureInputModelById(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, CreateItemFeatureInputModel itemFeature)
        {
            if (ModelState.IsValid)
            {
                bool result = this.itemsService.UpdateItemFeature(id, itemFeature);

                if (result == true)
                {
                    TempData["message"] = "Редактирахте успешно характеристикато!";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Index");
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }

            TempData["message"] = "Невалидни данни за характеристикато!<br/> Моля попълнете <strong>всички</strong> полета в червено!";
            TempData["messageType"] = "danger";
            return View(itemFeature);
        }

        public ActionResult Delete(int id)
        {

            bool isSuccessfull = this.itemsService.DeleteItemFeature(id);
            if (isSuccessfull)
            {
                TempData["message"] = "Успешно изтрихте характеристикато!";
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