using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.DTO;
using TSMS_2_.EF;

namespace TSMS_2_.Model
{
    internal class SupplierModel
    {
        private Model1 db = new Model1();

        public void CreateSupplier(SupplierDTO s)
        {
            supplier newSupplier = new supplier
            {
                FullName = s.FullName,
                CompanyName = s.CompanyName,
                address = s.address,
                mail = s.mail,
                requisites = s.requisites,
                number = s.number
            };
            db.supplier.Add(newSupplier);
            db.SaveChanges();
        }
        public void UpdateSupplier(SupplierDTO s)
        {
            supplier existingSupplier = db.supplier.Find(s.id);
            if (existingSupplier != null)
            {
                existingSupplier.FullName = s.FullName;
                existingSupplier.CompanyName = s.CompanyName;
                existingSupplier.address = s.address;
                existingSupplier.mail = s.mail;
                existingSupplier.requisites = s.requisites;
                existingSupplier.number = s.number;

                db.SaveChanges();
            }
        }

        public void DeleteSupplier(long id)
        {
            supplier supplierToDelete = db.supplier.Find(id);
            if (supplierToDelete != null)
            {
                db.supplier.Remove(supplierToDelete);
                db.SaveChanges();
            }
        }

       
    }
}
