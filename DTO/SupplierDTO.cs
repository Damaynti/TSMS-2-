using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.EF;

namespace TSMS_2_.DTO
{
    public class SupplierDTO
    {
        public SupplierDTO() { }

        public long id { get; set; }

        [Required]
        [StringLength(200)] 
        public string FullName { get; set; }

        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(300)] 
        public string address { get; set; }

        [Required]
        [EmailAddress] 
        public string mail { get; set; }

        [Required]
        [StringLength(500)]
        public string requisites { get; set; }

        [Required]
        [StringLength(20)] 
        public string number { get; set; }

        public SupplierDTO(supplier s)
        {
            id = s.id;
            FullName = s.FullName;
            CompanyName = s.CompanyName;
            address = s.address;
            mail = s.mail;
            requisites = s.requisites;
            number = s.number;
        }

        public SupplierDTO(SupplierDTO s)
        {
            if (s != null)
            {
                id = s.id;
                FullName = s.FullName;
                CompanyName = s.CompanyName;
                address = s.address;
                mail = s.mail;
                requisites = s.requisites;
                number = s.number;
            }
        }
    }
}
