using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSMS_2_.Model
{
    public class SalesReport
    {
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public double Quantity { get; set; }
    }
}
