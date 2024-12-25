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
    internal class loanAgreementModel
    {
        Model1 db = new Model1();
        public void CreateLoanAgreement(loanAgreementDTO dto)
        {
            loanAgreement newLoanAgreement = new loanAgreement
            {
                sup_id = dto.sup_id,
                sum = dto.sum,
                percent = dto.percent,
                status_id = dto.status_id,
                start = DateTime.Now,
                end = dto.end,
                end_sum = dto.end_sum,
            };
            db.loanAgreement.Add(newLoanAgreement);
            
            db.SaveChanges();
        }

        public void UpdateLoanAgreementStatus(loanAgreementDTO loanAgreement)
        {
            using (var context = new Model1())
            {
                var loan = context.loanAgreement.FirstOrDefault(l => l.id == loanAgreement.id);
                if (loan != null)
                {
                    loan.status_id = loanAgreement.status_id;
                    context.SaveChanges();
                }
            }
        }
        public void UpdateLoanAgreement(loanAgreementDTO dto)
        {
            loanAgreement la = db.loanAgreement.Find(dto.id);
            if (la != null)
            {
                la.sup_id = dto.sup_id;
                la.sum = dto.sum;
                la.percent = dto.percent;
                la.status_id = dto.status_id;
                la.start = dto.start;
                la.end = dto.end;

                db.SaveChanges();
            }
        }

        public void DeleteLoanAgreement(long id)
        {
            loanAgreement la = db.loanAgreement.Find(id);
            if (la != null)
            {
                db.loanAgreement.Remove(la);
                db.SaveChanges();
            }
        }
    }
}