using App.Data.Repositories;
using App.Models;
using App.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data
{
    public interface IUoWData
    {
        IRepository<ApplicationUser> Users { get; }
        IRepository<Item> Items { get; }
        IRepository<ItemCategory> ItemCategories { get; }
        IRepository<ItemFeature> ItemFeatures { get; }
        IRepository<Image> Images { get; }
        IRepository<Article> Articles { get; }
        IRepository<Reservation> Reservations { get; }
        IRepository<Page> Pages { get; }

        int SaveChanges();
    }
}
