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
            //this.AddInitialRoomCategories(context);
            //this.AddInitialRooms(context);
        }

        private void AddInitialRoomCategories(ApplicationDbContext context)
        {
            if (!context.RoomCategories.Any())
            {
                for (int i = 0; i < 4; i++)
                {
                    var newRoomCategory = new ItemCategory
                    {
                        Name = "Room category " + i,
                        DateAdded = DateTime.Now
                    };
                    context.RoomCategories.Add(newRoomCategory);
                }
                context.SaveChanges();
            }
        }

        private void AddInitialRooms(ApplicationDbContext context)
        {
            if (!context.Rooms.Any())
            {
                for (int i = 0; i < 10; i++)
                {
                    var newRoom = new Item
                    {
                        Name = "Room " + i,
                        Price = rand.Next(30,80),
                        Summary = "Short Description of the room " + i,
                        Description = "Long Description of the room " + i,
                        DateAdded = DateTime.Now,
                        ItemCategoryId = rand.Next(1, 5)
                    };
                    context.Rooms.Add(newRoom);
                }
                context.SaveChanges();
            }
        }
    }
}
