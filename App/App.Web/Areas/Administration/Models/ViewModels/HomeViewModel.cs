using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Areas.Administration.Models.ViewModels
{
	public class HomeViewModel
	{
		private IEnumerable<ItemViewModel> items;

		public HomeViewModel()
		{
			this.items = new List<ItemViewModel>();
		}


		public IEnumerable<ItemViewModel> Items
		{
			get { return this.items; }
			set { this.items = value; }
		}
	}
}