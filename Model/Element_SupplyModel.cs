using System;
using System.Collections.Generic;
using System.Linq;
using TSMS_2_.DTO;
using TSMS_2_.EF;

namespace TSMS_2_.Model
{
    internal class ElementSupplyModel
    {
        private Model1 db = new Model1();

        // Метод для создания нового элемента поставки
        public void CreateElementSupply(ElementSupplyDto elementSupplyDto)
        {
            element_supply newElementSupply = new element_supply
            {
                quentity = elementSupplyDto.Quantity,
                supply_id = elementSupplyDto.SupplyId,
                price = elementSupplyDto.Price,
                products_id = elementSupplyDto.ProductsId
            };

            db.element_supply.Add(newElementSupply);
            db.SaveChanges();
        }

        // Метод для обновления существующего элемента поставки
        public void UpdateElementSupply(ElementSupplyDto elementSupplyDto)
        {
            element_supply existingElementSupply = db.element_supply.Find(elementSupplyDto.Id);
            if (existingElementSupply != null)
            {
                existingElementSupply.quentity = elementSupplyDto.Quantity;
                existingElementSupply.supply_id = elementSupplyDto.SupplyId;
                existingElementSupply.price = elementSupplyDto.Price;
                existingElementSupply.products_id = elementSupplyDto.ProductsId;

                db.SaveChanges();
            }
        }

        // Метод для удаления элемента поставки по ID
        public void DeleteElementSupply(long id)
        {
            element_supply elementToDelete = db.element_supply.Find(id);
            if (elementToDelete != null)
            {
                db.element_supply.Remove(elementToDelete);
                db.SaveChanges();
            }
        }

        // Метод для получения всех элементов поставок
        public List<ElementSupplyDto> GetAllElementSupplies()
        {
            return db.element_supply
                     .Select(es => new ElementSupplyDto(es))
                     .ToList();
        }

        // Метод для получения элемента поставки по ID
        public ElementSupplyDto GetElementSupplyById(long id)
        {
            var elementSupply = db.element_supply.Find(id);
            return elementSupply != null ? new ElementSupplyDto(elementSupply) : null;
        }
    }
}
