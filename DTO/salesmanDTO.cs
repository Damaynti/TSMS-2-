using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.EF;

namespace TSMS_2_.DTO
{
    public class salesmanDTO
    {
        
        public salesmanDTO(){}
        public long id { get; set; }
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
        [Required]
        [StringLength(11)]
        public string number { get; set; }

        [Required]
        [StringLength(50)]
        public string address { get; set; }
        public string mail { get; set; }

        [Required]
        [StringLength(10)]
        public string password { get; set; }
        public long salary { get; set; }
        public bool admin { get; set; }

        public bool work { get; set; }
        public salesmanDTO(salesman m)
        {
            id = m.id;
            FullName = m.FullName;
            number = m.number;
            address = m.address;
            mail = m.mail;
            work = m.work;
            admin = m.admin;
            password = m.password;
            salary = m.salary;
        }
        public salesmanDTO(salesmanDTO m)
        {
            if (m != null)
            {
                id = m.id;
                FullName = m.FullName;
                number = m.number;
                address = m.address;
                mail = m.mail;
                work = m.work;
                admin = m.admin;
                password = m.password;
                salary =  m.salary;
            }
        }
    }
}
