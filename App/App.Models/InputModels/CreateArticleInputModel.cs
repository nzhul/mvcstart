using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace App.Models.InputModels
{
    public class CreateArticleInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името на статията е задължително:")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Невалидно име - Максимална дължина 250 символа, минимална 3")]
        [Display(Name = "Име:")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Краткото описание е задължително!")]
        [Display(Name = "Кратно описание:")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Невалидно име - Максимална дължина 200 символа, минимална 3")]
        public string Summary { get; set; }

        [Required(ErrorMessage = "Съдържанието е задължително!")]
        [AllowHtml]
        [Display(Name = "Съдържание:")]
        [DataType("tinymce_full")]
        [UIHint("tinymce_full")]
        public string Content { get; set; }

        [Display(Name = "Позиция:")]
        public int DisplayOrder { get; set; }

        public Image Image { get; set; }
    }
}
