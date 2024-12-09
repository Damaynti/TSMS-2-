using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.EF;
using System.Diagnostics;

namespace TSMS_2_.DTO
{
    public class ClientDTO
    {
        public ClientDTO(){}

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long id { get; set; }

        [Required]
        public string noomber { get; set; }

        public long? purchase_amount { get; set; }

        public long discount_id { get; set; }

        public long? _discount {  get; set; }
        public bool physical_person { get; set; }

        public string name { get; set; }

        public virtual discount discount { get; set; }
        public ClientDTO(client client)
        {
            id = client.id;
            noomber = client.noomber;
            discount_id = client.discount_id;
            physical_person = client.physical_person;
            name = client.name;
            purchase_amount = client.purchase_amount;
            discount = client.discount;
            if (client.discount != null)
            {
                _discount = client.discount.size;
            }
        }

        public ClientDTO(ClientDTO client)
        {
            id = client.id;
            noomber = client.noomber;
            discount_id = client.discount_id;
            _discount= client._discount;
            physical_person = client.physical_person;
            discount = client.discount;
            name = client.name;
            purchase_amount = client.purchase_amount;
        }
       
    }
}
