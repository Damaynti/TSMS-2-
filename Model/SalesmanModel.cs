using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.DTO;
using TSMS_2_.EF;

namespace TSMS_2_.Model
{
    internal class SalesmanModel
    {
        Model1 db = new Model1();

       
        public void CreateSalesman(salesmanDTO p)
        {
            salesman newSalesman = new salesman
            {
                FullName = p.FullName,
                number = p.number,
                password = p.password,
                salary = p.salary,
                mail = p.mail,
                address = p.address
            };
            db.salesman.Add(newSalesman);
            db.SaveChanges();
        }

        public void UpdateSalesman(salesmanDTO p)
        {
            salesman ph = db.salesman.Find(p.id);
            if (ph != null)
            {
                ph.number = p.number;
                ph.password = p.password;
                ph.FullName = p.FullName;
                ph.salary = p.salary;
                ph.address = p.address;
                ph.mail = p.mail;

                db.SaveChanges();
            }
        }

        public void DeleteSalesman(long id)
        {
            salesman p = db.salesman.Find(id);
            if (p != null)
            {
                db.salesman.Remove(p);
                db.SaveChanges();
            }
        }

        
    }
}
