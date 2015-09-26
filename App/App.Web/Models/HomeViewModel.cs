using App.Models;
using App.Models.ViewModels;
using System.Collections.Generic;

namespace App.Web.Models
{
	public class HomeViewModel
	{
		private IEnumerable<ItemFeatureViewModel> itemFeatures;
		private IEnumerable<ArticleViewModel> articles;
		private IEnumerable<ItemViewModel> featuredItems;
		private IEnumerable<Image> galleryImages;

		public HomeViewModel()
		{
			this.itemFeatures = new List<ItemFeatureViewModel>();
			this.Articles = new List<ArticleViewModel>();
			this.featuredItems = new List<ItemViewModel>();
			this.galleryImages = new List<Image>();
		}

		public IEnumerable<ArticleViewModel> Articles
		{
			get { return this.articles; }
			set { this.articles = value; }
		}

		public IEnumerable<ItemFeatureViewModel> ItemFeatures
		{
			get { return this.itemFeatures; }
			set { this.itemFeatures = value; }
		}

		public IEnumerable<ItemViewModel> FeaturedItems
		{
			get { return this.featuredItems; }
			set { this.featuredItems = value; }
		}

		public IEnumerable<Image> GalleryImages
		{
			get { return this.galleryImages; }
			set { this.galleryImages = value; }
		}


		public PageViewModel FeaturedCustomPage { get; set; }
	}
}