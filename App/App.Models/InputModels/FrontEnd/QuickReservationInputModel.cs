using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.InputModels.FrontEnd
{
    public class QuickReservationInputModel
    {
        [Required(ErrorMessage = "Задължително въведете брой възрастни!")]
        [Range(1, int.MaxValue, ErrorMessage = "Моля въведете валидно число!")]
        [Display(Name = "Брой възрастни:")]
        public int Adults { get; set; }

        [Display(Name = "Брой деца:")]
        public int Childrens { get; set; }

        [Required(ErrorMessage = "Задължително посочете датата на настаняване!")]
        [Display(Name = "Дата на настаняване:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ArrivalDate { get; set; }

        [Required(ErrorMessage = "Задължително посочете датата на напускане!")]
        [Display(Name = "Дата на напускане:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "Категорията е задължителна!")]
        public int ItemCategoryId { get; set; }

        [Required(ErrorMessage = "Продуктта е задължителна!")]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Първото име е задължително!")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Телефонът е задължителен!")]
        public string Phone { get; set; }

        public string Email { get; set; }

    }
}
