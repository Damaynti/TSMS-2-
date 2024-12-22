using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.DTO;
using TSMS_2_.EF;

namespace TSMS_2_.Model
{
    internal class SalyModel
    {
        private Model1 db = new Model1();

        public long CreateSale(SaleDTO s)
        {
            sale newSale = new sale
            {
                data = s.data,
                cost = s.cost,
                salesmn_id = s.salesmn_id,
                client_id = s.client_id,
                discount = s.discount,
            };
            db.sale.Add(newSale);
            db.SaveChanges();
            return newSale.id;
        }

        public void UpdateSale(SaleDTO s)
        {
            sale existingSale = db.sale.Find(s.id);
            if (existingSale != null)
            {
                existingSale.data = s.data;
                existingSale.cost = s.cost;
                existingSale.salesmn_id = s.salesmn_id;
                existingSale.client_id = s.client_id;

                db.SaveChanges();
            }
        }

        public long CreatOrder(ClientDTO _client,long TotalSum, long idsal, ObservableCollection<Element_saleDto> CartItems)
        {
            long? CId = null;
            long D = 0;
            if (_client != null)
            {
                CId = _client.id;
                D=(long)_client._discount;
            }

            var order = new SaleDTO()
            {
                cost = TotalSum,
                salesmn_id = idsal,
                client_id = CId,
                discount=D,
            };

           var id= CreateSale(order);

            for (int i = 0; i < CartItems.Count; i++)
            {
                var item = CartItems[i];

                var selectedElement = new Element_saleDto()
                {
                    sale_id = id,
                    products_id = item.products_id,
                    Quantity = item.Quantity,
                    price = (long)item.ProductPrice,
                };

                var elementSaleModel = new Element_saleModel();
                elementSaleModel.CreateElementSale(selectedElement);

                var productModel = new ProductsModel();
                productModel.DecreaseProductQuantity(item.products_id, item.Quantity);
            }

            if (_client != null)
            {
                var clientModel = new ClientModel();
                clientModel.IncreaseClientTotalAmount(_client.id, order.cost);
            }

            return id;
        }

        public void DeleteSale(long id)
        {
            sale saleToDelete = db.sale.Find(id);
            if (saleToDelete != null)
            {
                db.sale.Remove(saleToDelete);
                db.SaveChanges();
            }
        }
       
        public SaleDTO GetSaleById(long id)
        {
            sale sale = db.sale.Find(id);
            return sale != null ? new SaleDTO(sale) : null;
        }
    }
}
