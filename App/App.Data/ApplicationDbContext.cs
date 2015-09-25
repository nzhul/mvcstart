using App.Data.Migrations;
using App.Models;
using App.Models.Pages;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace App.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public IDbSet<Item> Items { get; set; }
        public IDbSet<ItemCategory> ItemCategories { get; set; }
        public IDbSet<ItemFeature> ItemFeatures { get; set; }
        public IDbSet<Image> Images { get; set; }
        public IDbSet<Article> Articles { get; set; }
        public IDbSet<Reservation> Reservations { get; set; }
        public IDbSet<Page> Pages { get; set; }
    }
}
