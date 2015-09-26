using System;
using System.Collections.Generic;

namespace App.Models.ViewModels
{
	public class ReservationViewModel
    {
        private ICollection<Item> occupiedItems;
        public ReservationViewModel()
        {
            this.occupiedItems = new HashSet<Item>();
        }

        public int Id { get; set; }

        public DateTime ArrivalDate { get; set; }

        public DateTime DepartureDate { get; set; }

        public bool IsConfirmed { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Item> OccupiedItems
        {
            get { return this.occupiedItems; }
            set { this.occupiedItems = value; }
        }
    }
}
