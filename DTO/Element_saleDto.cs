using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.EF;

namespace TSMS_2_.DTO
{
    public class Element_saleDto
    {
        public Element_saleDto() { }

        public long Id { get; set; }

        [Required]
        public long Quantity { get; set; }

        public long SaleId { get; set; }

        public long ProductId { get; set; }

        public long price { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public Element_saleDto(element_sale elementSale)
        {
            Id = elementSale.id;
            Quantity = elementSale.quentity;
            price= elementSale.price;
            SaleId = elementSale.sale_id;
            ProductId = elementSale.products_id;

            if (elementSale.products != null)
            {
                ProductName = elementSale.products.name;
                ProductPrice = elementSale.products.price;
            }
        }

       
        public Element_saleDto(Element_saleDto dto)
        {
            if (dto != null)
            {
                Id = dto.Id;
                Quantity = dto.Quantity;
                SaleId = dto.SaleId;
                price= dto.price;
                ProductId = dto.ProductId;
                ProductName = dto.ProductName;
                ProductPrice = dto.ProductPrice;
            }
        }
    }
}
