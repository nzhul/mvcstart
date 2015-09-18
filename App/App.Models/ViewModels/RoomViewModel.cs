using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.ViewModels
{
    public class RoomViewModel
    {
        private ICollection<RoomFeature> roomFeatures;

        public RoomViewModel()
        {
            this.roomFeatures = new HashSet<RoomFeature>();
        }

        public int Id { get; set; }

        public int RoomNumber { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }
        
        public string Description { get; set; }
        
        public int Price { get; set; }

        public int RoomCategoryId { get; set; }

        public string RoomCategoryName { get; set; }

        public Image PrimaryImage { get; set; }

        public IEnumerable<Image> Images { get; set; }

        public virtual ICollection<RoomFeature> RoomFeature
        {
            get { return this.roomFeatures; }
            set { this.roomFeatures = value; }
        }
    }
}
