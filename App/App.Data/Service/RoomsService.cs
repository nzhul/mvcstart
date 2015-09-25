using App.Models;
using App.Models.InputModels;
using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using App.Data.Service.Mappers;
using System.IO;

namespace App.Data.Service
{
    public class ItemsService : IItemsService
    {
        private readonly IUoWData Data;
        private readonly Mapper Mapper;

        public ItemsService(IUoWData data)
        {
            this.Data = data;
            this.Mapper = new Mapper();
        }

        public ItemViewModel GetItemById(int id)
        {
            Item item = this.Data.Items.Find(id);
            return this.Mapper.MapItemViewModel(item);
        }

        public CreateItemInputModel GetItemInputModelById(int id)
        {
            Item room = this.Data.Items.Find(id);
            return MapRoomInputModel(room);
        }

        private CreateItemInputModel MapRoomInputModel(Item room)
        {
            CreateItemInputModel model = new CreateItemInputModel();
            model.Id = room.Id;
            model.Name = room.Name;
            model.IsFeatured = room.IsFeatured;
            model.Price = room.Price;
            model.Summary = room.Summary;
            model.Description = room.Description;
            model.DisplayOrder = room.DisplayOrder;
            model.IsPriceVisible = room.IsPriceVisible;
            model.SelectedCategoryId = room.ItemCategoryId;
            model.Images = room.Images;
            model.AvailableRoomFeatures = room.ItemFeatures;

            return model;
        }

        public IEnumerable<ItemViewModel> GetRooms(int? categoryId)
        {
            IQueryable<Item> roomsQueriable = this.Data.Items.All().OrderBy(r => r.IsFeatured).AsQueryable();

            if (categoryId != null)
            {
                roomsQueriable = roomsQueriable.Where(r => r.ItemCategoryId == categoryId);
            }


            IEnumerable<ItemViewModel> rooms = roomsQueriable.Select(this.Mapper.MapItemViewModel);
            return rooms;
        }

        public bool CreateRoom(CreateItemInputModel room)
        {
            Item newRoom = new Item();
            newRoom.Name = room.Name;
            newRoom.IsFeatured = room.IsFeatured;
            newRoom.Price = room.Price;
            newRoom.DateAdded = DateTime.Now;
            newRoom.Summary = room.Summary;
            newRoom.Description = room.Description;
            newRoom.DisplayOrder = room.DisplayOrder;
            newRoom.IsPriceVisible = room.IsPriceVisible;
            newRoom.ItemCategoryId = room.SelectedCategoryId;

            if (room.SelectedRoomFeatureIds != null && room.SelectedRoomFeatureIds.Count > 0)
            {
                for (int i = 0; i < room.SelectedRoomFeatureIds.Count; i++)
                {
                    var theRoomFeature = this.Data.ItemFeatures.Find(room.SelectedRoomFeatureIds[i]);
                    newRoom.ItemFeatures.Add(theRoomFeature);
                }
            }

            Image defaultImage = new Image
            {
                ImageExtension = "jpg",
                ImagePath = "Content\\images\\noimage\\no-image",
                IsPrimary = true,
                DateAdded = DateTime.Now
            };

            this.Data.Items.Add(newRoom);
            this.Data.SaveChanges();

            newRoom.Images.Add(defaultImage);
            this.Data.SaveChanges();

            return true;
        }

        public bool RoomExists(int id)
        {
            if (id <= 0)
            {
                return false;
            }
            else
            {
                bool result = this.Data.Items.All().Any(r => r.Id == id);
                return result;
            }
        }


        public IEnumerable<SelectListItem> GetCategories()
        {
            var categories = this.Data.ItemCategories
                .All()
                .OrderBy(x => x.Id)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name.ToString()
                });

