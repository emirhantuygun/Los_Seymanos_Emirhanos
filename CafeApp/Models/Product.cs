using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CafeApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafeApp.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [BindProperty]
        public string? Name { get; set; }
        
        [BindProperty]
        public string? ImageSrc { get; set; }

        [BindProperty]
        public string? Option { get; set; }

        [BindProperty]
        public string? Type { get; set; }  //Beverage, dessert

        [BindProperty]
        public string? Details { get; set; }

        [BindProperty]
        public decimal? PriceTall { get; set; }

        [BindProperty]
        public decimal? PriceGrande { get; set; }
        [BindProperty]
        public decimal? PriceVenti { get; set; }

        [BindProperty]
        public decimal? PriceDessert { get; set; }

    }
}