using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthProject.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }

        [Display(Name = "Цена")]
        [Required(ErrorMessage = "Не указана цена")]
        public int? Price { get; set; }

        [Display(Name = "Количество")]
        [Required(ErrorMessage = "Не указано количество")]
        public int? Count { get; set; }
    }
}
