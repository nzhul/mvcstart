using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Areas.Administration.Models.ViewModels
{
    public class HomeViewModel
    {
        private IEnumerable<ItemViewModel> availableRooms;
        private IEnumerable<ItemViewModel> occupiedRooms;

        public HomeViewModel()
        {
            this.availableRooms = new List<ItemViewModel>();
            this.occupiedRooms = new List<ItemViewModel>();
        }


        public IEnumerable<ItemViewModel> AvailableRooms
        {
            get { return this.availableRooms; }
            set { this.availableRooms = value; }
        }

        public IEnumerable<ItemViewModel> OccupiedRooms
        {
            get { return this.occupiedRooms; }
            set { this.occupiedRooms = value; }
        }
    }
}