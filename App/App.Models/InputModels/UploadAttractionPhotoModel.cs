using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace App.Models.InputModels
{
    public class UploadArticlePhotoModel
    {
        public int ArticleId { get; set; }

        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }
}
