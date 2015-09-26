using App.Models;
using App.Models.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace App.Data.Service
{
    public interface IImagesService
    {
        bool UploadImages(UploadPhotoModel uploadData);

        bool UploadImages(UploadArticlePhotoModel uploadData);

        bool MakePrimary(int imageId, int productId);

        IEnumerable<Image> GetRandomItemImages();

        bool UploadGalleryImage(HttpPostedFileBase file);

        IEnumerable<Image> GetGalleryImage();

        bool DeleteGalleryImage(int imageId);
    }
}
