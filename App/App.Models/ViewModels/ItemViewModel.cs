using System.Collections.Generic;

namespace App.Models.ViewModels
{
	public class ItemViewModel
    {
        private ICollection<ItemFeature> itemFeatures;

        public ItemViewModel()
        {
            this.itemFeatures = new HashSet<ItemFeature>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }
        
        public string Description { get; set; }
        
        public int Price { get; set; }

        public int ItemCategoryId { get; set; }

        public string ItemCategoryName { get; set; }

        public Image PrimaryImage { get; set; }

        public IEnumerable<Image> Images { get; set; }

        public virtual ICollection<ItemFeature> ItemFeature
        {
            get { return this.itemFeatures; }
            set { this.itemFeatures = value; }
        }
    }
}