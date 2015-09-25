using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class ItemCategory
    {
        private ICollection<Item> items;
        public ItemCategory()
        {
            this.items = new HashSet<Item>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public int DisplayOrder { get; set; }

        public virtual ICollection<Item> Items
        {
            get { return this.items; }
            set { this.items = value; }
        }
    }
}
