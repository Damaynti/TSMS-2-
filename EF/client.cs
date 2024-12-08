namespace TSMS_2_.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.client")]
    public partial class client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public client()
        {
            sale = new HashSet<sale>();
        }

        [Required]
        public string noomber { get; set; }

        public long? purchase_amount { get; set; }

        public long discount_id { get; set; }

        public long id { get; set; }

        public bool physical_person { get; set; }

        public string name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sale> sale { get; set; }

        public virtual discount discount { get; set; }
    }
}
