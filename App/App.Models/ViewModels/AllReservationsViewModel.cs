using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.ViewModels
{
    public class AllReservationsViewModel
    {
        public AllReservationsViewModel()
        {
            this.ActiveReservations = new List<ReservationViewModel>();

            this.ConfirmedReservations = new List<ReservationViewModel>();

            this.RequestedReservations = new List<ReservationViewModel>();

            this.PassedReservations = new List<ReservationViewModel>();
        }

        public IEnumerable<ReservationViewModel> ActiveReservations { get; set; }

        public IEnumerable<ReservationViewModel> ConfirmedReservations { get; set; }

        public IEnumerable<ReservationViewModel> RequestedReservations { get; set; }

        public IEnumerable<ReservationViewModel> PassedReservations { get; set; }
    }
}
