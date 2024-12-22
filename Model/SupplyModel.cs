using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.DTO;
using TSMS_2_.EF;

namespace TSMS_2_.Model
{
    internal class SupplyModel
    {
        private Model1 db = new Model1();

        public long CreateSupply(SupplyDTO s)
        {
            supply newSupply = new supply
            {
                supplier_id = s.sup_id,
                data = s.data,
                cost = s.cost
            };
            db.supply.Add(newSupply);
            db.SaveChanges();
            return newSupply.id;
        }

        public void UpdateSupply(SupplyDTO s)
        {
            supply existingSupply = db.supply.Find(s.id);
            if (existingSupply != null)
            {
                existingSupply.supplier_id = s.sup_id;
                existingSupply.data = s.data;
                existingSupply.cost = s.cost;

                db.SaveChanges();
            }
        }
        public long CreatOrder(loanAgreementDTO _loanAg, long TotalSum, long idsal, ObservableCollection<Element_saleDto> CartItems)
        {
            long? CId = null;
            if (_loanAg != null) CId = _loanAg.id;

            var order = new SupplyDTO()
            {
                cost = TotalSum,
                sup_id = idsal,
                data=DateTime.Now,
            };

            var id = CreateSupply(order);

            if (_loanAg != null)
            {
                _loanAg.end_sum = _loanAg.sum*(100+_loanAg.percent)/100;
                _loanAg.sup_id = id;
                _loanAg.sum = TotalSum;
                _loanAg.status_id = 2;
                var loanAgModel = new loanAgreementModel();
                loanAgModel.CreateLoanAgreement(_loanAg);
            }

            for (int i = 0; i < CartItems.Count; i++)
            {
                var item = CartItems[i];

                var selectedElement = new ElementSupplyDto()
                {
                    SupplyId = id,
                    ProductsId = item.products_id,
                    Quantity = item.Quantity,
                    Price = (long)item.ProductPrice,
                };

                var elementSupplyModel = new ElementSupplyModel();
                {
                    elementSupplyModel.CreateElementSupply(selectedElement);

                    var productModel = new ProductsModel();
                    productModel.increaseProductQuantity(item.products_id, item.Quantity);
                }

                
            }
                return id;
            
        }

        public void DeleteSupply(long id)
        {
            supply supplyToDelete = db.supply.Find(id);
            if (supplyToDelete != null)
            {
                db.supply.Remove(supplyToDelete);
                db.SaveChanges();
            }
        }
       
        public SupplyDTO GetSupplyById(long id)
        {
            supply supply = db.supply.Find(id);
            return supply != null ? new SupplyDTO(supply) : null;
        }
    }
}

