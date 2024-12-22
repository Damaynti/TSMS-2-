using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.DTO;
using TSMS_2_.EF;

namespace TSMS_2_.Model
{
    internal class ProductsModel
    {
        private Model1 db = new Model1();

        public void CreateProduct(ProductsDTO p)
        {
            products newProduct = new products
            {
                name = p.name,
                categoris_id = p.categoris_id,
                price = (long)(p.purchase*1.7),
                count = p.count,
                purchase = p.purchase,
                tex=p.tex,
            };
            db.products.Add(newProduct);
            db.SaveChanges();
        }
        public void UpdateProduct(ProductsDTO p)
        {
            products existingProduct = db.products.Find(p.id);
            if (existingProduct != null)
            {
                existingProduct.name = p.name;
                existingProduct.categoris_id = p.categoris_id;
                existingProduct.price = p.price;
                existingProduct.count = p.count;

                db.SaveChanges();
            }
        }

        public void DeleteProduct(long id)
        {
            products productToDelete = db.products.Find(id);
            if (productToDelete != null)
            {
                db.products.Remove(productToDelete);
                db.SaveChanges();
            }
        }


        public void DecreaseProductQuantity(long productId, long quantity)
        {
            using (var db = new Model1())  
            {
                var product = db.products.FirstOrDefault(p => p.id == productId);

                if (product != null)
                {
                    product.count -= quantity; 
                    db.SaveChanges(); 
                }
            }
        }


        public void increaseProductQuantity(long productId, long quantity)
        {
            using (var db = new Model1())  
            {
                var product = db.products.FirstOrDefault(p => p.id == productId);

                if (product != null)
                {
                    product.count += quantity; 
                    db.SaveChanges(); 
                }
            }
        }

        public ProductsDTO GetProductById(long id)
        {
            products product = db.products.Find(id);
            return product != null ? new ProductsDTO(product) : null;
        }
    }
}
