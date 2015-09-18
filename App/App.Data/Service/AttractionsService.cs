using App.Models;
using App.Models.InputModels;
using App.Models.ViewModels;
using ImageResizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Service
{
    public class AttractionsService : IAttractionsService
    {
        private readonly IUoWData Data;

        public AttractionsService(IUoWData data)
        {
            this.Data = data;
        }

        public IEnumerable<AttractionViewModel> GetAttractions()
        {
            return this.Data.Attractions.All().Select(this.MapAttractionViewModel);
        }

        private AttractionViewModel MapAttractionViewModel(Attraction dbAttraction)
        {
            AttractionViewModel model = new AttractionViewModel();
            model.Id = dbAttraction.Id;
            model.Title = dbAttraction.Title;
            model.Image = dbAttraction.Image;
            model.DateAdded = dbAttraction.DateAdded;
            model.Summary = dbAttraction.Summary;
            model.Content = dbAttraction.Content;

            return model;
        }


        public int CreateAttraction(CreateAttractionInputModel inputModel)
        {
            Attraction newAttraction = new Attraction();
            newAttraction.Title = inputModel.Title;
            newAttraction.Summary = inputModel.Summary;
            newAttraction.Content = inputModel.Content;
            newAttraction.DateAdded = DateTime.Now;
            newAttraction.DisplayOrder = inputModel.DisplayOrder;

            this.Data.Attractions.Add(newAttraction);
            this.Data.SaveChanges();

            Image defaultImage = new Image
            {
                ImageExtension = "jpg",
                ImagePath = "Content\\images\\noimage\\no-image",
                IsPrimary = true,
                DateAdded = DateTime.Now
            };

            newAttraction.Image = defaultImage;
            this.Data.SaveChanges();

            return newAttraction.Id;
        }

        public bool AttractionExists(int id)
        {
            if (id <= 0)
            {
                return false;
            }
            else
            {
                bool result = this.Data.Attractions.All().Any(r => r.Id == id);
                return result;
            }
        }

        public CreateAttractionInputModel GetAttractionInputModelById(int id)
        {
            Attraction dbAttraction = this.Data.Attractions.Find(id);
            return MapPageInputModel(dbAttraction);
        }

        private CreateAttractionInputModel MapPageInputModel(Attraction dbAttraction)
        {
            CreateAttractionInputModel model = new CreateAttractionInputModel();
            model.Id = dbAttraction.Id;
            model.Title = dbAttraction.Title;
            model.Summary = dbAttraction.Summary;
            model.Content = dbAttraction.Content;
            model.DisplayOrder = dbAttraction.DisplayOrder;
            model.Image = dbAttraction.Image;

            return model;
        }

        public bool UpdateAttraction(int id, CreateAttractionInputModel inputModel)
        {
            Attraction dbAttraction = this.Data.Attractions.Find(id);
            if (dbAttraction != null)
            {
                dbAttraction.Title = inputModel.Title;
                dbAttraction.Summary = inputModel.Summary;
                dbAttraction.Content = inputModel.Content;
                dbAttraction.DisplayOrder = inputModel.DisplayOrder;

                this.Data.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteAttraction(int id)
        {
            // TODO - Also delete the images for that attraction

            var theAttraction = this.Data.Attractions.Find(id);
            if (theAttraction == null)
            {
                return false;
            }

            this.Data.Attractions.Delete(id);
            this.Data.SaveChanges();

            return true;
        }


        public AttractionViewModel GetAttractionById(int id)
        {
            AttractionViewModel model = new AttractionViewModel();
            if (this.AttractionExists(id))
            {
                Attraction dbAttraction = this.Data.Attractions.Find(id);
                model = this.MapAttractionViewModel(dbAttraction);
            }
            else
            {
                model.Content = "Не съществува такава атракция!";
                model.Title = "НЕСЪЩЕСТВУВАЩА АТКРАКЦИЯ!";
            }

            return model;
        }
    }
}
