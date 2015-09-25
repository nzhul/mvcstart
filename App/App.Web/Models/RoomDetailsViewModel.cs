using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models
{
    public class RoomDetailsViewModel
    {
        public ItemViewModel TheRoom { get; set; }

        public IEnumerable<ItemViewModel> SimilarRooms { get; set; }
    }
}