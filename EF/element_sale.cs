namespace TSMS_2_.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.element_sale")]
    public partial class element_sale
    {
        public long quentity { get; set; }

        public long sale_id { get; set; }

        public long products_id { get; set; }

        public long price { get; set; }

        public long id { get; set; }

        public virtual products products { get; set; }

        public virtual sale sale { get; set; }
    }
}
