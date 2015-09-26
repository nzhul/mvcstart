using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class Item
    {
        private ICollection<Image> images;
        private ICollection<ItemFeature> itemFeatures;
		private ICollection<Reservation> reservations;

		public Item()
        {
            this.images = new HashSet<Image>();
            this.itemFeatures = new HashSet<ItemFeature>();
			this.reservations = new HashSet<Reservation>();
		}

        public int Id { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }
        
        public int Price { get; set; }

        public bool IsPriceVisible { get; set; }

        public bool IsFeatured { get; set; }

        public DateTime DateAdded { get; set; }

        public int DisplayOrder { get; set; }

        public int ItemCategoryId { get; set; }

        public virtual ItemCategory ItemCategory { get; set; }

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

		public virtual ICollection<ItemFeature> ItemFeatures
        {
            get { return this.itemFeatures; }
            set { this.itemFeatures = value; }
        }
    }
}
