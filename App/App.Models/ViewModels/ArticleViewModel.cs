using System;

namespace App.Models.ViewModels
{
	public class ArticleViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Content { get; set; }

        public Image Image { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
