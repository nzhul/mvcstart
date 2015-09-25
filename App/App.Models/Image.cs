using System;

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
