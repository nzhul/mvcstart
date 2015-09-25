using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.ViewModels
{
    public class ItemCategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public int ItemsCount { get; set; }
    }
}
