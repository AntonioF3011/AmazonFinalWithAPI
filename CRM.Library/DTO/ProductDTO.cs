using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Library.DTO
{
    public  class ProductDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? Quantity { get; set; }

        public decimal Markdown { get; set; }

        public bool BuyoneGetone { get; set; }
        public string? ImageName { get; set; }

        public ProductDTO(Products p)
        {
            Name = p.Name;
            Description = p.Description;
            Price = p.Price;
            Quantity = p.Quantity;
            Markdown = p.Markdown;
            BuyoneGetone = p.BuyoneGetone;
            ImageName = p.ImageName;
            Id = p.Id;
        }
        public ProductDTO(ProductDTO p)
        {
            Name = p.Name;
            Description = p.Description;
            Price = p.Price;
            Quantity = p.Quantity;
            Markdown = p.Markdown;
            BuyoneGetone = p.BuyoneGetone;
            ImageName = p.ImageName;
            Id = p.Id;


        }
        public ProductDTO()
        {
            
        }
    }
}
