using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class Reservation
    {
        private ICollection<Item> occupiedItems;
        public Reservation()
        {
            this.occupiedItems = new HashSet<Item>();
        }

        public int Id { get; set; }

        public DateTime ArrivalDate { get; set; }

        public DateTime DepartureDate { get; set; }

        public int ItemsCount { get; set; }

        public int Adults { get; set; }

        public int Childs { get; set; }

        // Administration properties:

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
