namespace TSMS_2_.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.supplier")]
    public partial class supplier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public supplier()
        {
            loanAgreement = new HashSet<loanAgreement>();
            supply = new HashSet<supply>();
        }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string address { get; set; }

        [Required]
        public string mail { get; set; }

        [Required]
        public string requisites { get; set; }

        [Required]
        public string number { get; set; }

        public long id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<loanAgreement> loanAgreement { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<supply> supply { get; set; }
    }
}
