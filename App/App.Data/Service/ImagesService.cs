using App.Models;
using App.Models.InputModels;
using ImageResizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace App.Data.Service
{
    public class ImagesService : IImagesService
    {
        private readonly IUoWData Data;

        public ImagesService(IUoWData data)
        {
            this.Data = data;
        }

        public bool UploadImages(UploadPhotoModel uploadData)
        {
            Dictionary<string, string> versions = new Dictionary<string, string>();
            //Define the versions to generate
            versions.Add("_indexThumb", "width=361&height=223&crop=auto&format=jpg"); //Crop to square thumbnail
            versions.Add("_detailsBigThumb", "maxwidth=336&crop=auto&format=jpg"); //Fit inside 400x400 area, jpeg
            versions.Add("_detailsSmallThumb", "width=77&height=61&crop=auto&format=jpg"); //Fit inside 400x400 area, jpeg
            versions.Add("_large", "width=1139&height=578&crop=auto&scale=both&format=jpg"); //Fit inside 1900x1200 area

            int categoryId = uploadData.CategoryId;
            int itemId = uploadData.ItemId;
            var theItem = this.Data.Items.Find(itemId);

            bool firstLoop = true;
            foreach (var file in uploadData.Files)
            {
                if (file != null)
                {
                    var originalFileName = file.FileName.Split('.')[0].Replace(' ', '_');
                    var originalFileExtension = file.FileName.Split('.')[1];

                    string uploadFolder = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/" + categoryId + "/" + itemId);
                    if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

                    foreach (string suffix in versions.Keys)
                    {
                        //Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        string fileName = Path.Combine(uploadFolder, originalFileName + suffix);

                        fileName = ImageBuilder.Current.Build(file, fileName, new ResizeSettings(versions[suffix]), false, true);
                    }

                    var newImage = new Image
                    {
                        ImagePath = "Uploads\\" + categoryId + "\\" + itemId + "\\" + originalFileName,
                        ImageExtension = originalFileExtension,
                        IsPrimary = false,
                        DateAdded = DateTime.Now
                    };

                    if (firstLoop)
                    {
                        theItem.Images.First(image => image.IsPrimary == true).IsPrimary = false;
                        newImage.IsPrimary = true;
                    }

                    theItem.Images.Add(newImage);
                    this.Data.SaveChanges();
                }

                firstLoop = false;
            }

            return true;
        }


        public bool MakePrimary(int imageId, int itemId)
        {
            var theItem = this.Data.Items.Find(itemId);
            if (theItem == null)
            {
                return false;
            }

            var oldPrimary = theItem.Images.FirstOrDefault(image => image.IsPrimary);
            oldPrimary.IsPrimary = false;

            var newPrimary = theItem.Images.FirstOrDefault(image => image.Id == imageId);
            newPrimary.IsPrimary = true;

            this.Data.SaveChanges();

            return true;
        }

        public bool UploadImages(UploadArticlePhotoModel uploadData)
        {
            Dictionary<string, string> versions = new Dictionary<string, string>();
            //Define the versions to generate
            versions.Add("_indexThumb", "width=361&height=78&crop=auto&format=jpg");
            versions.Add("_BigThumb", "maxwidth=361&height=223&crop=auto&format=jpg");
            versions.Add("_detailsSmallThumb", "width=77&height=61&crop=auto&format=jpg"); //Fit inside 400x400 area, jpeg
            versions.Add("_large", "width=827&crop=auto&format=jpg");

            int ArticleId = uploadData.ArticleId;
            var theArticle = this.Data.Articles.Find(ArticleId);

            foreach (var file in uploadData.Files)
            {
                if (file != null)
                {
                    var originalFileName = file.FileName.Split('.')[0].Replace(' ', '_');
                    var originalFileExtension = file.FileName.Split('.')[1];

                    string uploadFolder = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/Articles/" + ArticleId);
                    if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

                    foreach (string suffix in versions.Keys)
                    {
                        string fileName = Path.Combine(uploadFolder, originalFileName + suffix);

                        fileName = ImageBuilder.Current.Build(file, fileName, new ResizeSettings(versions[suffix]), false, true);
                    }

                    var newImage = new Image
                    {
                        ImagePath = "Uploads\\Articles\\" + ArticleId + "\\" + originalFileName,
                        ImageExtension = originalFileExtension,
                        IsPrimary = true,
                        DateAdded = DateTime.Now,
                    };

                    theArticle.Image = newImage;
                    this.Data.SaveChanges();
                }
            }

            return true;
        }


        public IEnumerable<Image> GetRandomItemImages()
        {
            Item randomItem = this.Data.Items.All().OrderBy(r => Guid.NewGuid()).FirstOrDefault();

            if (randomItem != null)
            {
                return randomItem.Images.Where(i => !i.ImagePath.EndsWith("no-image"));
            }
            else
            {
                return new List<Image>();
            }
        }


        public bool UploadGalleryImage(HttpPostedFileBase file)
        {
            Dictionary<string, string> versions = new Dictionary<string, string>();
            //Define the versions to generate

            if (file.FileName.Length % 2 == 0)
            {
                versions.Add("_thumb", "width=280&height=340&crop=auto&format=jpg");
            }
            else
            {
                versions.Add("_thumb", "width=300&height=240&crop=auto&format=jpg");
            }

            versions.Add("_large", "maxwidth=1200&maxheight=1200&format=jpg");

            if (file != null)
            {
                var originalFileName = file.FileName.Split('.')[0].Replace(' ', '_');
                var originalFileExtension = file.FileName.Split('.')[1];

                originalFileName += Guid.NewGuid().ToString();

                string uploadFolder = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/Gallery");
                if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

                foreach (string suffix in versions.Keys)
                {
                    string fileName = Path.Combine(uploadFolder, originalFileName + suffix);

                    fileName = ImageBuilder.Current.Build(file, fileName, new ResizeSettings(versions[suffix]), false, true);
                }

                var newImage = new Image
                {
                    ImagePath = "Uploads\\Gallery\\" + originalFileName,
                    ImageExtension = originalFileExtension,
                    IsPrimary = true,
                    DateAdded = DateTime.Now,
                    IsGalleryImage = true
                };

                this.Data.Images.Add(newImage);
                this.Data.SaveChanges();
            }

            return true;
        }


        public IEnumerable<Image> GetGalleryImage()
        {
            return this.Data.Images.All().Where(i => i.IsGalleryImage == true);
        }


        public bool DeleteGalleryImage(int imageId)
        {
            var theImage = this.Data.Images.Find(imageId);
            string file1 = System.Web.HttpContext.Current.Server.MapPath("~/" + theImage.ImagePath + "_thumb.jpg");
            string file2 = System.Web.HttpContext.Current.Server.MapPath("~/" + theImage.ImagePath + "_large.jpg");

            TryToDelete(file1);
            TryToDelete(file2);

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
    }
}
