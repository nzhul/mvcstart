using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class Room
    {
        private ICollection<Image> images;
        private ICollection<RoomFeature> roomFeatures;
        private ICollection<Reservation> reservations;

        public Room()
        {
            this.images = new HashSet<Image>();
            this.roomFeatures = new HashSet<RoomFeature>();
            this.reservations = new HashSet<Reservation>();
        }

        public int Id { get; set; }

        public int RoomNumber { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }
        
        public int Price { get; set; }

        public bool IsPriceVisible { get; set; }

        public bool IsFeatured { get; set; }

        public DateTime DateAdded { get; set; }

        public int DisplayOrder { get; set; }

        public int RoomCategoryId { get; set; }

        public virtual RoomCategory RoomCategory { get; set; }

        public virtual ICollection<Image> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }

        public virtual ICollection<Reservation> Reservations
        {
            get { return this.reservations; }
            set { this.reservations = value; }
        }

        public virtual ICollection<RoomFeature> RoomFeatures
        {
            get { return this.roomFeatures; }
            set { this.roomFeatures = value; }
        }
    }
}
