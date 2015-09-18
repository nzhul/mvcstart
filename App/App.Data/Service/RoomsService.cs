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
    public class RoomsService : IRoomsService
    {
        private readonly IUoWData Data;
        private readonly Mapper Mapper;

        public RoomsService(IUoWData data)
        {
            this.Data = data;
            this.Mapper = new Mapper();
        }

        public RoomViewModel GetRoomById(int id)
        {
            Room room = this.Data.Rooms.Find(id);
            return this.Mapper.MapRoomViewModel(room);
        }

        public CreateRoomInputModel GetRoomInputModelById(int id)
        {
            Room room = this.Data.Rooms.Find(id);
            return MapRoomInputModel(room);
        }

        private CreateRoomInputModel MapRoomInputModel(Room room)
        {
            CreateRoomInputModel model = new CreateRoomInputModel();
            model.Id = room.Id;
            model.Name = room.Name;
            model.IsFeatured = room.IsFeatured;
            model.RoomNumber = room.RoomNumber;
            model.Price = room.Price;
            model.Summary = room.Summary;
            model.Description = room.Description;
            model.DisplayOrder = room.DisplayOrder;
            model.IsPriceVisible = room.IsPriceVisible;
            model.SelectedCategoryId = room.RoomCategoryId;
            model.Images = room.Images;
            model.AvailableRoomFeatures = room.RoomFeatures;

            return model;
        }

        public IEnumerable<RoomViewModel> GetRooms(int? categoryId)
        {
            IQueryable<Room> roomsQueriable = this.Data.Rooms.All().OrderBy(r => r.IsFeatured).AsQueryable();

            if (categoryId != null)
            {
                roomsQueriable = roomsQueriable.Where(r => r.RoomCategoryId == categoryId);
            }


            IEnumerable<RoomViewModel> rooms = roomsQueriable.Select(this.Mapper.MapRoomViewModel);
            return rooms;
        }

        public bool CreateRoom(CreateRoomInputModel room)
        {
            Room newRoom = new Room();
            newRoom.Name = room.Name;
            newRoom.IsFeatured = room.IsFeatured;
            newRoom.RoomNumber = room.RoomNumber;
            newRoom.Price = room.Price;
            newRoom.DateAdded = DateTime.Now;
            newRoom.Summary = room.Summary;
            newRoom.Description = room.Description;
            newRoom.DisplayOrder = room.DisplayOrder;
            newRoom.IsPriceVisible = room.IsPriceVisible;
            newRoom.RoomCategoryId = room.SelectedCategoryId;

            if (room.SelectedRoomFeatureIds != null && room.SelectedRoomFeatureIds.Count > 0)
            {
                for (int i = 0; i < room.SelectedRoomFeatureIds.Count; i++)
                {
                    var theRoomFeature = this.Data.RoomFeatures.Find(room.SelectedRoomFeatureIds[i]);
                    newRoom.RoomFeatures.Add(theRoomFeature);
                }
            }

            Image defaultImage = new Image
            {
                ImageExtension = "jpg",
                ImagePath = "Content\\images\\noimage\\no-image",
                IsPrimary = true,
                DateAdded = DateTime.Now
            };

            this.Data.Rooms.Add(newRoom);
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
                bool result = this.Data.Rooms.All().Any(r => r.Id == id);
                return result;
            }
        }


        public IEnumerable<SelectListItem> GetCategories()
        {
            var categories = this.Data.RoomCategories
                .All()
                .OrderBy(x => x.Id)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name.ToString()
                });

            return new SelectList(categories, "Value", "Text");
        }


        public IEnumerable<RoomCategoryViewModel> GetRoomCategories()
        {
            IEnumerable<RoomCategoryViewModel> roomCategories = this.Data.RoomCategories.All().Select(MapRoomCategoriesViewModel);
            return roomCategories;
        }

        private RoomCategoryViewModel MapRoomCategoriesViewModel(RoomCategory roomCategory)
        {
            RoomCategoryViewModel model = new RoomCategoryViewModel();
            model.Id = roomCategory.Id;
            model.Name = roomCategory.Name;
            model.Slug = roomCategory.Slug;
            model.Description = roomCategory.Description;
            model.DateAdded = roomCategory.DateAdded;
            model.RoomsCount = roomCategory.Rooms.Count;

            return model;
        }


        public int CreateRoomCategory(CreateRoomCategoryInputModel roomCategory)
        {
            RoomCategory newRoomCategory = new RoomCategory();
            newRoomCategory.Name = roomCategory.Name;
            newRoomCategory.DisplayOrder = roomCategory.DisplayOrder;
            newRoomCategory.Description = roomCategory.Description;
            newRoomCategory.DateAdded = DateTime.Now;

            this.Data.RoomCategories.Add(newRoomCategory);
            this.Data.SaveChanges();

            return newRoomCategory.Id;
        }


        public CreateRoomCategoryInputModel GetRoomCategoryInputModelById(int id)
        {
            RoomCategory roomCategory = this.Data.RoomCategories.Find(id);
            return MapRoomCategoryInputModel(roomCategory);
        }

        private CreateRoomCategoryInputModel MapRoomCategoryInputModel(RoomCategory roomCategory)
        {
            CreateRoomCategoryInputModel model = new CreateRoomCategoryInputModel();
            model.Description = roomCategory.Description;
            model.DisplayOrder = roomCategory.DisplayOrder;
            model.Name = roomCategory.Name;

            return model;
        }


        public bool UpdateRoomCategory(int id, CreateRoomCategoryInputModel roomCategory)
        {
            RoomCategory dbRoomCategory = this.Data.RoomCategories.Find(id);
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
        
        public bool UpdateRoom(int id, CreateRoomInputModel inputModel)
        {
            Room dbRoom = this.Data.Rooms.Find(id);
            if (dbRoom != null)
            {
                dbRoom.DisplayOrder = inputModel.DisplayOrder;
                dbRoom.IsFeatured = inputModel.IsFeatured;
                dbRoom.IsPriceVisible = inputModel.IsPriceVisible;
                dbRoom.Description = inputModel.Description;
                dbRoom.Name = inputModel.Name;
                dbRoom.Price = inputModel.Price;
                dbRoom.RoomCategoryId = inputModel.SelectedCategoryId;
                dbRoom.RoomNumber = inputModel.RoomNumber;
                dbRoom.Summary = inputModel.Summary;

                // Delete all RoomFeatures
                foreach (var subCategory in dbRoom.RoomFeatures.ToList())
                {
                    dbRoom.RoomFeatures.Remove(subCategory);
                }
                this.Data.SaveChanges();

                // Insert The New RoomFeatures
                if (inputModel.SelectedRoomFeatureIds != null && inputModel.SelectedRoomFeatureIds.Count > 0)
                {
                    for (int i = 0; i < inputModel.SelectedRoomFeatureIds.Count; i++)
                    {
                        var theRoomFeature = this.Data.RoomFeatures.Find(inputModel.SelectedRoomFeatureIds[i]);
                        dbRoom.RoomFeatures.Add(theRoomFeature);
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


        public IEnumerable<RoomFeatureViewModel> GetRoomFeatures()
        {
            IEnumerable<RoomFeatureViewModel> roomFeatures = this.Data.RoomFeatures.All().Select(MapRoomFeaturesViewModel);
            return roomFeatures;
        }

        private RoomFeatureViewModel MapRoomFeaturesViewModel(RoomFeature roomFeature)
        {
            RoomFeatureViewModel model = new RoomFeatureViewModel();
            model.Id = roomFeature.Id;
            model.Name = roomFeature.Name;
            model.IconName = roomFeature.IconName;

            return model;
        }


        public int CreateRoomFeature(CreateRoomFeatureInputModel featureInput)
        {
            RoomFeature newFeature = new RoomFeature();
            newFeature.Name = featureInput.Name;
            newFeature.IconName = featureInput.IconName;

            this.Data.RoomFeatures.Add(newFeature);
            this.Data.SaveChanges();

            return newFeature.Id;
        }

        public CreateRoomFeatureInputModel GetRoomFeatureInputModelById(int id)
        {
            RoomFeature roomFeature = this.Data.RoomFeatures.Find(id);
            return MapRoomFeatureInputModel(roomFeature);
        }

        private CreateRoomFeatureInputModel MapRoomFeatureInputModel(RoomFeature roomFeature)
        {
            CreateRoomFeatureInputModel model = new CreateRoomFeatureInputModel();
            model.Id = roomFeature.Id;
            model.Name = roomFeature.Name;
            model.IconName = roomFeature.IconName;
            return model;
        }

        public bool UpdateRoomFeature(int id, CreateRoomFeatureInputModel roomFeature)
        {
            RoomFeature dbRoomFeature = this.Data.RoomFeatures.Find(id);
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
            var theRoomFeature = this.Data.RoomFeatures.Find(id);
            if (theRoomFeature == null)
            {
                return false;
            }

            this.Data.RoomFeatures.Delete(id);
            this.Data.SaveChanges();

            return true;
        }


        public IEnumerable<RoomFeature> GetAvailableRoomFeatures()
        {
            return this.Data.RoomFeatures.All();
        }


        public List<int> GetSelectedRoomFeatureIds(int id)
        {
            Room dbRoom = this.Data.Rooms.Find(id);

            List<int> selectedRoomFeatureIds = new List<int>();

            foreach (var subCategory in dbRoom.RoomFeatures)
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
            Room dbRoom = this.Data.Rooms.Find(id);

            var dbRoomImagesClone = dbRoom.Images.Where(i => !i.ImagePath.Contains("noimage")).ToList();

            foreach (var roomImage in dbRoomImagesClone)
            {
                this.DeleteImage(roomImage.Id);
            }
            this.Data.SaveChanges();

            this.Data.Rooms.Delete(dbRoom);
            this.Data.SaveChanges();

            return true;
        }
    }
}