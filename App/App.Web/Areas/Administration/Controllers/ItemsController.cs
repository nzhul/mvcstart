using App.Data;
using App.Data.Service;
using App.Models;
using App.Models.InputModels;
using App.Models.ViewModels;
using App.Web.Areas.Administration.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Areas.Administration.Controllers
{
    public class ItemsController : BaseController
    {
        private readonly IItemsService itemsService;
        private readonly IImagesService imagesService;
        private readonly IUoWData uoWData;
        public ItemsController()
        {
            this.uoWData = new UoWData();
            this.itemsService = new ItemsService(this.uoWData);
            this.imagesService = new ImagesService(this.uoWData);
        }

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<ItemViewModel> itemsCollection = this.itemsService.GetItems(null);

            HomeViewModel homeModel = new HomeViewModel();
            homeModel.Items = itemsCollection;

            return View(homeModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            CreateItemInputModel model = new CreateItemInputModel();
            model.Categories = this.itemsService.GetCategories();
            model.AvailableItemFeatures = this.itemsService.GetAvailableItemFeatures();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateItemInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                bool IsCreateItemSuccessfull = this.itemsService.CreateItem(inputModel);
                if (IsCreateItemSuccessfull)
                {
                    TempData["message"] = "Продукта беше добавен успешно!";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Index");
                }
            }

            inputModel.Categories = this.itemsService.GetCategories();
            TempData["message"] = "Невалидни данни за продукта!<br/> Моля попълнете <strong>всички</strong> задължителни полета!";
            TempData["messageType"] = "danger";
            return View(inputModel);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            CreateItemInputModel model = new CreateItemInputModel();

            if (this.itemsService.ItemExists(id))
            {
                model = this.itemsService.GetItemInputModelById(id);
                model.Categories = this.itemsService.GetCategories();
                model.AvailableItemFeatures = this.itemsService.GetAvailableItemFeatures();
                model.SelectedItemFeatureIds = this.itemsService.GetSelectedItemFeatureIds(id);
            }
            else
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, CreateItemInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                bool IsUpdateSuccessfull = this.itemsService.UpdateItem(id, inputModel);
                if (IsUpdateSuccessfull)
                {
                    TempData["message"] = "Продуктта беше редактирана успешно!";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Index");
                }
            }

            inputModel.Categories = this.itemsService.GetCategories();
            inputModel.AvailableItemFeatures = this.itemsService.GetAvailableItemFeatures();
            TempData["message"] = "Невалидни данни за продуктта!<br/> Моля попълнете <strong>всички</strong> задължителни полета!";
            TempData["messageType"] = "danger";
            return View(inputModel);
        }

        [HttpPost]
        public ActionResult UploadPhotos(UploadPhotoModel uploadData)
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

        [HttpGet]
        public ActionResult MakePrimary(int imageId, int productId)
        {
            bool IsSuccessfull = this.imagesService.MakePrimary(imageId, productId);

            if (IsSuccessfull)
            {
                return RedirectToAction("Edit", new { id = productId });
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            bool itemExists = this.itemsService.ItemExists(id);
            if (itemExists)
            {
                bool result = this.itemsService.DeleteItem(id);
                if (result)
                {
                    TempData["message"] = "Продуктта беше <strong>изтрита</strong> успешно!";
                    TempData["messageType"] = "warning";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "Неуспешно изтриване!<br/> Моля свържете се с администратор!";
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["message"] = "Несъществуваща продукт!";
                TempData["messageType"] = "danger";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult DeleteImage(int imageId)
        {
            bool result = this.itemsService.DeleteImage(imageId);

            if (result)
            {
                return Content(@"<span class='label label-success'>Изтрито успешно!</span>");
            }
            else
            {
                return Content(@"<span class='label label-danger'>НЕУСПЕШНО!</span>");
            }
            
        }

        private IEnumerable<SelectListItem> GetCategories()
        {
            throw new NotImplementedException();
        }
    }
}