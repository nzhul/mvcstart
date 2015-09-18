using App.Models.InputModels;
using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Service
{
    public interface IPagesService
    {
        IEnumerable<PageViewModel> GetPages();

        PageViewModel GetPageById(int id);

        int CreatePage(CreatePageInputModel inputModel);

        bool PageExists(int id);

        CreatePageInputModel GetPageInputModelById(int id);

        bool UpdatePage(int id, CreatePageInputModel inputModel);

        bool DeletePage(int id);

        PageViewModel GetFeaturedCustomPage(int pageId);
    }
}
