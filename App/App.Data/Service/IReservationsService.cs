using App.Models.InputModels;
using App.Models.InputModels.FrontEnd;
using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Service
{
    public interface IReservationsService
    {

        IEnumerable<ItemViewModel> GetAvailableItems();

        bool CreateReservation(CreateReservationInputModel inputModel);

        IEnumerable<ReservationViewModel> GetActiveReservations();

        IEnumerable<ReservationViewModel> GetConfirmedReservations();

        IEnumerable<ReservationViewModel> GetRequestedReservations();

        IEnumerable<ReservationViewModel> GetPassedReservations();

        bool IsItemAvailable(CreateReservationInputModel inputModel);

        bool IsItemAvailable(QuickReservationInputModel input);

        void ToggleReservationConfirmation(int reservationId, bool isReservationConfirmed);

        bool ReservationExists(int id);

        CreateReservationInputModel GetReservationInputModelById(int id);

        List<int> GetSelectedItemIds(int id);

        bool UpdateReservation(int id, CreateReservationInputModel inputModel);

        bool CreateReservationFromFrontEnd(QuickReservationInputModel input);

        bool DeleteReservation(int id);

		void SendConfirmationEmail(int reservationId);
	}
}
