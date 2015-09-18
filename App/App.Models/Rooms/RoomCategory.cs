using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class RoomCategory
    {
        private ICollection<Room> rooms;
        public RoomCategory()
        {
            this.rooms = new HashSet<Room>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public int DisplayOrder { get; set; }

        public virtual ICollection<Room> Rooms
        {
            get { return this.rooms; }
            set { this.rooms = value; }
        }
    }
}
