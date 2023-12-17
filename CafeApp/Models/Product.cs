using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CafeApp.Models;

namespace CafeApp.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        public string? Name { get; set; }

        public string? ImageSrc { get; set; }

        public string? Option { get; set; }

        public string? Type { get; set; }  //Beverage, dessert

        public string? Details { get; set; }

        public string? Price { get; set; }

    }
}