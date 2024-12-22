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
    public class ElementSupplyDto : INotifyPropertyChanged
    {
        public ElementSupplyDto() { }

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
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity)); 
                    OnPropertyChanged(nameof(TotalPrice)); 
                }
            }
        }

        public long SupplyId { get; set; }

        public long ProductsId { get; set; }

        public long Price { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal TotalPrice
        {
            get => ProductPrice * Quantity;
        }

        public virtual products Products { get; set; }

        public virtual supply Supply { get; set; }

        public ElementSupplyDto(element_supply elementSupply)
        {
            Id = elementSupply.id;
            Quantity = elementSupply.quentity;
            Price = elementSupply.price;
            SupplyId = elementSupply.supply_id;
            Products = elementSupply.products;
            Supply = elementSupply.supply;
            ProductsId = elementSupply.products_id;

            if (elementSupply.products != null)
            {
                ProductName = elementSupply.products.name;
                ProductPrice = elementSupply.products.purchase;
            }
        }

        public ElementSupplyDto(ElementSupplyDto dto)
        {
            if (dto != null)
            {
                Id = dto.Id;
                Quantity = dto.Quantity;
                SupplyId = dto.SupplyId;
                Price = dto.Price;
                ProductsId = dto.ProductsId;
                Supply = dto.Supply;
                Products = dto.Products;
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
