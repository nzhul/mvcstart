using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class Image
    {
        public int Id { get; set; }

        public string ImagePath { get; set; }

        public string ImageExtension { get; set; }

        public bool IsPrimary { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsGalleryImage { get; set; }
    }
}
