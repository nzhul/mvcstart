using App.Models.DataAnnotations;
using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.InputModels
{
    public class CreateReservationInputModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Задължително посочете датата на настаняване!")]
        [Display(Name = "Дата на настаняване:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime ArrivalDate { get; set; }

        [Required(ErrorMessage = "Задължително посочете датата на напускане!")]
        [Display(Name = "Дата на напускане:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "Посочете брой продукти!")]
        [Range(1, int.MaxValue, ErrorMessage = "Моля въведете валидно число!")]
        [Display(Name = "Брой на наетите продукти:")]
        public int ItemsCount { get; set; }

        [Required(ErrorMessage = "Задължително въведете брой възрастни!")]
        [Range(1, int.MaxValue, ErrorMessage = "Моля въведете валидно число!")]
        [Display(Name = "Брой възрастни:")]
        public int Adults { get; set; }

        [Display(Name = "Брой деца:")]
        public int Childs { get; set; }

        [Display(Name = "Потвърдена резервация:")]
        public bool IsConfirmed { get; set; }

        [Display(Name = "Име на наемателя:")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия на наемателя:")]
        public string LastName { get; set; }

        [Display(Name = "Телефон на наемателя:")]
        public string Phone { get; set; }

        [Display(Name = "Email на наемателя:")]
        public string Email { get; set; }

        public IEnumerable<ItemViewModel> AvailableItems { get; set; }

        [CheckList(1, false, ErrorMessage = "Моля изберете поне една продукт!")]
        [Display(Name = "Резервирани продукти:")]
        public List<int> SelectedItemIds { get; set; }

    }
}
