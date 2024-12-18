using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.EF;

namespace TSMS_2_.DTO
{
    public class SaleDTO
    {
        public SaleDTO() {
            data = DateTime.Now;
        }

        public long id { get; set; }

        public DateTime? data { get; set; }// Время продажи

        [Required]
        public long cost { get; set; } // Стоимость продажи

        [Required]
        public long salesmn_id { get; set; } // ID продавца
        public long discount { get; set; }

        [Required]
        public long? client_id { get; set; } // ID клиента

        // Конструктор для преобразования из сущности sale
        public SaleDTO(sale s)
        {
            id = s.id;
            data = s.data;
            cost = s.cost;
            salesmn_id = s.salesmn_id;
            client_id = s.client_id;
            salesman = s.salesman;
            salesmnName = s.salesman.FullName;
            client = s.client;
            discount = s.discount;
            if (client!=null)
                clientNum = client.noomber;
        }

        // Конструктор для преобразования из другого DTO (если нужно)
        public SaleDTO(SaleDTO s)
        {
            if (s != null)
            {
                id = s.id;
                data = s.data;
                cost = s.cost;
                salesmn_id = s.salesmn_id;
                client_id = s.client_id;
                salesman = s.salesman;
                discount= s.discount;
                salesmnName = s.salesman.FullName;
                client = s.client;
                clientNum = s.clientNum;
            }
        }
        public string salesmnName { get; set; }
        public virtual salesman salesman { get; set; }
        public string clientNum { get; set; }
        public virtual client client { get; set; }
    }
}
