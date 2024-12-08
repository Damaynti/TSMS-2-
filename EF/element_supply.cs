namespace TSMS_2_.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.element_supply")]
    public partial class element_supply
    {
        public long quentity { get; set; }

        public long supply_id { get; set; }

        public long products_id { get; set; }

        public long price { get; set; }

        public long id { get; set; }

        public virtual products products { get; set; }

        public virtual supply supply { get; set; }
    }
}
