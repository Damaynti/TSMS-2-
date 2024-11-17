namespace TSMS_2_.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.salesman")]
    public partial class salesman
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public salesman()
        {
            sale = new HashSet<sale>();
        }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string number { get; set; }

        [Required]
        public string address { get; set; }

        [Required]
        public string mail { get; set; }

        [Required]
        public string password { get; set; }

        public long salary { get; set; }

        public long id { get; set; }

        public bool admin { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sale> sale { get; set; }
    }
}
