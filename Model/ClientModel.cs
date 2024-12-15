using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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


        public long CreateClient(ClientDTO p)
        {
            client newClient = new client
            {
                noomber = p.noomber,
                name = p.name,
                physical_person = p.physical_person,
                purchase_amount = p.purchase_amount,
                discount_id = p.discount_id,
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
                ph.physical_person = p.physical_person;
                ph.name = p.name;

                db.SaveChanges();
            }
        }
        public void IncreaseClientTotalAmount(long clientId, long amount)
        {
            using (var db = new Model1())  // Предполагается использование контекста базы данных
            {
                var client = db.client.FirstOrDefault(c => c.id == clientId);
                var _tableModel = new TableModel();
                if (client != null)
                {
                    client.purchase_amount += amount;  // Увеличиваем общую сумму покупок клиента
                    var idD = _tableModel.FindDiscountIdByPurchaseAmount(client.purchase_amount);
                    if (idD != null) client.discount_id=(long)idD;
                    db.SaveChanges(); // Сохраняем изменения в базе данных
                }
            }
        }
        public void RemoveClientAssociations(long clientId)
        {
            using (var context = new Model1())
            {
                // Find all sales associated with the client
                var salesToUpdate = context.sale.Where(sale => sale.client_id == clientId).ToList();

                // Set client_id to null for these sales
                foreach (var sale in salesToUpdate)
                {
                    sale.client_id = null;
                }

                // Save changes to the database
                context.SaveChanges();
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
