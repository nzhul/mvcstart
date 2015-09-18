using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Data.Repositories;
using App.Models;
using System.Data.Entity;
using App.Models.Pages;

namespace App.Data
{
    public class UoWData : IUoWData
    {
        private DbContext context;
        private IDictionary<Type, object> repositories;

        public UoWData()
            : this(new ApplicationDbContext())
        {
        }

        public UoWData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<ApplicationUser> Users
        {
            get { return this.GetRepository<ApplicationUser>(); }
        }

        public IRepository<Room> Rooms
        {
            get { return this.GetRepository<Room>(); }
        }

        public IRepository<RoomCategory> RoomCategories
        {
            get { return this.GetRepository<RoomCategory>(); }
        }

        public IRepository<RoomFeature> RoomFeatures
        {
            get { return this.GetRepository<RoomFeature>(); }
        }

        public IRepository<Image> Images
        {
            get { return this.GetRepository<Image>(); }
        }

        public IRepository<Attraction> Attractions
        {
            get { return this.GetRepository<Attraction>(); }
        }

        public IRepository<Reservation> Reservations
        {
            get { return this.GetRepository<Reservation>(); }
        }

        public IRepository<Page> Pages
        {
            get { return this.GetRepository<Page>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfRepository = typeof(T);
            if (!this.repositories.ContainsKey(typeOfRepository))
            {
                var newRepository = Activator.CreateInstance(typeof(EFRepository<T>), context);
                this.repositories.Add(typeOfRepository, newRepository);
            }

            return (IRepository<T>)this.repositories[typeOfRepository];
        }
    }
}
