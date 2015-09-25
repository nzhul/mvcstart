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

        IEnumerable<ItemViewModel> GetAvailableRooms();

        bool CreateReservation(CreateReservationInputModel inputModel);

        IEnumerable<ReservationViewModel> GetActiveReservations();

        IEnumerable<ReservationViewModel> GetConfirmedReservations();

        IEnumerable<ReservationViewModel> GetRequestedReservations();

        IEnumerable<ReservationViewModel> GetPassedReservations();

        bool IsRoomAvailable(CreateReservationInputModel inputModel);

        bool IsRoomAvailable(QuickReservationInputModel input);

        void ToggleReservationConfirmation(int reservationId, bool isReservationConfirmed);

        bool ReservationExists(int id);

        CreateReservationInputModel GetReservationInputModelById(int id);

        List<int> GetSelectedRoomIds(int id);

        bool UpdateReservation(int id, CreateReservationInputModel inputModel);

        bool CreateReservationFromFrontEnd(QuickReservationInputModel input);

        bool DeleteReservation(int id);

		void SendConfirmationEmail(int reservationId);
	}
}
