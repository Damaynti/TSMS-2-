using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.EF;

namespace TSMS_2_.DTO
{
    public class SupplyDTO
    {
        public SupplyDTO() { }

        public long id { get; set; }

        [Required]
        public long supplier_id { get; set; } // ID поставщика

        [Required]
        public DateTime? data { get; set; }// Дата поставки

        [Required]
        public long cost { get; set; } // Стоимость поставки

        // Конструктор для преобразования из сущности supply
        public SupplyDTO(supply s)
        {
            id = s.id;
            supplier_id = s.supplier_id;
            data = s.data;
            cost = s.cost;
            supplier = s.supplier;
            supplierName = s.supplier.FullName;
        }

        // Конструктор для преобразования из другого DTO (если нужно)
        public SupplyDTO(SupplyDTO s)
        {
            if (s != null)
            {
                id = s.id;
                supplier_id = s.supplier_id;
                data = s.data;
                cost = s.cost;
                supplier = s.supplier;
                supplierName = s.supplierName;
            }
        }
        public string supplierName { get; set; }
        public virtual supplier supplier { get; set; }
    }
}
