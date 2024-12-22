using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.EF;

namespace TSMS_2_.DTO
{
    public class Element_saleDto : INotifyPropertyChanged
    {
        public Element_saleDto() { }
        private long _quantity;
        private decimal _totalPrice;
        public long Id { get; set; }

        [Required]
        public long Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = (value);
                    OnPropertyChanged(nameof(Quantity));  
                    OnPropertyChanged(nameof(TotalPrice));  
                }
            }
        }

        public long sale_id { get; set; }

        public long products_id { get; set; }

        public long price { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal TotalPrice
        {
            get => ProductPrice * Quantity;
        }
        public virtual products products { get; set; }

        public virtual sale sale { get; set; }
        public Element_saleDto(element_sale elementSale)
        {
            Id = elementSale.id;
            Quantity = elementSale.quentity;
            price= elementSale.price;
            sale_id = elementSale.sale_id;
            products= elementSale.products;
            sale = elementSale.sale;
            products_id = elementSale.products_id;

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
                sale_id = dto.sale_id;
                price= dto.price;
                products_id = dto.products_id;
                sale= dto.sale;
                products = dto.products;
                ProductName = dto.ProductName;
                ProductPrice = dto.ProductPrice;
            }
        }

        


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
