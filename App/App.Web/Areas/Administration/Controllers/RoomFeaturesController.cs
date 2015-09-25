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
    public class RoomFeaturesController : BaseController
    {
        private readonly IItemsService roomsService;
        private readonly IUoWData uoWData;
        public RoomFeaturesController()
        {
            this.uoWData = new UoWData();
            this.roomsService = new ItemsService(this.uoWData);
        }


        public ActionResult Index()
        {
            IEnumerable<ItemFeatureViewModel> model = this.roomsService.GetRoomFeatures();
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
                int result = this.roomsService.CreateRoomFeature(featureInput);
                if (result > 0)
                {
                    TempData["message"] = "Успешно добавихте ново удобство!";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Index");
                }
            }

            TempData["message"] = "Невалидни данни за удобството!<br/> Моля попълнете <strong>всички</strong> задължителни полета!";
            TempData["messageType"] = "danger";
            return View(featureInput);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            CreateItemFeatureInputModel model = this.roomsService.GetRoomFeatureInputModelById(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, CreateItemFeatureInputModel roomFeature)
        {
            if (ModelState.IsValid)
            {
                bool result = this.roomsService.UpdateRoomFeature(id, roomFeature);

                if (result == true)
                {
                    TempData["message"] = "Редактирахте успешно удобството!";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Index");
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }

            TempData["message"] = "Невалидни данни за удобството!<br/> Моля попълнете <strong>всички</strong> полета в червено!";
            TempData["messageType"] = "danger";
            return View(roomFeature);
        }

        public ActionResult Delete(int id)
        {

            bool isSuccessfull = this.roomsService.DeleteRoomFeature(id);
            if (isSuccessfull)
            {
                TempData["message"] = "Успешно изтрихте удобството!";
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