using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class RoomFeature
    {
        private ICollection<Room> rooms;

        public RoomFeature()
        {
            this.rooms = new HashSet<Room>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string IconName { get; set; }

        public virtual ICollection<Room> Rooms
        {
            get { return this.rooms; }
            set { this.rooms = value; }
        }
    }
}
