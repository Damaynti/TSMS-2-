using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TSMS_2_.EF;

namespace TSMS_2_.DTO
{
    public class ProductsDTO
    {
        public ProductsDTO() { }

        public long id { get; set; }

        [Required]
        [StringLength(100)] 
        public string name { get; set; }

        public long categoris_id { get; set; }

        [Required]
        public long price { get; set; }
        public string tex { get; set; }
        public long? count { get; set; }
        public long purchase { get; set; }
        public ProductsDTO(products p)
        {
            id = p.id;
            tex = p.tex;
            name = p.name;
            categoris_id = p.categoris_id;
            price = p.price;
            count = p.count;
            categories = p.categories;
            categorisName = p.categories.name;
            purchase=p.purchase;
        }

        public ProductsDTO(ProductsDTO p)
        {
            if (p != null && p.id!=0)
            {
                id = p.id;
                tex = p.tex;
                name = p.name;
                categoris_id = p.categoris_id;
                price = p.price;
                count = p.count;
                categories = p.categories;
                categorisName = p.categories.name;
                purchase=p.purchase;
            }
        }
        public string categorisName { get; set; }
        public virtual categories categories { get; set; }
    }
}
