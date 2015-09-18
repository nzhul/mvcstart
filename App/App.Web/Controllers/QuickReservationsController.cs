using App.Data;
using App.Data.Service;
using App.Models.InputModels.FrontEnd;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers
{
    public class QuickReservationsController : Controller
    {
        private readonly IUoWData data;
        private readonly IReservationsService reservationsService;

        public QuickReservationsController()
        {
            this.data = new UoWData();
            this.reservationsService = new ReservationsService(this.data);
        }

        public JsonResult GetInitialRoomCategories()
        {
            var roomCategories = this.data.RoomCategories
                .All()
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();


            return Json(new SelectList(roomCategories, "Value", "Text"));
        }

        public JsonResult GetInitialRooms()
        {
            var rooms = this.data.Rooms
                .All()
                .OrderBy(x => x.DisplayOrder)
                .AsEnumerable()
                .Select(x => new SelectListItem
                {
                    Text = string.Format("{0}: {1}", x.RoomNumber, x.Name),
                    Value = x.Id.ToString()
                }).ToList();


            return Json(new SelectList(rooms, "Value", "Text"));
        }



        public JsonResult GetRoomsFromSelectedCategory(string id)
        {
            var categoryId = int.Parse(id);
            var subcategories = this.data.Rooms
                .All()
                .OrderBy(x => x.DisplayOrder)
                .Where(x => x.RoomCategoryId == categoryId)
                .AsEnumerable()
                .Select(x => new SelectListItem
                {
                    Text = string.Format("{0}: {1}", x.RoomNumber, x.Name),
                    Value = x.Id.ToString()
                }).ToList();


            return Json(new SelectList(subcategories, "Value", "Text"));
        }

        public JsonResult CheckRoomAvailability(QuickReservationInputModel input)
        {
            if (input.ArrivalDate == DateTime.MinValue || input.DepartureDate == DateTime.MinValue)
            {
                return Json(false);
            }
            else
            {
                bool isRoomAvailable = this.reservationsService.IsRoomAvailable(input);

                return Json(isRoomAvailable);
            }
        }

        public JsonResult ConfirmReservation(QuickReservationInputModel input)
        {
            if (ModelState.IsValid)
            {
                bool IsCreationSuccessfull = this.reservationsService.CreateReservationFromFrontEnd(input);

                if (IsCreationSuccessfull)
                {
                    return Json(new { status = "success" });
                }
                else
                {
                    return Json(new { status = "errorCreatingReservation" });
                }
            }
            else
            {
                return Json(new { status = "invalidModel" });
            }
        }
    }
}