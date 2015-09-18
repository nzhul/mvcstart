using App.Models.InputModels;
using App.Models.Pages;
using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Service
{
    public class PagesService : IPagesService
    {
        private readonly IUoWData Data;

        public PagesService(IUoWData data)
        {
            this.Data = data;
        }

        public IEnumerable<PageViewModel> GetPages()
        {
            IEnumerable<PageViewModel> model = this.Data.Pages.All().Select(MapPageViewModel);

            return model;
        }

        private PageViewModel MapPageViewModel (Page page)
        {
            PageViewModel model = new PageViewModel();
            model.Id = page.Id;
            model.Title = page.Title;
            model.Summary = page.Summary;
            model.Content = page.Content;

            return model;
        }


        public int CreatePage(CreatePageInputModel inputModel)
        {
            Page newPage = new Page();
            newPage.Title = inputModel.Title;
            newPage.Summary = inputModel.Summary;
            newPage.Content = inputModel.Content;
            newPage.DateCreated = DateTime.Now;
            newPage.DisplayOrder = inputModel.DisplayOrder;

            this.Data.Pages.Add(newPage);
            this.Data.SaveChanges();

            return newPage.Id;
        }


        public bool PageExists(int id)
        {
            if (id <= 0)
            {
                return false;
            }
            else
            {
                bool result = this.Data.Pages.All().Any(r => r.Id == id);
                return result;
            }
        }


        public CreatePageInputModel GetPageInputModelById(int id)
        {
            Page dbPage = this.Data.Pages.Find(id);
            return MapPageInputModel(dbPage);
        }

        private CreatePageInputModel MapPageInputModel(Page dbPage)
        {
            CreatePageInputModel model = new CreatePageInputModel();
            model.Id = dbPage.Id;
            model.Title = dbPage.Title;
            model.Summary = dbPage.Summary;
            model.Content = dbPage.Content;
            model.DisplayOrder = dbPage.DisplayOrder;

            return model;
        }


        public bool UpdatePage(int id, CreatePageInputModel inputModel)
        {
            Page dbPage = this.Data.Pages.Find(id);
            if (dbPage != null)
            {
                dbPage.Title = inputModel.Title;
                dbPage.Summary = inputModel.Summary;
                dbPage.Content = inputModel.Content;
                dbPage.DisplayOrder = inputModel.DisplayOrder;

                this.Data.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }


        public bool DeletePage(int id)
        {
            var thePage = this.Data.Pages.Find(id);
            if (thePage == null)
            {
                return false;
            }

            this.Data.Pages.Delete(id);
            this.Data.SaveChanges();

            return true;
        }



        public PageViewModel GetFeaturedCustomPage(int pageId)
        {
            PageViewModel model = new PageViewModel();
            if (this.PageExists(pageId))
            {
                Page dbPage = this.Data.Pages.Find(pageId);
                model.Id = dbPage.Id;
                model.Title = dbPage.Title;
                model.Summary = dbPage.Summary;
            }
            else
            {
                model.Id = 99999;
                model.Title = "Page Do Not Exists";
                model.Summary = "There are no pages, please contact the system administrator";
            }

            return model;
        }


        public PageViewModel GetPageById(int id)
        {
            PageViewModel model = new PageViewModel();
            if (this.PageExists(id))
            {
                Page dbPage = this.Data.Pages.Find(id);
                model.Id = dbPage.Id;
                model.Title = dbPage.Title;
                model.Summary = dbPage.Summary;
                model.Content = dbPage.Content;
            }
            else
            {
                model.Id = 1;
                model.Title = "НЕСЪЩЕСТВУВАЩА СТРАНИЦА!";
                model.Content = "НЕСЪЩЕСТВУВАЩА СТРАНИЦА!";
                model.Summary = "НЕСЪЩЕСТВУВАЩА СТРАНИЦА!";
            }

            return model;
        }
    }
}
