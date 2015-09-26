using App.Models;
using App.Models.InputModels;
using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace App.Data.Service
{
    public interface IItemsService
    {
        ItemViewModel GetItemById(int id);

        CreateItemInputModel GetItemInputModelById(int id);

        /// <summary>
        /// Get all items from the database
        /// </summary>
        /// <returns>Collection of ItemViewModel</returns>
        IEnumerable<ItemViewModel> GetItems(int? categoryId);

        /// <summary>
        /// Creates new Item in the database
        /// </summary>
        /// <returns>Returns the Id of the new Item</returns>
        bool CreateItem(CreateItemInputModel item);

        bool ItemExists(int id);

        IEnumerable<SelectListItem> GetCategories();

        IEnumerable<ItemCategoryViewModel> GetItemCategories();

        int CreateItemCategory(CreateItemCategoryInputModel itemCategory);

        CreateItemCategoryInputModel GetItemCategoryInputModelById(int id);

        bool UpdateItemCategory(int id, CreateItemCategoryInputModel inputModel);

        bool UpdateItem(int id, CreateItemInputModel inputModel);

        IEnumerable<ItemFeatureViewModel> GetItemFeatures();

        int CreateItemFeature(CreateItemFeatureInputModel featureInput);

        CreateItemFeatureInputModel GetItemFeatureInputModelById(int id);

        bool UpdateItemFeature(int id, CreateItemFeatureInputModel itemFeature);

        bool DeleteItemFeature(int id);

        IEnumerable<ItemFeature> GetAvailableItemFeatures();

        List<int> GetSelectedItemFeatureIds(int id);

        bool DeleteImage(int imageId);

        bool DeleteItem(int id);
    }
}
