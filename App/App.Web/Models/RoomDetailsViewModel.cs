using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models
{
    public class ItemDetailsViewModel
    {
        public ItemViewModel TheItem { get; set; }

        public IEnumerable<ItemViewModel> SimilarItems { get; set; }
    }
}