            return new SelectList(categories, "Value", "Text");
        }


        public IEnumerable<ItemCategoryViewModel> GetRoomCategories()
        {
            IEnumerable<ItemCategoryViewModel> roomCategories = this.Data.ItemCategories.All().Select(MapRoomCategoriesViewModel);
            return roomCategories;
        }

        private ItemCategoryViewModel MapRoomCategoriesViewModel(ItemCategory itemCategory)
        {
            ItemCategoryViewModel model = new ItemCategoryViewModel();
            model.Id = itemCategory.Id;
            model.Name = itemCategory.Name;
            model.Slug = itemCategory.Slug;
            model.Description = itemCategory.Description;
            model.DateAdded = itemCategory.DateAdded;
            model.ItemsCount = itemCategory.Items.Count;

            return model;
        }


        public int CreateRoomCategory(CreateItemCategoryInputModel roomCategory)
        {
            ItemCategory newRoomCategory = new ItemCategory();
            newRoomCategory.Name = roomCategory.Name;
            newRoomCategory.DisplayOrder = roomCategory.DisplayOrder;
            newRoomCategory.Description = roomCategory.Description;
            newRoomCategory.DateAdded = DateTime.Now;

            this.Data.ItemCategories.Add(newRoomCategory);
            this.Data.SaveChanges();

            return newRoomCategory.Id;
        }


        public CreateItemCategoryInputModel GetRoomCategoryInputModelById(int id)
        {
            ItemCategory roomCategory = this.Data.ItemCategories.Find(id);
            return MapRoomCategoryInputModel(roomCategory);
        }

        private CreateItemCategoryInputModel MapRoomCategoryInputModel(ItemCategory roomCategory)
        {
            CreateItemCategoryInputModel model = new CreateItemCategoryInputModel();
            model.Description = roomCategory.Description;
            model.DisplayOrder = roomCategory.DisplayOrder;
            model.Name = roomCategory.Name;

            return model;
        }


        public bool UpdateRoomCategory(int id, CreateItemCategoryInputModel roomCategory)
        {
            ItemCategory dbRoomCategory = this.Data.ItemCategories.Find(id);
            if (dbRoomCategory != null)
            {
                dbRoomCategory.Description = roomCategory.Description;
                dbRoomCategory.DisplayOrder = roomCategory.DisplayOrder;
                dbRoomCategory.Name = roomCategory.Name;

                this.Data.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
        
        public bool UpdateRoom(int id, CreateItemInputModel inputModel)
        {
            Item dbRoom = this.Data.Items.Find(id);
            if (dbRoom != null)
            {
                dbRoom.DisplayOrder = inputModel.DisplayOrder;
                dbRoom.IsFeatured = inputModel.IsFeatured;
                dbRoom.IsPriceVisible = inputModel.IsPriceVisible;
                dbRoom.Description = inputModel.Description;
                dbRoom.Name = inputModel.Name;
                dbRoom.Price = inputModel.Price;
                dbRoom.ItemCategoryId = inputModel.SelectedCategoryId;
                dbRoom.Summary = inputModel.Summary;

                // Delete all RoomFeatures
                foreach (var subCategory in dbRoom.ItemFeatures.ToList())
                {
                    dbRoom.ItemFeatures.Remove(subCategory);
                }
                this.Data.SaveChanges();

                // Insert The New RoomFeatures
                if (inputModel.SelectedRoomFeatureIds != null && inputModel.SelectedRoomFeatureIds.Count > 0)
                {
                    for (int i = 0; i < inputModel.SelectedRoomFeatureIds.Count; i++)
                    {
                        var theRoomFeature = this.Data.ItemFeatures.Find(inputModel.SelectedRoomFeatureIds[i]);
                        dbRoom.ItemFeatures.Add(theRoomFeature);
                    }
                }

                this.Data.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }


        public IEnumerable<ItemFeatureViewModel> GetRoomFeatures()
        {
            IEnumerable<ItemFeatureViewModel> roomFeatures = this.Data.ItemFeatures.All().Select(MapRoomFeaturesViewModel);
            return roomFeatures;
        }

        private ItemFeatureViewModel MapRoomFeaturesViewModel(ItemFeature roomFeature)
        {
            ItemFeatureViewModel model = new ItemFeatureViewModel();
            model.Id = roomFeature.Id;
            model.Name = roomFeature.Name;
            model.IconName = roomFeature.IconName;

            return model;
        }


        public int CreateRoomFeature(CreateItemFeatureInputModel featureInput)
        {
            ItemFeature newFeature = new ItemFeature();
            newFeature.Name = featureInput.Name;
            newFeature.IconName = featureInput.IconName;

            this.Data.ItemFeatures.Add(newFeature);
            this.Data.SaveChanges();

            return newFeature.Id;
        }

        public CreateItemFeatureInputModel GetRoomFeatureInputModelById(int id)
        {
            ItemFeature roomFeature = this.Data.ItemFeatures.Find(id);
            return MapRoomFeatureInputModel(roomFeature);
        }

        private CreateItemFeatureInputModel MapRoomFeatureInputModel(ItemFeature roomFeature)
        {
            CreateItemFeatureInputModel model = new CreateItemFeatureInputModel();
            model.Id = roomFeature.Id;
            model.Name = roomFeature.Name;
            model.IconName = roomFeature.IconName;
            return model;
        }

        public bool UpdateRoomFeature(int id, CreateItemFeatureInputModel roomFeature)
        {
            ItemFeature dbRoomFeature = this.Data.ItemFeatures.Find(id);
            if (dbRoomFeature != null)
            {
                dbRoomFeature.Name = roomFeature.Name;
                dbRoomFeature.IconName = roomFeature.IconName;

                this.Data.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }


        public bool DeleteRoomFeature(int id)
        {
            var theRoomFeature = this.Data.ItemFeatures.Find(id);
            if (theRoomFeature == null)
            {
                return false;
            }

            this.Data.ItemFeatures.Delete(id);
            this.Data.SaveChanges();

            return true;
        }


        public IEnumerable<ItemFeature> GetAvailableRoomFeatures()
        {
            return this.Data.ItemFeatures.All();
        }


        public List<int> GetSelectedRoomFeatureIds(int id)
        {
            Item dbRoom = this.Data.Items.Find(id);

            List<int> selectedRoomFeatureIds = new List<int>();

            foreach (var subCategory in dbRoom.ItemFeatures)
            {
                selectedRoomFeatureIds.Add(subCategory.Id);
            }

            return selectedRoomFeatureIds;
        }

        public bool DeleteImage(int imageId)
        {
            var theImage = this.Data.Images.Find(imageId);
            string file1 = System.Web.HttpContext.Current.Server.MapPath("~/" + theImage.ImagePath + "_detailsBigThumb.jpg");
            string file2 = System.Web.HttpContext.Current.Server.MapPath("~/" + theImage.ImagePath + "_detailsSmallThumb.jpg");
            string file3 = System.Web.HttpContext.Current.Server.MapPath("~/" + theImage.ImagePath + "_indexThumb.jpg");
            string file4 = System.Web.HttpContext.Current.Server.MapPath("~/" + theImage.ImagePath + "_large.jpg");

            TryToDelete(file1);
            TryToDelete(file2);
            TryToDelete(file3);
            TryToDelete(file4);

            this.Data.Images.Delete(imageId);
            this.Data.SaveChanges();

            return true;
        }

        private bool TryToDelete(string filePath)
        {
            try
            {
                System.IO.File.Delete(filePath);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }


        public bool DeleteRoom(int id)
        {
            Item dbRoom = this.Data.Items.Find(id);

            var dbRoomImagesClone = dbRoom.Images.Where(i => !i.ImagePath.Contains("noimage")).ToList();

            foreach (var roomImage in dbRoomImagesClone)
            {
                this.DeleteImage(roomImage.Id);
            }
            this.Data.SaveChanges();

            this.Data.Items.Delete(dbRoom);
            this.Data.SaveChanges();

            return true;
        }
    }
}