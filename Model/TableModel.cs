using PdfSharp.Pdf.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

        public List<ProductsDTO> GetProductsDTOID(long Id, List<ProductsDTO> allProducts) {

            return allProducts
                    .Where(p => p.id == Id)
                    .ToList();
        }

        public List<ProductsDTO> GetProductsDTOName(string SearchTerm, List<ProductsDTO> allProducts) {

            return allProducts
                    .Where(p => p.name.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
        }

        public List<ClientDTO> GetClientDTO()
        {
            using (var db = new Model1())
            {
                db.client.Load();
                List<ClientDTO> r = db.client.ToList().Select(i => new ClientDTO(i)).ToList();

                return r;
            }
        }

        public ClientDTO GetClientDTOID(long id) {

            var allClients = GetClientDTO();

            var client = allClients
                .FirstOrDefault(p => p.id==id);


            return client;
        }



        public List<ClientDTO> GetnClientDTONoom(string SearchTerm, List<ClientDTO> allClients)
        {
            return allClients
                    .Where(p => p.noomber.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
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
       

        // Допустим, у вас есть метод в TableModel или в ViewModel, который будет преобразовывать ID категории в название
        public string GetCategoryName(long categoryId)
        {
            // Здесь предполагается, что у вас есть доступ к данным категорий
            var category = db.categories.FirstOrDefault(c => c.id == categoryId);
            return category?.name ?? "Unknown"; // Возвращаем имя категории или "Unknown" если не найдено
        }

        public long GetTotalRevenue(DateTime startDate, DateTime endDate)
        {
            var totalRevenue = db.sale
                .Where(s => s.data.HasValue && // Проверяем, что дата не пустая
                            s.data.Value >= startDate && // Фильтруем по начальной дате
                            s.data.Value <= endDate) // Фильтруем по конечной дате
                .Select(s => (long?)s.cost) // Преобразуем в long? для предотвращения ошибок с пустыми коллекциями
                .DefaultIfEmpty(0) // Если коллекция пуста, возвращаем 0
                .Sum(); // Суммируем поле cost

            return totalRevenue ?? 0; // Возвращаем сумму или 0, если результат null
        }

        public Dictionary<long, long> GetTotalRevenueByCategories(DateTime startDate, DateTime endDate)
        {
            var revenueByCategories = db.element_sale
    .Include(es => es.products) // Загружаем связанные данные
    .Where(es => es.sale.data.HasValue &&
                 es.sale.data.Value >= startDate &&
                 es.sale.data.Value <= endDate)
    .GroupBy(es => es.products.categoris_id)
    .Select(g => new
    {
        CategoryId = g.Key,
        TotalRevenue = g.Sum(es => (long?)(es.price * es.quentity)) ?? 0
    })
    .ToDictionary(x => x.CategoryId, x => x.TotalRevenue);


            return revenueByCategories;
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
                                 Quantity = es.quentity,
                                 price=es.price
                             })
                             .ToList();
        }
        public salesman ValidateUser(bool r,string password)
        {
            return db.salesman.FirstOrDefault(u => u.password == password && u.admin == r);
        }

        public Dictionary<string, double> GetCategoryRevenuePercentage(DateTime startDate, DateTime endDate)
        {
            var revenueByCategories = GetTotalRevenueByCategories(startDate, endDate);

            // Общая прибыль за указанный период
            var totalRevenue = revenueByCategories.Values.Sum();

            if (totalRevenue == 0)
                return new Dictionary<string, double>(); // Если нет прибыли, возвращаем пустой словарь

            // Вычисляем процент прибыли по категориям
            return revenueByCategories.ToDictionary(
                kvp => GetCategoryName(kvp.Key), // Имя категории
                kvp => (double)kvp.Value / totalRevenue * 100 // Процент от общей прибыли
            );
        }

     
        public int GetUniqueClientsCount(DateTime startDate, DateTime endDate)
        {
            // Получаем все продажи, которые попадают в указанный диапазон дат
            var query = db.sale
                         .Where(s => s.data.HasValue &&
                                     s.data.Value >= startDate &&
                                     s.data.Value <= endDate)
                         .Distinct(); // Убираем дубли по client_id

            return query.Count(); // Возвращаем количество уникальных клиентов
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
