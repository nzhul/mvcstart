using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.InputModels
{
    public class CreateRoomFeatureInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името на удобството е задължително:")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Невалидно име - Максимална дължина 250 символа, минимална 3")]
        [Display(Name = "Име:")]
        public string Name { get; set; }


        [Display(Name = "Font awesome class:")]
        public string IconName { get; set; }
    }
}
