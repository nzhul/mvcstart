using App.Models.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace App.Models.InputModels
{
    public class CreateRoomInputModel
    {
        public CreateRoomInputModel()
        {
            this.AvailableRoomFeatures = new List<ItemFeature>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Номера на стаята е задължителен!")]
        [Range(1, int.MaxValue, ErrorMessage = "Моля въведете валидно число!")]
        [Display(Name = "Номер:")]
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = "Името на стаята е задължително:")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Невалидно име - Максимална дължина 250 символа, минимална 3")]
        [Display(Name = "Име:")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Краткото описание е задължително!")]
        [Display(Name = "Кратко описание:")]
        [StringLength(160, MinimumLength = 3, ErrorMessage = "Невалидно име - Максимална дължина 160 символа, минимална 3")]
        public string Summary { get; set; }

        [Required(ErrorMessage = "Дългото описание е задължително!")]
        [AllowHtml]
        [Display(Name = "Дълго описание:")]
        [DataType("tinymce_full")]
        [UIHint("tinymce_full")]
        public string Description { get; set; }

        [Display(Name = "Цена")]
        public int Price { get; set; }


        [Display(Name = "На Заглавна страница: ")]
        public bool IsFeatured { get; set; }

        [Display(Name = "Покажи цената: ")]
        public bool IsPriceVisible { get; set; }

        [Display(Name = "Позиция: ")]
        public int DisplayOrder { get; set; }

        [Required(ErrorMessage = "Задължително!")]
        [Display(Name = "Категория")]
        public int SelectedCategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        public IEnumerable<Image> Images { get; set; }

        [Display(Name = "Екстри: ")]
        public IEnumerable<ItemFeature> AvailableRoomFeatures { get; set; }

        //[CheckList(1, false, ErrorMessage = "Моля изберете поне едно удобство!")]
        [Display(Name = "Удобства:")]
        public List<int> SelectedRoomFeatureIds { get; set; }
    }
}
