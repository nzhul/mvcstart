using App.Models;
using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models
{
    public class HomeViewModel
    {
        private IEnumerable<ItemFeatureViewModel> roomFeatures;
        private IEnumerable<ArticleViewModel> attractions;
        private IEnumerable<ItemViewModel> featuredRooms;
        private IEnumerable<Image> galleryImages;

        public HomeViewModel()
        {
            this.roomFeatures = new List<ItemFeatureViewModel>();
            this.attractions = new List<ArticleViewModel>();
            this.featuredRooms = new List<ItemViewModel>();
            this.galleryImages = new List<Image>();
        }

        public IEnumerable<ArticleViewModel> Attractions
        {
            get { return this.attractions; }
            set { this.attractions = value; }
        }

        public IEnumerable<ItemFeatureViewModel> RoomFeatures
        {
            get { return this.roomFeatures; }
            set { this.roomFeatures = value; }
        }

        public IEnumerable<ItemViewModel> FeaturedRooms
        {
            get { return this.featuredRooms; }
            set { this.featuredRooms = value; }
        }

        public IEnumerable<Image> GalleryImages
        {
            get { return this.galleryImages; }
            set { this.galleryImages = value; }
        }


        public PageViewModel FeaturedCustomPage { get; set; }
    }
}