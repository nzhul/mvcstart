using App.Models;
using App.Models.InputModels;
using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Service.Mappers
{
    public class Mapper
    {
        public ItemViewModel MapItemViewModel(Item item)
        {
            ItemViewModel model = new ItemViewModel();
            model.Id = item.Id;
            model.Name = item.Name;
            model.Price = item.Price;
            model.Summary = item.Summary;
            model.ItemFeature = item.ItemFeatures;
            model.PrimaryImage = item.Images.FirstOrDefault(i => i.IsPrimary);
            model.Images = item.Images;
            model.Description = item.Description;
            model.ItemCategoryId = item.ItemCategoryId;

            return model;
        }

        public ReservationViewModel MapReservationViewModel(Reservation reservation)
        {
            ReservationViewModel model = new ReservationViewModel();
            model.Id = reservation.Id;
            model.ArrivalDate = reservation.ArrivalDate;
            model.DepartureDate = reservation.DepartureDate;
            model.IsConfirmed = reservation.IsConfirmed;
            model.OccupiedItems = reservation.OccupiedItems;
            model.FirstName = reservation.FirstName;
            model.LastName = reservation.LastName;
            model.Phone = reservation.Phone;
            model.Email = reservation.Email;

            return model;
        }

        public CreateReservationInputModel MapReservationInputModel(Reservation dbReservation)
        {
            CreateReservationInputModel model = new CreateReservationInputModel();
            model.Id = dbReservation.Id;
            model.ArrivalDate = dbReservation.ArrivalDate;
            model.DepartureDate = dbReservation.DepartureDate;
            model.IsConfirmed = dbReservation.IsConfirmed;
            model.FirstName = dbReservation.FirstName;
            model.LastName = dbReservation.LastName;
            model.Phone = dbReservation.Phone;
            model.Email = dbReservation.Email;
            model.ItemsCount = dbReservation.ItemsCount;
            model.Adults = dbReservation.Adults;
            model.Childs = dbReservation.Childs;

            return model;
        }
    }
}
