namespace TSMS_2_.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.supply")]
    public partial class supply
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public supply()
        {
            element_supply = new HashSet<element_supply>();
            loanAgreement = new HashSet<loanAgreement>();
        }

        public long supplier_id { get; set; }

        public long cost { get; set; }

        public DateTime? data { get; set; }

        public long id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<element_supply> element_supply { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<loanAgreement> loanAgreement { get; set; }

        public virtual supplier supplier { get; set; }
    }
}
