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
            Item item = this.Data.Items.Find(id);
            return MapItemInputModel(item);
        }

        private CreateItemInputModel MapItemInputModel(Item item)
        {
            CreateItemInputModel model = new CreateItemInputModel();
            model.Id = item.Id;
            model.Name = item.Name;
            model.IsFeatured = item.IsFeatured;
            model.Price = item.Price;
            model.Summary = item.Summary;
            model.Description = item.Description;
            model.DisplayOrder = item.DisplayOrder;
            model.IsPriceVisible = item.IsPriceVisible;
            model.SelectedCategoryId = item.ItemCategoryId;
            model.Images = item.Images;
            model.AvailableItemFeatures = item.ItemFeatures;

            return model;
        }

        public IEnumerable<ItemViewModel> GetItems(int? categoryId)
        {
            IQueryable<Item> itemsQueriable = this.Data.Items.All().OrderBy(r => r.IsFeatured).AsQueryable();

            if (categoryId != null)
            {
                itemsQueriable = itemsQueriable.Where(r => r.ItemCategoryId == categoryId);
            }


            IEnumerable<ItemViewModel> items = itemsQueriable.Select(this.Mapper.MapItemViewModel);
            return items;
        }

        public bool CreateItem(CreateItemInputModel item)
        {
            Item newItem = new Item();
            newItem.Name = item.Name;
            newItem.IsFeatured = item.IsFeatured;
            newItem.Price = item.Price;
            newItem.DateAdded = DateTime.Now;
            newItem.Summary = item.Summary;
            newItem.Description = item.Description;
            newItem.DisplayOrder = item.DisplayOrder;
            newItem.IsPriceVisible = item.IsPriceVisible;
            newItem.ItemCategoryId = item.SelectedCategoryId;

            if (item.SelectedItemFeatureIds != null && item.SelectedItemFeatureIds.Count > 0)
            {
                for (int i = 0; i < item.SelectedItemFeatureIds.Count; i++)
                {
                    var theItemFeature = this.Data.ItemFeatures.Find(item.SelectedItemFeatureIds[i]);
                    newItem.ItemFeatures.Add(theItemFeature);
                }
            }

            Image defaultImage = new Image
            {
                ImageExtension = "jpg",
                ImagePath = "Content\\images\\noimage\\no-image",
                IsPrimary = true,
                DateAdded = DateTime.Now
            };

            this.Data.Items.Add(newItem);
            this.Data.SaveChanges();

            newItem.Images.Add(defaultImage);
            this.Data.SaveChanges();

            return true;
        }

        public bool ItemExists(int id)
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


        public IEnumerable<ItemCategoryViewModel> GetItemCategories()
        {
            IEnumerable<ItemCategoryViewModel> itemCategories = this.Data.ItemCategories.All().Select(MapItemCategoriesViewModel);
            return itemCategories;
        }

        private ItemCategoryViewModel MapItemCategoriesViewModel(ItemCategory itemCategory)
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


        public int CreateItemCategory(CreateItemCategoryInputModel itemCategory)
        {
            ItemCategory newItemCategory = new ItemCategory();
            newItemCategory.Name = itemCategory.Name;
            newItemCategory.DisplayOrder = itemCategory.DisplayOrder;
            newItemCategory.Description = itemCategory.Description;
            newItemCategory.DateAdded = DateTime.Now;

            this.Data.ItemCategories.Add(newItemCategory);
            this.Data.SaveChanges();

            return newItemCategory.Id;
        }


        public CreateItemCategoryInputModel GetItemCategoryInputModelById(int id)
        {
            ItemCategory itemCategory = this.Data.ItemCategories.Find(id);
            return MapItemCategoryInputModel(itemCategory);
        }

        private CreateItemCategoryInputModel MapItemCategoryInputModel(ItemCategory itemCategory)
        {
            CreateItemCategoryInputModel model = new CreateItemCategoryInputModel();
            model.Description = itemCategory.Description;
            model.DisplayOrder = itemCategory.DisplayOrder;
            model.Name = itemCategory.Name;

            return model;
        }


        public bool UpdateItemCategory(int id, CreateItemCategoryInputModel itemCategory)
        {
            ItemCategory dbItemCategory = this.Data.ItemCategories.Find(id);
            if (dbItemCategory != null)
            {
                dbItemCategory.Description = itemCategory.Description;
                dbItemCategory.DisplayOrder = itemCategory.DisplayOrder;
                dbItemCategory.Name = itemCategory.Name;

                this.Data.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
        
        public bool UpdateItem(int id, CreateItemInputModel inputModel)
        {
            Item dbItem = this.Data.Items.Find(id);
            if (dbItem != null)
            {
                dbItem.DisplayOrder = inputModel.DisplayOrder;
                dbItem.IsFeatured = inputModel.IsFeatured;
                dbItem.IsPriceVisible = inputModel.IsPriceVisible;
                dbItem.Description = inputModel.Description;
                dbItem.Name = inputModel.Name;
                dbItem.Price = inputModel.Price;
                dbItem.ItemCategoryId = inputModel.SelectedCategoryId;
                dbItem.Summary = inputModel.Summary;

                // Delete all ItemFeatures
                foreach (var subCategory in dbItem.ItemFeatures.ToList())
                {
                    dbItem.ItemFeatures.Remove(subCategory);
                }
                this.Data.SaveChanges();

                // Insert The New ItemFeatures
                if (inputModel.SelectedItemFeatureIds != null && inputModel.SelectedItemFeatureIds.Count > 0)
                {
                    for (int i = 0; i < inputModel.SelectedItemFeatureIds.Count; i++)
                    {
                        var theItemFeature = this.Data.ItemFeatures.Find(inputModel.SelectedItemFeatureIds[i]);
                        dbItem.ItemFeatures.Add(theItemFeature);
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


        public IEnumerable<ItemFeatureViewModel> GetItemFeatures()
        {
            IEnumerable<ItemFeatureViewModel> itemFeatures = this.Data.ItemFeatures.All().Select(MapItemFeaturesViewModel);
            return itemFeatures;
        }

        private ItemFeatureViewModel MapItemFeaturesViewModel(ItemFeature itemFeature)
        {
            ItemFeatureViewModel model = new ItemFeatureViewModel();
            model.Id = itemFeature.Id;
            model.Name = itemFeature.Name;
            model.IconName = itemFeature.IconName;

            return model;
        }


        public int CreateItemFeature(CreateItemFeatureInputModel featureInput)
        {
            ItemFeature newFeature = new ItemFeature();
            newFeature.Name = featureInput.Name;
            newFeature.IconName = featureInput.IconName;

            this.Data.ItemFeatures.Add(newFeature);
            this.Data.SaveChanges();

            return newFeature.Id;
        }

        public CreateItemFeatureInputModel GetItemFeatureInputModelById(int id)
        {
            ItemFeature itemFeature = this.Data.ItemFeatures.Find(id);
            return MapItemFeatureInputModel(itemFeature);
        }

        private CreateItemFeatureInputModel MapItemFeatureInputModel(ItemFeature itemFeature)
        {
            CreateItemFeatureInputModel model = new CreateItemFeatureInputModel();
            model.Id = itemFeature.Id;
            model.Name = itemFeature.Name;
            model.IconName = itemFeature.IconName;
            return model;
        }

        public bool UpdateItemFeature(int id, CreateItemFeatureInputModel itemFeature)
        {
            ItemFeature dbItemFeature = this.Data.ItemFeatures.Find(id);
            if (dbItemFeature != null)
            {
                dbItemFeature.Name = itemFeature.Name;
                dbItemFeature.IconName = itemFeature.IconName;

                this.Data.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }


        public bool DeleteItemFeature(int id)
        {
            var theItemFeature = this.Data.ItemFeatures.Find(id);
            if (theItemFeature == null)
            {
                return false;
            }

            this.Data.ItemFeatures.Delete(id);
            this.Data.SaveChanges();

            return true;
        }


        public IEnumerable<ItemFeature> GetAvailableItemFeatures()
        {
            return this.Data.ItemFeatures.All();
        }


        public List<int> GetSelectedItemFeatureIds(int id)
        {
            Item dbItem = this.Data.Items.Find(id);

            List<int> selectedItemFeatureIds = new List<int>();

            foreach (var subCategory in dbItem.ItemFeatures)
            {
                selectedItemFeatureIds.Add(subCategory.Id);
            }

            return selectedItemFeatureIds;
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


        public bool DeleteItem(int id)
        {
            Item dbItem = this.Data.Items.Find(id);

            var dbItemImagesClone = dbItem.Images.Where(i => !i.ImagePath.Contains("noimage")).ToList();

            foreach (var itemImage in dbItemImagesClone)
            {
                this.DeleteImage(itemImage.Id);
            }
            this.Data.SaveChanges();

            this.Data.Items.Delete(dbItem);
            this.Data.SaveChanges();

            return true;
        }
    }
}