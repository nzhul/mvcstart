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
            Article newArticle = new Article();
            newArticle.Title = inputModel.Title;
            newArticle.Summary = inputModel.Summary;
            newArticle.Content = inputModel.Content;
            newArticle.DateAdded = DateTime.Now;
            newArticle.DisplayOrder = inputModel.DisplayOrder;

            this.Data.Articles.Add(newArticle);
            this.Data.SaveChanges();

            Image defaultImage = new Image
            {
                ImageExtension = "jpg",
                ImagePath = "Content\\images\\noimage\\no-image",
                IsPrimary = true,
                DateAdded = DateTime.Now
            };

            newArticle.Image = defaultImage;
            this.Data.SaveChanges();

            return newArticle.Id;
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
            Article dbArticle = this.Data.Articles.Find(id);
            return MapPageInputModel(dbArticle);
        }

        private CreateArticleInputModel MapPageInputModel(Article dbArticle)
        {
            CreateArticleInputModel model = new CreateArticleInputModel();
            model.Id = dbArticle.Id;
            model.Title = dbArticle.Title;
            model.Summary = dbArticle.Summary;
            model.Content = dbArticle.Content;
            model.DisplayOrder = dbArticle.DisplayOrder;
            model.Image = dbArticle.Image;

            return model;
        }

        public bool UpdateArticle(int id, CreateArticleInputModel inputModel)
        {
            Article dbArticle = this.Data.Articles.Find(id);
            if (dbArticle != null)
            {
                dbArticle.Title = inputModel.Title;
                dbArticle.Summary = inputModel.Summary;
                dbArticle.Content = inputModel.Content;
                dbArticle.DisplayOrder = inputModel.DisplayOrder;

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
            // TODO - Also delete the images for that Article

            var theArticle = this.Data.Articles.Find(id);
            if (theArticle == null)
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
                Article dbArticle = this.Data.Articles.Find(id);
                model = this.MapArticleViewModel(dbArticle);
            }
            else
            {
                model.Content = "Не съществува такава статия!";
                model.Title = "НЕСЪЩЕСТВУВАЩА АТКРАКЦИЯ!";
            }

            return model;
        }
    }
}
