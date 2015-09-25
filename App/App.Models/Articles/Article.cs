using System;

namespace App.Models
{
    public class Article
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Content { get; set; }

        public virtual Image Image { get; set; }

        public DateTime DateAdded { get; set; }

        public int DisplayOrder { get; set; }
    }
}