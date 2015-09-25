using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace App.Models.InputModels
{
    public class UploadPhotoModel
    {
        public int ItemId { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }
}
