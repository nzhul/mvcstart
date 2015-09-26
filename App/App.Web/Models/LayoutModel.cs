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
        private IEnumerable<ItemCategoryViewModel> itemCategories;
        private IEnumerable<ArticleViewModel> articles;
        private IEnumerable<PageViewModel> pages;

        public LayoutModel()
        {
            this.itemCategories = new List<ItemCategoryViewModel>();
            this.articles = new List<ArticleViewModel>();
            this.pages = new List<PageViewModel>();
        }

        public IEnumerable<PageViewModel> Pages
        {
            get { return this.pages; }
            set { this.pages = value; }
        }

        public IEnumerable<ItemCategoryViewModel> ItemCategories 
        {
            get { return this.itemCategories; }
            set { this.itemCategories = value; }
        }

        public IEnumerable<ArticleViewModel> Articles
        {
            get { return this.articles; }
            set { this.articles = value; }
        }
    }
}