using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.DTO;
using TSMS_2_.EF;
using TSMS_2_.View;

namespace TSMS_2_.Model
{
    internal class ClientModel
    {
        Model1 db = new Model1();


        public long CreateClient(string noomber)
        {
            client newClient = new client
            {
                noomber = noomber,
                discount_id = 5,
            };
            db.client.Add(newClient);
            db.SaveChanges();

            return newClient.id;
        }

        public void UpdateClient(ClientDTO p)
        {
            client ph = db.client.Find(p.id);
            if (ph != null)
            {
                ph.noomber = p.noomber;
                ph.discount = p.discount;

                db.SaveChanges();
            }
        }

        public void DeleteClient(long id)
        {
            client p = db.client.Find(id);
            if (p != null)
            {
                db.client.Remove(p);
                db.SaveChanges();
            }
        }
    }
}
