using App.Models.InputModels;
using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Service
{
    public interface IAttractionsService
    {

        IEnumerable<AttractionViewModel> GetAttractions();

        int CreateAttraction(CreateAttractionInputModel inputModel);

        bool AttractionExists(int id);

        CreateAttractionInputModel GetAttractionInputModelById(int id);

        bool UpdateAttraction(int id, CreateAttractionInputModel inputModel);

        bool DeleteAttraction(int id);

        AttractionViewModel GetAttractionById(int id);
    }
}
