using System.ComponentModel.DataAnnotations;

namespace App.Models.InputModels
{
	public class CreateItemFeatureInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името на характеристикато е задължително:")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Невалидно име - Максимална дължина 250 символа, минимална 3")]
        [Display(Name = "Име:")]
        public string Name { get; set; }


        [Display(Name = "Font awesome class:")]
        public string IconName { get; set; }
    }
}
