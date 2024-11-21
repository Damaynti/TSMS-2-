using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.DTO;
using TSMS_2_.EF;

namespace TSMS_2_.Model
{
    internal class SupplyModel
    {
        private Model1 db = new Model1();

        // Метод для создания новой поставки
        public void CreateSupply(SupplyDTO s)
        {
            supply newSupply = new supply
            {
                supplier_id = s.supplier_id,
                data = s.data,
                cost = s.cost
            };
            db.supply.Add(newSupply);
            db.SaveChanges();
        }

        // Метод для обновления существующей поставки
        public void UpdateSupply(SupplyDTO s)
        {
            supply existingSupply = db.supply.Find(s.id);
            if (existingSupply != null)
            {
                existingSupply.supplier_id = s.supplier_id;
                existingSupply.data = s.data;
                existingSupply.cost = s.cost;

                db.SaveChanges();
            }
        }

        // Метод для удаления поставки по ID
        public void DeleteSupply(long id)
        {
            supply supplyToDelete = db.supply.Find(id);
            if (supplyToDelete != null)
            {
                db.supply.Remove(supplyToDelete);
                db.SaveChanges();
            }
        }

        // Метод для получения списка всех поставок
       

        // Метод для получения поставки по ID
        public SupplyDTO GetSupplyById(long id)
        {
            supply supply = db.supply.Find(id);
            return supply != null ? new SupplyDTO(supply) : null;
        }
    }
}

