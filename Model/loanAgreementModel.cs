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
                supplier_id = dto.supplier_id,
                sum = dto.sum,
                percent = dto.percent,
                status_id = dto.status_id,
                start = dto.start,
                end = dto.end
            };
            db.loanAgreement.Add(newLoanAgreement);
            db.SaveChanges();
        }

        public void UpdateLoanAgreement(loanAgreementDTO dto)
        {
            loanAgreement la = db.loanAgreement.Find(dto.id);
            if (la != null)
            {
                la.supplier_id = dto.supplier_id;
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