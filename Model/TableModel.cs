using PdfSharp.Pdf.Filters;
using SkiaSharp;
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
       
        public List<salesmanDTO> GetSalesmanDTO()
        {

            using (var db = new Model1())
            {
                db.salesman.Load();
                List<salesmanDTO> r = db.salesman
                                         .Where(i => !i.admin) 
                                         .ToList()
                                         .Select(i => new salesmanDTO(i))
                                         .ToList();
                return r;
            }

        }

        public bool DoesClientNumberExist(string phoneNumber)
        {
            using (var context = new Model1())
            {
                return context.client.Any(client => client.noomber == phoneNumber);
            }
        }
        public bool DoesClientNumberExist(string number, long clientId)
        {
            using (var context = new Model1())
            {
                return context.client.Any(c => c.noomber == number && c.id != clientId);
            }
        }


        public long? FindDiscountIdByPurchaseAmount(long purchaseAmount)
        {
            using (var db = new Model1())
            {
                var discounts = db.discount.ToList();

                var discount = discounts.FirstOrDefault(d => purchaseAmount >= d.start && purchaseAmount <= d.end);

                return discount?.id;
            }
        }


        public List<ProductsDTO> GetProductsDTO(long? Id, string SearchTerm="")
        {
            using (var db = new Model1())
            {
                db.products.Load();
                List<ProductsDTO> product = db.products.ToList().Select(i => new ProductsDTO(i)).ToList();
                for (int i = 0; i < product.Count; i++)
                {
                    product[i].categorisName = db.categories.Find(product[i].categoris_id).name;
                }
                if (Id!=null)
                {
                    product = product.Where(p => p.id == Id && p.count != 0)
                    .ToList();
                }

                if (SearchTerm!=null && SearchTerm!="")
                {
                    product = product.Where(p => p.name.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0 && p.count != 0)
                    .ToList();
                }
                return product;
            }
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
            using (var db = new Model1())
            {
                db.supplier.Load();
                List<SupplierDTO> r = db.supplier.ToList().Select(i => new SupplierDTO(i)).ToList();

                return r;
            }
        }
       

        public string GetCategoryName(long categoryId)
        {
            var category = db.categories.FirstOrDefault(c => c.id == categoryId);
            return category?.name ?? "Unknown"; 
        }

        public long GetTotalRevenue(DateTime startDate, DateTime endDate)
        {
            var totalRevenue = db.sale
                .Where(s => s.data.HasValue && 
                            s.data.Value >= startDate && 
                            s.data.Value <= endDate) 
                .Select(s => (long?)s.cost) 
                .DefaultIfEmpty(0) 
                .Sum(); 

            return totalRevenue ?? 0; 
        }

        public Dictionary<long, long> GetTotalRevenueByCategories(DateTime startDate, DateTime endDate)
        {
            var revenueByCategories = db.element_sale
    .Include(es => es.products) 
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
            using (var db = new Model1()) 
            {
                db.loanAgreement.Load();
                return db.loanAgreement.Local.ToList().Select(la => new loanAgreementDTO(la)).ToList();
            }
        }

        public List<SupplyDTO> GetSupplyDTO()
        {
          
            var Supply = db.supply.ToList();

            return Supply.Select(i => new SupplyDTO(i)).ToList();
        }
        public List<ClientDTO> GetClientsDTO()
        {
            using (var context = new Model1())
            {
                var clients = context.client.Include("Discount").ToList();
                return clients.Select(client => new ClientDTO
                {
                    id = client.id,
                    name = client.name,
                    noomber = client.noomber,
                    purchase_amount = client.purchase_amount,
                    discount_id = client.discount_id,
                    _discount = client.discount?.size,
                    tClient = client.physical_person ? "Физическое лицо" : "Юридическое лицо",
                    physical_person = client.physical_person
                }).ToList();
            }
        }

        public List<SaleDTO> GetSaleDTO()
        {
            using (var db = new Model1()) 
            {
                db.sale.Load();
                return db.sale.Local.ToList().Select(i => new SaleDTO(i)).ToList();
            }
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
                             .Where(es => es.sale_id == saleId) 
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

            var totalRevenue = revenueByCategories.Values.Sum();

            if (totalRevenue == 0)
                return new Dictionary<string, double>(); 

            return revenueByCategories.ToDictionary(
                kvp => GetCategoryName(kvp.Key), 
                kvp => (double)kvp.Value / totalRevenue * 100 
            );
        }

     
        public int GetUniqueClientsCount(DateTime startDate, DateTime endDate)
        {
            var query = db.sale
                         .Where(s => s.data.HasValue &&
                                     s.data.Value >= startDate &&
                                     s.data.Value <= endDate)
                         .Distinct(); 

            return query.Count(); 
        }

        public List<CategoryDto> GetCategoriesDTO()
        {
            using (var context = new Model1())
            {
                return context.categories
                    .Select(c => new CategoryDto
                    {
                        Id = c.id,
                        Name = c.name,
                        Products = c.products.Select(p => new ProductsDTO
                        {
                            id = p.id,
                            name = p.name,
                            price = p.price
                        }).ToList()
                    }).ToList();
            }
        }

        public List<Salesman> GetSalesman()
        {
            return db.salesman.Include(o => o.FullName)
                            .Include(o => o.mail)
                            .Include(o => o.salary)
                            .Include(o => o.password)
                            .Include(o=>o.number)
                            .Include(o=>o.address)
                            .ToList();
        }
    }
}
