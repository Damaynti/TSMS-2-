namespace TSMS_2_.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.loanAgreement")]
    public partial class loanAgreement
    {
        public long id { get; set; }

        public long sup_id { get; set; }

        public long sum { get; set; }

        public long percent { get; set; }

        public long status_id { get; set; }

        public DateTime? start { get; set; }

        public DateTime? end { get; set; }

        public long end_sum { get; set; }

        public virtual status status { get; set; }

        public virtual supply supply { get; set; }
    }
}
