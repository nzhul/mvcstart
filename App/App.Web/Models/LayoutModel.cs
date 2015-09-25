using App.Models;
using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models
{
    public class LayoutModel
    {
        private IEnumerable<ItemCategoryViewModel> roomCategories;
        private IEnumerable<ArticleViewModel> attractions;
        private IEnumerable<PageViewModel> pages;

        public LayoutModel()
        {
            this.roomCategories = new List<ItemCategoryViewModel>();
            this.attractions = new List<ArticleViewModel>();
            this.pages = new List<PageViewModel>();
        }

        public IEnumerable<PageViewModel> Pages
        {
            get { return this.pages; }
            set { this.pages = value; }
        }

        public IEnumerable<ItemCategoryViewModel> RoomCategories 
        {
            get { return this.roomCategories; }
            set { this.roomCategories = value; }
        }

        public IEnumerable<ArticleViewModel> Attractions
        {
            get { return this.attractions; }
            set { this.attractions = value; }
        }
    }
}