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
        [StringLength(200)] // Увеличиваем длину для полного имени
        public string FullName { get; set; }

        [Required]
        [StringLength(200)] // Увеличиваем длину для названия компании
        public string CompanyName { get; set; }

        [Required]
        [StringLength(300)] // Увеличиваем длину для адреса
        public string address { get; set; }

        [Required]
        [EmailAddress] // Добавляем валидацию для email
        public string mail { get; set; }

        [Required]
        [StringLength(500)] // Увеличиваем длину для реквизитов
        public string requisites { get; set; }

        [Required]
        [StringLength(20)] // Увеличиваем длину для номера
        public string number { get; set; }

        // Конструктор для преобразования из сущности supplier
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

        // Конструктор для преобразования из другого DTO (если нужно)
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
