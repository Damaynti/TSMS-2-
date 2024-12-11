using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TSMS_2_.EF;

namespace TSMS_2_.DTO
{
    public class CategoryDto
    {
        public CategoryDto() { }

        public long Id { get; set; }

        [Required]
        [StringLength(100)] // Set length restriction if necessary
        public string Name { get; set; }

        public List<ProductsDTO> Products { get; set; }

        // Constructor to map from category entity
        public CategoryDto(categories category)
        {
            Id = category.id;
            Name = category.name;
            Products = new List<ProductsDTO>();

            foreach (var product in category.products)
            {
                Products.Add(new ProductsDTO(product));
            }
        }
    }
}