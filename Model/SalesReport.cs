using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSMS_2_.Model
{
    public class SalesReport
    {
        public string Category { get; set; }  // Текстовое название категории
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }  // Возможно, Amount - это и есть стоимость
        public int Quantity { get; set; }
        public long TotalRevenue { get; set; }  // Добавьте это свойство для хранения суммы
    }

}
