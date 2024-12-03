using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.DTO;
using TSMS_2_.EF;

namespace TSMS_2_.Model
{
    internal class SalyModel
    {
        private Model1 db = new Model1();

        // Метод для создания новой продажи
        public long CreateSale(SaleDTO s)
        {
            sale newSale = new sale
            {
                data = s.data,
                cost = s.cost,
                salesmn_id = s.salesmn_id,
                client_id = s.client_id,
            };
            db.sale.Add(newSale);
            db.SaveChanges();
            return newSale.id;
        }

        // Метод для обновления существующей продажи
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

        // Метод для удаления продажи по ID
        public void DeleteSale(long id)
        {
            sale saleToDelete = db.sale.Find(id);
            if (saleToDelete != null)
            {
                db.sale.Remove(saleToDelete);
                db.SaveChanges();
            }
        }

        // Метод для получения списка всех продаж
       

        // Метод для получения продажи по ID
        public SaleDTO GetSaleById(long id)
        {
            sale sale = db.sale.Find(id);
            return sale != null ? new SaleDTO(sale) : null;
        }
    }
}
