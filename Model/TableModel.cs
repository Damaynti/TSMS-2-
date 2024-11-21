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
        //public List<salesmanDTO> GetSalesmanDTO()
        //{
        //    db.salesman.Load();
        //    return db.salesman.Select(i => new salesmanDTO(i)).ToList();
        //}
        public List<salesmanDTO> GetSalesmanDTO()
        {

            using (var db = new Model1())
            {
                db.salesman.Load();
                List<salesmanDTO> r = db.salesman.ToList().Select(i => new salesmanDTO(i)).ToList();
              
                return r;
            }
            //// Получаем данные из базы данных без использования конструктора
            //var salesmen = db.salesman.ToList(); // Загружаем данные в память

            //// Преобразуем их в список DTO
            //return salesmen.Select(i => new salesmanDTO(i)).ToList();
        }


        public List<SupplierDTO> GetSupplierDTO()
        {
            // Получаем данные из базы данных без использования конструктора
            using (var db = new Model1())
            {
                db.supplier.Load();
                List<SupplierDTO> r = db.supplier.ToList().Select(i => new SupplierDTO(i)).ToList();

                return r;
            }
        }

        public List<loanAgreementDTO> GetLoanAgreementDTOs()
        {
            using (var db = new Model1()) // Предположим, что у вас есть контекст базы данных MyDbContext
            {
                db.loanAgreement.Load();
                return db.loanAgreement.Local.ToList().Select(la => new loanAgreementDTO(la)).ToList();
            }
        }

        public List<SupplyDTO> GetSupplyDTO()
        {
            // Получаем данные из базы данных без использования конструктора
            var Supply = db.supply.ToList(); // Загружаем данные в память

            // Преобразуем их в список DTO
            return Supply.Select(i => new SupplyDTO(i)).ToList();
        }

        public List<SaleDTO> GetSaleDTO()
        {
            // Получаем данные из базы данных без использования конструктора
            var Sale = db.sale.ToList(); // Загружаем данные в память

            // Преобразуем их в список DTO
            return Sale.Select(i => new SaleDTO(i)).ToList();
        }

        public List<salesman> GetSalesmans()
        {
            db.Set<salesman>().Load();
            return db.Set<salesman>().ToList();
        }
        public List<client> GetClients()
        {
            db.Set<client>().Load();
            return db.Set<client>().ToList();
        }
        public List<categories> GetCategories()
        {
            db.Set<categories>().Load();
            return db.Set<categories>().ToList();
        }

        public List<ProductsDTO> GetProductsDTO()
        {
            using (var db = new Model1())
            {
                db.products.Load();
                List<ProductsDTO> r = db.products.ToList().Select(i => new ProductsDTO(i)).ToList();
                for (int i = 0; i < r.Count; i++)
                {
                    r[i].categorisName = db.categories.Find(r[i].categoris_id).name;
                }
                return r;
            }
            
        }

        public List<Element_saleDto> GetElementsBySaleId(long saleId)
        {
            return db.element_sale
                             .Where(es => es.sale_id == saleId) // Фильтр по идентификатору продажи
                             .Select(es => new Element_saleDto
                             {
                                 Id = es.id,
                                 ProductName = es.products.name,
                                 Quantity = es.quentity
                                 // Другие свойства...
                             })
                             .ToList();
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
