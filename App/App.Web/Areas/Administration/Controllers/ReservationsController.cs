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
    public class ReservationsController : BaseController
    {
        private readonly IReservationsService reservationsService;
        private readonly IUoWData data;

        public ReservationsController()
        {
            this.data = new UoWData();
            this.reservationsService = new ReservationsService(data);
        }

        public ActionResult Index()
        {
            AllReservationsViewModel model = new AllReservationsViewModel();
            model.ActiveReservations = this.reservationsService.GetActiveReservations();
            model.ConfirmedReservations = this.reservationsService.GetConfirmedReservations();
            model.RequestedReservations = this.reservationsService.GetRequestedReservations();
            model.PassedReservations = this.reservationsService.GetPassedReservations();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            CreateReservationInputModel model = new CreateReservationInputModel();
            model.AvailableRooms = this.reservationsService.GetAvailableRooms();
            DateTime date = DateTime.Now;
            model.ArrivalDate = date;
            model.DepartureDate = date.AddDays(1);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateReservationInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                if (inputModel.ArrivalDate > inputModel.DepartureDate)
                {
                    inputModel.AvailableRooms = this.reservationsService.GetAvailableRooms();
                    TempData["message"] = "Невалидни данни за резервацията!<br/> Датата на пристигане трябва да е преди датата на заминаване!";
                    TempData["messageType"] = "danger";
                    return View(inputModel);
                }

                bool isRoomAvailable = this.reservationsService.IsRoomAvailable(inputModel);

                if (isRoomAvailable)
                {
                    bool IsCreateRoomSuccessfull = this.reservationsService.CreateReservation(inputModel);
                    if (IsCreateRoomSuccessfull)
                    {
                        TempData["message"] = "Резервацията беше направена успешно!";
                        TempData["messageType"] = "success";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    inputModel.AvailableRooms = this.reservationsService.GetAvailableRooms();
                    TempData["message"] = "Стаята е заета за посочения период!<br/> Моля изберете друг период!";
                    TempData["messageType"] = "danger";
                    return View(inputModel);
                }


            }

            inputModel.AvailableRooms = this.reservationsService.GetAvailableRooms();
            TempData["message"] = "Невалидни данни за резервацията!<br/> Моля попълнете <strong>всички</strong> задължителни полета!";
            TempData["messageType"] = "danger";
            return View(inputModel);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            CreateReservationInputModel model = new CreateReservationInputModel();

            if (this.reservationsService.ReservationExists(id))
            {
                model = this.reservationsService.GetReservationInputModelById(id);
                model.AvailableRooms = this.reservationsService.GetAvailableRooms();
                model.SelectedRoomIds = this.reservationsService.GetSelectedRoomIds(id);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, CreateReservationInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                 if (inputModel.ArrivalDate > inputModel.DepartureDate)
                {
                    inputModel.AvailableRooms = this.reservationsService.GetAvailableRooms();
                    TempData["message"] = "Невалидни данни за резервацията!<br/> Датата на пристигане трябва да е преди датата на заминаване!";
                    TempData["messageType"] = "danger";
                    return View(inputModel);
                }

                bool IsUpdateSuccessfull = this.reservationsService.UpdateReservation(id, inputModel);
                if (IsUpdateSuccessfull)
                {
                    TempData["message"] = "Резервацията беше редактирана успешно!";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Index");
                }
            }

            inputModel.AvailableRooms = this.reservationsService.GetAvailableRooms();
            inputModel.SelectedRoomIds = this.reservationsService.GetSelectedRoomIds(id);
            TempData["message"] = "Невалидни данни за резервацията!<br/> Моля попълнете <strong>всички</strong> задължителни полета!";
            TempData["messageType"] = "danger";
            return View(inputModel);
        }

        public ActionResult Delete(int id)
        {

            bool isSuccessfull = this.reservationsService.DeleteReservation(id);
            if (isSuccessfull)
            {
                TempData["message"] = "Успешно изтрихте резервацията!";
                TempData["messageType"] = "success";
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult ToggleConfirmation(int reservationId, bool isConfirmed)
        {
            this.reservationsService.ToggleReservationConfirmation(reservationId, isConfirmed);

			if (!isConfirmed)
			{
				this.reservationsService.SendConfirmationEmail(reservationId);
			}

            return RedirectToAction("Index");
        }
    }
}