using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class ItemFeature
    {
        private ICollection<Item> items;

        public ItemFeature()
        {
            this.items = new HashSet<Item>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string IconName { get; set; }

        public virtual ICollection<Item> Items
        {
            get { return this.items; }
            set { this.items = value; }
        }
    }
}
