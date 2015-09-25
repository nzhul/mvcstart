﻿using App.Data;
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
    public class RoomsController : BaseController
    {
        private readonly IItemsService roomsService;
        private readonly IImagesService imagesService;
        private readonly IUoWData uoWData;
        public RoomsController()
        {
            this.uoWData = new UoWData();
            this.roomsService = new ItemsService(this.uoWData);
            this.imagesService = new ImagesService(this.uoWData);
        }

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<ItemViewModel> roomsCollection = this.roomsService.GetRooms(null);

            HomeViewModel homeModel = new HomeViewModel();
            homeModel.AvailableRooms = roomsCollection;
            homeModel.OccupiedRooms = roomsCollection;

            return View(homeModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            CreateItemInputModel model = new CreateItemInputModel();
            model.Categories = this.roomsService.GetCategories();
            model.AvailableRoomFeatures = this.roomsService.GetAvailableRoomFeatures();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateItemInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                bool IsCreateRoomSuccessfull = this.roomsService.CreateRoom(inputModel);
                if (IsCreateRoomSuccessfull)
                {
                    TempData["message"] = "Стаята беше добавена успешно!";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Index");
                }
            }

            inputModel.Categories = this.roomsService.GetCategories();
            TempData["message"] = "Невалидни данни за стаята!<br/> Моля попълнете <strong>всички</strong> задължителни полета!";
            TempData["messageType"] = "danger";
            return View(inputModel);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            CreateItemInputModel model = new CreateItemInputModel();

            if (this.roomsService.RoomExists(id))
            {
                model = this.roomsService.GetItemInputModelById(id);
                model.Categories = this.roomsService.GetCategories();
                model.AvailableRoomFeatures = this.roomsService.GetAvailableRoomFeatures();
                model.SelectedRoomFeatureIds = this.roomsService.GetSelectedRoomFeatureIds(id);
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
                bool IsUpdateSuccessfull = this.roomsService.UpdateRoom(id, inputModel);
                if (IsUpdateSuccessfull)
                {
                    TempData["message"] = "Стаята беше редактирана успешно!";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Index");
                }
            }

            inputModel.Categories = this.roomsService.GetCategories();
            inputModel.AvailableRoomFeatures = this.roomsService.GetAvailableRoomFeatures();
            TempData["message"] = "Невалидни данни за стаята!<br/> Моля попълнете <strong>всички</strong> задължителни полета!";
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
            bool roomExists = this.roomsService.RoomExists(id);
            if (roomExists)
            {
                bool result = this.roomsService.DeleteRoom(id);
                if (result)
                {
                    TempData["message"] = "Стаята беше <strong>изтрита</strong> успешно!";
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
                TempData["message"] = "Несъществуваща стая!";
                TempData["messageType"] = "danger";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult DeleteImage(int imageId)
        {
            bool result = this.roomsService.DeleteImage(imageId);

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