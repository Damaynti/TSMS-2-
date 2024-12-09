using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.DTO;
using TSMS_2_.EF;

namespace TSMS_2_.Model
{
    internal class Element_saleModel
    {
        private Model1 db = new Model1();

        // Метод для создания нового элемента продажи
        public void CreateElementSale(Element_saleDto elementSaleDto)
        {
            element_sale newElementSale = new element_sale
            {
                quentity = elementSaleDto.Quantity,
                sale_id = elementSaleDto.sale_id,
                price=elementSaleDto.price,
                products_id = elementSaleDto.products_id
            };

            db.element_sale.Add(newElementSale);
            db.SaveChanges();
        }

        // Метод для обновления существующего элемента продажи
        public void UpdateElementSale(Element_saleDto elementSaleDto)
        {
            element_sale existingElementSale = db.element_sale.Find(elementSaleDto.Id);
            if (existingElementSale != null)
            {
                existingElementSale.quentity = elementSaleDto.Quantity;
                existingElementSale.sale_id = elementSaleDto.sale_id;
                existingElementSale.products_id = elementSaleDto.products_id;

                db.SaveChanges();
            }
        }

        // Метод для удаления элемента продажи по ID
        public void DeleteElementSale(long id)
        {
            element_sale elementToDelete = db.element_sale.Find(id);
            if (elementToDelete != null)
            {
                db.element_sale.Remove(elementToDelete);
                db.SaveChanges();
            }
        }

    }
}
