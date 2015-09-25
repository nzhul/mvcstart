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
        /// Get all rooms from the database
        /// </summary>
        /// <returns>Collection of RoomViewModel</returns>
        IEnumerable<ItemViewModel> GetRooms(int? categoryId);

        /// <summary>
        /// Creates new Room in the database
        /// </summary>
        /// <returns>Returns the Id of the new Room</returns>
        bool CreateRoom(CreateItemInputModel room);

        bool RoomExists(int id);

        IEnumerable<SelectListItem> GetCategories();

        IEnumerable<ItemCategoryViewModel> GetRoomCategories();

        int CreateRoomCategory(CreateItemCategoryInputModel roomCategory);

        CreateItemCategoryInputModel GetRoomCategoryInputModelById(int id);

        bool UpdateRoomCategory(int id, CreateItemCategoryInputModel inputModel);

        bool UpdateRoom(int id, CreateItemInputModel inputModel);

        IEnumerable<ItemFeatureViewModel> GetRoomFeatures();

        int CreateRoomFeature(CreateItemFeatureInputModel featureInput);

        CreateItemFeatureInputModel GetRoomFeatureInputModelById(int id);

        bool UpdateRoomFeature(int id, CreateItemFeatureInputModel roomFeature);

        bool DeleteRoomFeature(int id);

        IEnumerable<ItemFeature> GetAvailableRoomFeatures();

        List<int> GetSelectedRoomFeatureIds(int id);

        bool DeleteImage(int imageId);

        bool DeleteRoom(int id);
    }
}
