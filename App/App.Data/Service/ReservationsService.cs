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



        public IEnumerable<ItemViewModel> GetAvailableItems()
        {
            return this.Data.Items.All().Select(this.Mapper.MapItemViewModel);
        }


        public bool CreateReservation(CreateReservationInputModel inputModel)
        {
            Reservation newReservation = new Reservation();
            newReservation.ArrivalDate = inputModel.ArrivalDate;
            newReservation.DepartureDate = inputModel.DepartureDate;
            newReservation.ItemsCount = inputModel.ItemsCount;
            newReservation.Adults = inputModel.Adults;
            newReservation.Childs = inputModel.Childs;
            newReservation.IsConfirmed = inputModel.IsConfirmed;
            newReservation.FirstName = inputModel.FirstName;
            newReservation.LastName = inputModel.LastName;
            newReservation.Phone = inputModel.Phone;
            newReservation.Email = inputModel.Email;

            if (inputModel.SelectedItemIds != null && inputModel.SelectedItemIds.Count > 0)
            {
                for (int i = 0; i < inputModel.SelectedItemIds.Count; i++)
                {
                    Item theItem = this.Data.Items.Find(inputModel.SelectedItemIds[i]);
                    newReservation.OccupiedItems.Add(theItem);
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


        public bool IsItemAvailable(CreateReservationInputModel inputModel)
        {
            foreach (var itemId in inputModel.SelectedItemIds)
            {
                Item currentSelectedItem = this.Data.Items.Find(itemId);
                IEnumerable<Reservation> reservationsWithThatItem = this.Data.Reservations.All().Where(r => r.OccupiedItems.Select(or => or.Id).Contains(currentSelectedItem.Id));

                if (reservationsWithThatItem.Count() > 0)
                {
                    foreach (var reservation in reservationsWithThatItem)
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


        public List<int> GetSelectedItemIds(int id)
        {
            Reservation dbReservation = this.Data.Reservations.Find(id);

            List<int> selectedItemIds = new List<int>();

            foreach (var item in dbReservation.OccupiedItems)
            {
                selectedItemIds.Add(item.Id);
            }

            return selectedItemIds;
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
                dbReservation.ItemsCount = inputModel.ItemsCount;

                foreach (var item in dbReservation.OccupiedItems.ToList())
                {
                    dbReservation.OccupiedItems.Remove(item);
                }
                this.Data.SaveChanges();

                if (inputModel.SelectedItemIds != null && inputModel.SelectedItemIds.Count > 0)
                {
                    for (int i = 0; i < inputModel.SelectedItemIds.Count; i++)
                    {
                        var theItem = this.Data.Items.Find(inputModel.SelectedItemIds[i]);
                        dbReservation.OccupiedItems.Add(theItem);
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


        public bool IsItemAvailable(QuickReservationInputModel inputModel)
        {
            Item currentSelectedItem = this.Data.Items.Find(inputModel.ItemId);
            IEnumerable<Reservation> approvedReservationsWithThatItem = this.Data.Reservations.All().Where(r => r.OccupiedItems.Select(or => or.Id).Contains(currentSelectedItem.Id) && r.IsConfirmed == true);

            if (approvedReservationsWithThatItem.Count() > 0)
            {
                foreach (var reservation in approvedReservationsWithThatItem)
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


            Item theItem = this.Data.Items.Find(inputModel.ItemId);
            newReservation.OccupiedItems.Add(theItem);
            this.Data.SaveChanges();

            this.Data.Reservations.Add(newReservation);
            this.Data.SaveChanges();

            string sender = "system@dabravata.com";
            string receiver = "manager@dabravata.com";

            MailMessage mailMessage = new MailMessage(sender, receiver);
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = "Запитване за резервация: " + theItem.Name;
            mailMessage.Body = "Име: " + (newReservation.FirstName != null ? newReservation.FirstName : "--липсва--") + "<br/>" +
                               "Фамилия: " + (newReservation.LastName != null ? newReservation.LastName : "--липсва--") + "<br/>" +
                               "Email: " + (newReservation.Email != null ? newReservation.Email : "--липсва--") + "<br/>" +
                               "Телефон: " + (newReservation.Phone != null ? newReservation.Phone : "--липсва--") + "<br/><br/>" +
                               "Настаняване: " + (newReservation.Phone != null ? newReservation.ArrivalDate.ToShortDateString() : "--липсва--") + "<br/><br/>" +
                               "Освобождаване: " + (newReservation.Phone != null ? newReservation.DepartureDate.ToShortDateString() : "--липсва--") + "<br/><br/>" +
                               "Продукт: " + theItem.Name;

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
