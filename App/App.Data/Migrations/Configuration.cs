namespace App.Data.Migrations
{
    using App.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        static Random rand = new Random();
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //this.AddInitialItemCategories(context);
            //this.AddInitialItems(context);
        }

        private void AddInitialItemCategories(ApplicationDbContext context)
        {
            if (!context.ItemCategories.Any())
            {
                for (int i = 0; i < 4; i++)
                {
                    var newItemCategory = new ItemCategory
                    {
                        Name = "Item category " + i,
                        DateAdded = DateTime.Now
                    };
                    context.ItemCategories.Add(newItemCategory);
                }
                context.SaveChanges();
            }
        }

        private void AddInitialItems(ApplicationDbContext context)
        {
            if (!context.Items.Any())
            {
                for (int i = 0; i < 10; i++)
                {
                    var newItem = new Item
                    {
                        Name = "Item " + i,
                        Price = rand.Next(30,80),
                        Summary = "Short Description of the item " + i,
                        Description = "Long Description of the item " + i,
                        DateAdded = DateTime.Now,
                        ItemCategoryId = rand.Next(1, 5)
                    };
                    context.Items.Add(newItem);
                }
                context.SaveChanges();
            }
        }
    }
}
