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
    public class ItemCategoriesController : BaseController
    {
        private readonly IItemsService itemsService;
        private readonly IUoWData uoWData;
        public ItemCategoriesController()
        {
            this.uoWData = new UoWData();
            this.itemsService = new ItemsService(this.uoWData);
        }

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<ItemCategoryViewModel> model = this.itemsService.GetItemCategories();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateItemCategoryInputModel categoryInput)
        {
            if (ModelState.IsValid)
            {
                int result = this.itemsService.CreateItemCategory(categoryInput);
                if (result > 0)
                {
                    TempData["message"] = "Успешно добавихте нова категория!";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Index");
                }
            }

            TempData["message"] = "Невалидни данни за категорията!<br/> Моля попълнете <strong>всички</strong> задължителни полета!";
            TempData["messageType"] = "danger";
            return View(categoryInput);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            CreateItemCategoryInputModel model = this.itemsService.GetItemCategoryInputModelById(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, CreateItemCategoryInputModel itemCategory)
        {
            if (ModelState.IsValid)
            {
                bool result = this.itemsService.UpdateItemCategory(id, itemCategory);

                if (result == true)
                {
                    TempData["message"] = "Редактирахте успешно категорията!";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Index");
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }

            TempData["message"] = "Невалидни данни за категорията!<br/> Моля попълнете <strong>всички</strong> полета в червено!";
            TempData["messageType"] = "danger";
            return View(itemCategory);
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
