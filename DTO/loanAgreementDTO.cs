using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.EF;

namespace TSMS_2_.DTO
{
    public class loanAgreementDTO
    {
        public loanAgreementDTO() { }

        public long id { get; set; }

        public long supplier_id { get; set; }

        public long sum { get; set; }

        public long percent { get; set; }

        public long status_id { get; set; }

        public DateTime? start { get; set; }

        public TimeSpan? end { get; set; }
        public virtual status status { get; set; }

        public virtual supplier supplier { get; set; }
        public loanAgreementDTO(loanAgreement m)
        {
            id = m.id;
            supplier_id = m.supplier_id;
            sum = m.sum;
            percent = m.percent;
            status = m.status;
            supplier = m.supplier;
            status_id = m.status_id;
            start = m.start;
            end = m.end;
        }

        public loanAgreementDTO(loanAgreementDTO m)
        {
            if (m != null)
            {
                id = m.id;
                supplier_id = m.supplier_id;
                sum = m.sum;
                status = m.status;
                supplier = m.supplier;
                percent = m.percent;
                status_id = m.status_id;
                start = m.start;
                end = m.end;
            }
        }
    }
}