using App.Models.InputModels;
using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Service
{
    public interface IArticlesService
    {

        IEnumerable<ArticleViewModel> GetArticles();

        int CreateArticle(CreateArticleInputModel inputModel);

        bool ArticleExists(int id);

        CreateArticleInputModel GetArticleInputModelById(int id);

        bool UpdateArticle(int id, CreateArticleInputModel inputModel);

        bool DeleteArticle(int id);

        ArticleViewModel GetArticleById(int id);
    }
}
