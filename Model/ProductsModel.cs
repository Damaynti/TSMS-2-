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

        // Метод для создания нового продукта
        public void CreateProduct(ProductsDTO p)
        {
            products newProduct = new products
            {
                name = p.name,
                categoris_id = p.categoris_id,
                price = p.price,
                count = p.count,
                tex=p.tex,
            };
            db.products.Add(newProduct);
            db.SaveChanges();
        }

        // Метод для обновления существующего продукта
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

        // Метод для удаления продукта по ID
        public void DeleteProduct(long id)
        {
            products productToDelete = db.products.Find(id);
            if (productToDelete != null)
            {
                db.products.Remove(productToDelete);
                db.SaveChanges();
            }
        }

        // Метод для получения списка всех продуктов (по желанию)

        public void DecreaseProductQuantity(long productId, long quantity)
        {
            using (var db = new Model1())  // Предполагается использование контекста базы данных
            {
                var product = db.products.FirstOrDefault(p => p.id == productId);

                if (product != null)
                {
                    product.count -= quantity; // Уменьшаем количество товара
                    db.SaveChanges(); // Сохраняем изменения в базе данных
                }
            }
        }


        public void increaseProductQuantity(long productId, long quantity)
        {
            using (var db = new Model1())  // Предполагается использование контекста базы данных
            {
                var product = db.products.FirstOrDefault(p => p.id == productId);

                if (product != null)
                {
                    product.count += quantity; // Уменьшаем количество товара
                    db.SaveChanges(); // Сохраняем изменения в базе данных
                }
            }
        }

        // Метод для получения продукта по ID (по желанию)
        public ProductsDTO GetProductById(long id)
        {
            products product = db.products.Find(id);
            return product != null ? new ProductsDTO(product) : null;
        }
    }
}
