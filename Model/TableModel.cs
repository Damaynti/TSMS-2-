using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.DTO;
using TSMS_2_.EF;

namespace TSMS_2_.Model
{
    using Salesman = TSMS_2_.EF.salesman;
    public class TableModel
    {
        Model1 db = new Model1();
        public List<salesmanDTO> GetSalesmanDTO()
        {
            db.salesman.Load();
            return db.salesman.Select(i => new salesmanDTO(i)).ToList();
        }
        public salesman ValidateUser(bool r,string password)
        {
            return db.salesman.FirstOrDefault(u => u.password == password && u.admin == r);
        }
        public List<Salesman> GetSalesman()
        {
            return db.salesman.Include(o => o.FullName) // Загружаем владельцев
                            .Include(o => o.mail)
                            .Include(o => o.salary)
                            .Include(o => o.password)
                            .Include(o=>o.number)
                            .Include(o=>o.address)
                            .ToList();
        }
    }
}
