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
        public RoomViewModel MapRoomViewModel(Item room)
        {
            RoomViewModel model = new RoomViewModel();
            model.Id = room.Id;
            model.Name = room.Name;
            model.Price = room.Price;
            model.Summary = room.Summary;
            model.RoomFeature = room.ItemFeatures;
            model.PrimaryImage = room.Images.FirstOrDefault(i => i.IsPrimary);
            model.Images = room.Images;
            model.Description = room.Description;
            model.RoomCategoryId = room.ItemCategoryId;

            return model;
        }

        public ReservationViewModel MapReservationViewModel(Reservation reservation)
        {
            ReservationViewModel model = new ReservationViewModel();
            model.Id = reservation.Id;
            model.ArrivalDate = reservation.ArrivalDate;
            model.DepartureDate = reservation.DepartureDate;
            model.IsConfirmed = reservation.IsConfirmed;
            model.OccupiedRooms = reservation.OccupiedRooms;
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
            model.RoomsCount = dbReservation.RoomsCount;
            model.Adults = dbReservation.Adults;
            model.Childs = dbReservation.Childs;

            return model;
        }
    }
}
