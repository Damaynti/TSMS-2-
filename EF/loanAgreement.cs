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

        public long supplier_id { get; set; }

        public long sum { get; set; }

        public long percent { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan start { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan end { get; set; }

        public long status_id { get; set; }

        public virtual status status { get; set; }

        public virtual supplier supplier { get; set; }
    }
}
