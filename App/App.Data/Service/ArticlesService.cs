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
    public class ArticlesService : IArticlesService
    {
        private readonly IUoWData Data;

        public ArticlesService(IUoWData data)
        {
            this.Data = data;
        }

        public IEnumerable<ArticleViewModel> GetArticles()
        {
            return this.Data.Articles.All().Select(this.MapArticleViewModel);
        }

        private ArticleViewModel MapArticleViewModel(Article dbArticle)
        {
            ArticleViewModel model = new ArticleViewModel();
            model.Id = dbArticle.Id;
            model.Title = dbArticle.Title;
            model.Image = dbArticle.Image;
            model.DateAdded = dbArticle.DateAdded;
            model.Summary = dbArticle.Summary;
            model.Content = dbArticle.Content;

            return model;
        }


        public int CreateArticle(CreateArticleInputModel inputModel)
        {
            Article newAttraction = new Article();
            newAttraction.Title = inputModel.Title;
            newAttraction.Summary = inputModel.Summary;
            newAttraction.Content = inputModel.Content;
            newAttraction.DateAdded = DateTime.Now;
            newAttraction.DisplayOrder = inputModel.DisplayOrder;

            this.Data.Articles.Add(newAttraction);
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

        public bool ArticleExists(int id)
        {
            if (id <= 0)
            {
                return false;
            }
            else
            {
                bool result = this.Data.Articles.All().Any(r => r.Id == id);
                return result;
            }
        }

        public CreateArticleInputModel GetArticleInputModelById(int id)
        {
            Article dbAttraction = this.Data.Articles.Find(id);
            return MapPageInputModel(dbAttraction);
        }

        private CreateArticleInputModel MapPageInputModel(Article dbAttraction)
        {
            CreateArticleInputModel model = new CreateArticleInputModel();
            model.Id = dbAttraction.Id;
            model.Title = dbAttraction.Title;
            model.Summary = dbAttraction.Summary;
            model.Content = dbAttraction.Content;
            model.DisplayOrder = dbAttraction.DisplayOrder;
            model.Image = dbAttraction.Image;

            return model;
        }

        public bool UpdateArticle(int id, CreateArticleInputModel inputModel)
        {
            Article dbAttraction = this.Data.Articles.Find(id);
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

        public bool DeleteArticle(int id)
        {
            // TODO - Also delete the images for that attraction

            var theAttraction = this.Data.Articles.Find(id);
            if (theAttraction == null)
            {
                return false;
            }

            this.Data.Articles.Delete(id);
            this.Data.SaveChanges();

            return true;
        }


        public ArticleViewModel GetArticleById(int id)
        {
            ArticleViewModel model = new ArticleViewModel();
            if (this.ArticleExists(id))
            {
                Article dbAttraction = this.Data.Articles.Find(id);
                model = this.MapArticleViewModel(dbAttraction);
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
