namespace TSMS_2_.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.sale")]
    public partial class sale
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sale()
        {
            element_sale = new HashSet<element_sale>();
        }

        public long cost { get; set; }

        public long salesmn_id { get; set; }

        public long? client_id { get; set; }

        public DateTime? data { get; set; }

        public long id { get; set; }

        public long discount { get; set; }

        public virtual client client { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<element_sale> element_sale { get; set; }

        public virtual salesman salesman { get; set; }
    }
}
