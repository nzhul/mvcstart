using App.Data.Service.Mappers;
using App.Models;
using App.Models.InputModels;
using App.Models.InputModels.FrontEnd;
using App.Models.ViewModels;
using Itenso.TimePeriod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Service
{
    public class ReservationsService : IReservationsService
    {
        private readonly IUoWData Data;
        private readonly Mapper Mapper;

        public ReservationsService(IUoWData data)
        {
            this.Data = data;
            this.Mapper = new Mapper();
        }



        public IEnumerable<ItemViewModel> GetAvailableRooms()
        {
            return this.Data.Items.All().Select(this.Mapper.MapRoomViewModel);
        }


        public bool CreateReservation(CreateReservationInputModel inputModel)
        {
            Reservation newReservation = new Reservation();
            newReservation.ArrivalDate = inputModel.ArrivalDate;
            newReservation.DepartureDate = inputModel.DepartureDate;
            newReservation.RoomsCount = inputModel.RoomsCount;
            newReservation.Adults = inputModel.Adults;
            newReservation.Childs = inputModel.Childs;
            newReservation.IsConfirmed = inputModel.IsConfirmed;
            newReservation.FirstName = inputModel.FirstName;
            newReservation.LastName = inputModel.LastName;
            newReservation.Phone = inputModel.Phone;
            newReservation.Email = inputModel.Email;

            if (inputModel.SelectedRoomIds != null && inputModel.SelectedRoomIds.Count > 0)
            {
                for (int i = 0; i < inputModel.SelectedRoomIds.Count; i++)
                {
                    Item theRoom = this.Data.Items.Find(inputModel.SelectedRoomIds[i]);
                    newReservation.OccupiedRooms.Add(theRoom);
                    this.Data.SaveChanges();
                }
            }

            this.Data.Reservations.Add(newReservation);
            this.Data.SaveChanges();

            return true;
        }


        public IEnumerable<ReservationViewModel> GetActiveReservations()
        {
            return this.Data.Reservations.All()
                .Where(r => r.IsConfirmed == true && (DateTime.Now > r.ArrivalDate && DateTime.Now < r.DepartureDate))
                .OrderBy(r => r.DepartureDate)
                .Select(this.Mapper.MapReservationViewModel);
        }

        public IEnumerable<ReservationViewModel> GetConfirmedReservations()
        {
            return this.Data.Reservations.All()
                .Where(r => r.IsConfirmed == true && DateTime.Now < r.ArrivalDate)
                .OrderBy(r => r.ArrivalDate)
                .Select(this.Mapper.MapReservationViewModel);
        }

        public IEnumerable<ReservationViewModel> GetRequestedReservations()
        {
            return this.Data.Reservations.All()
                .Where(r => r.IsConfirmed == false)
                .OrderBy(r => r.ArrivalDate)
                .Select(this.Mapper.MapReservationViewModel);
        }

        public IEnumerable<ReservationViewModel> GetPassedReservations()
        {
            return this.Data.Reservations.All()
                .Where(r => r.IsConfirmed == true && DateTime.Now > r.DepartureDate)
                .OrderByDescending(r => r.DepartureDate)
                .Select(this.Mapper.MapReservationViewModel);
        }


        public bool IsRoomAvailable(CreateReservationInputModel inputModel)
        {
            foreach (var roomId in inputModel.SelectedRoomIds)
            {
                Item currentSelectedRoom = this.Data.Items.Find(roomId);
                IEnumerable<Reservation> reservationsWithThatRoom = this.Data.Reservations.All().Where(r => r.OccupiedRooms.Select(or => or.Id).Contains(currentSelectedRoom.Id));

                if (reservationsWithThatRoom.Count() > 0)
                {
                    foreach (var reservation in reservationsWithThatRoom)
                    {
                        TimeRange inputTimeRange = new TimeRange(inputModel.ArrivalDate, inputModel.DepartureDate);
                        TimeRange dbTimeRange = new TimeRange(reservation.ArrivalDate, reservation.DepartureDate);

                        if (inputTimeRange.OverlapsWith(dbTimeRange))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }


        public void ToggleReservationConfirmation(int reservationId, bool isReservationConfirmed)
        {
            Reservation dbReservation = this.Data.Reservations.Find(reservationId);

            if (isReservationConfirmed)
            {
                dbReservation.IsConfirmed = false;
            }
            else
            {
                dbReservation.IsConfirmed = true;
            }

            this.Data.SaveChanges();
        }


        public bool ReservationExists(int id)
        {
            if (id <= 0)
            {
                return false;
            }
            else
            {
                bool result = this.Data.Reservations.All().Any(r => r.Id == id);
                return result;
            }
        }

        public CreateReservationInputModel GetReservationInputModelById(int id)
        {
            Reservation dbReservation = this.Data.Reservations.Find(id);
            return this.Mapper.MapReservationInputModel(dbReservation);
        }


        public List<int> GetSelectedRoomIds(int id)
        {
            Reservation dbReservation = this.Data.Reservations.Find(id);

            List<int> selectedRoomIds = new List<int>();

            foreach (var room in dbReservation.OccupiedRooms)
            {
                selectedRoomIds.Add(room.Id);
            }

            return selectedRoomIds;
        }


        public bool UpdateReservation(int id, CreateReservationInputModel inputModel)
        {
            Reservation dbReservation = this.Data.Reservations.Find(id);
            if (dbReservation != null)
            {
                dbReservation.Adults = inputModel.Adults;
                dbReservation.ArrivalDate = inputModel.ArrivalDate;
                dbReservation.Childs = inputModel.Childs;
                dbReservation.FirstName = inputModel.FirstName;
                dbReservation.LastName = inputModel.LastName;
                dbReservation.Phone = inputModel.Phone;
                dbReservation.Email = inputModel.Email;
                dbReservation.DepartureDate = inputModel.DepartureDate;
                dbReservation.IsConfirmed = inputModel.IsConfirmed;
                dbReservation.RoomsCount = inputModel.RoomsCount;

                foreach (var room in dbReservation.OccupiedRooms.ToList())
                {
                    dbReservation.OccupiedRooms.Remove(room);
                }
                this.Data.SaveChanges();

                if (inputModel.SelectedRoomIds != null && inputModel.SelectedRoomIds.Count > 0)
                {
                    for (int i = 0; i < inputModel.SelectedRoomIds.Count; i++)
                    {
                        var theRoom = this.Data.Items.Find(inputModel.SelectedRoomIds[i]);
                        dbReservation.OccupiedRooms.Add(theRoom);
                    }
                }

                this.Data.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }


        public bool IsRoomAvailable(QuickReservationInputModel inputModel)
        {
            Item currentSelectedRoom = this.Data.Items.Find(inputModel.RoomId);
            IEnumerable<Reservation> approvedReservationsWithThatRoom = this.Data.Reservations.All().Where(r => r.OccupiedRooms.Select(or => or.Id).Contains(currentSelectedRoom.Id) && r.IsConfirmed == true);

            if (approvedReservationsWithThatRoom.Count() > 0)
            {
                foreach (var reservation in approvedReservationsWithThatRoom)
                {
                    TimeRange inputTimeRange = new TimeRange(inputModel.ArrivalDate, inputModel.DepartureDate);
                    TimeRange dbTimeRange = new TimeRange(reservation.ArrivalDate, reservation.DepartureDate);

                    if (inputTimeRange.OverlapsWith(dbTimeRange))
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        public bool CreateReservationFromFrontEnd(QuickReservationInputModel inputModel)
        {
            Reservation newReservation = new Reservation();
            newReservation.ArrivalDate = inputModel.ArrivalDate;
            newReservation.DepartureDate = inputModel.DepartureDate;
            newReservation.Adults = inputModel.Adults;
            newReservation.Childs = inputModel.Childrens;
            newReservation.IsConfirmed = false;
            newReservation.FirstName = inputModel.FirstName;
            newReservation.LastName = inputModel.LastName;
            newReservation.Phone = inputModel.Phone;
            newReservation.Email = inputModel.Email;


            Item theRoom = this.Data.Items.Find(inputModel.RoomId);
            newReservation.OccupiedRooms.Add(theRoom);
            this.Data.SaveChanges();

            this.Data.Reservations.Add(newReservation);
            this.Data.SaveChanges();

            string sender = "system@dabravata.com";
            string receiver = "manager@dabravata.com";

            MailMessage mailMessage = new MailMessage(sender, receiver);
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = "Запитване за резервация: " + theRoom.Name;
            mailMessage.Body = "Име: " + (newReservation.FirstName != null ? newReservation.FirstName : "--липсва--") + "<br/>" +
                               "Фамилия: " + (newReservation.LastName != null ? newReservation.LastName : "--липсва--") + "<br/>" +
                               "Email: " + (newReservation.Email != null ? newReservation.Email : "--липсва--") + "<br/>" +
                               "Телефон: " + (newReservation.Phone != null ? newReservation.Phone : "--липсва--") + "<br/><br/>" +
                               "Настаняване: " + (newReservation.Phone != null ? newReservation.ArrivalDate.ToShortDateString() : "--липсва--") + "<br/><br/>" +
                               "Освобождаване: " + (newReservation.Phone != null ? newReservation.DepartureDate.ToShortDateString() : "--липсва--") + "<br/><br/>" +
                               "Стая: " + theRoom.Name;

            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Send(mailMessage);

            return true;
        }


        public bool DeleteReservation(int id)
        {
            Reservation theReservation = this.Data.Reservations.Find(id);
            if (theReservation == null)
            {
                return false;
            }

            this.Data.Reservations.Delete(id);
            this.Data.SaveChanges();

            return true;
        }


		public void SendConfirmationEmail(int reservationId)
		{
			Reservation dbReservation = this.Data.Reservations.Find(reservationId);

			if (!string.IsNullOrWhiteSpace(dbReservation.Email))
			{
				string sender = "manager@dabravata.com";
				string receiver = dbReservation.Email;

				MailMessage mailMessage = new MailMessage(sender, receiver);
				mailMessage.IsBodyHtml = true;
				mailMessage.Subject = "Къща за гости Дъбравата - Потвърдена резервация";
				mailMessage.Body = "Здравейте,<br/>Вашата резервация беше успешно потвърдена от наш администратор!<br/>Желаем ви приятна почивка!<br/><br/> Телефон за контакт: +35988 999 9887";

				SmtpClient smtpClient = new SmtpClient();

				smtpClient.Send(mailMessage);
			}
		}
	}
}
