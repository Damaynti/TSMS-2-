using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.DTO;
using TSMS_2_.EF;

namespace TSMS_2_.Model
{
    internal class CategoryModel
    {
        private Model1 db = new Model1();

        // Метод для создания новой категории
        public void CreateCategory(CategoryDto categoryDto)
        {
            categories newCategory = new categories
            {
                name = categoryDto.Name
            };
            db.categories.Add(newCategory);
            db.SaveChanges();
        }

        // Метод для обновления существующей категории
        public void UpdateCategory(CategoryDto categoryDto)
        {
            categories existingCategory = db.categories.Find(categoryDto.Id);
            if (existingCategory != null)
            {
                existingCategory.name = categoryDto.Name;
                db.SaveChanges();
            }
        }

        // Метод для удаления категории по ID
        public void DeleteCategory(long id)
        {
            categories categoryToDelete = db.categories.Find(id);
            if (categoryToDelete != null)
            {
                db.categories.Remove(categoryToDelete);
                db.SaveChanges();
            }
        }

        // Метод для получения списка всех категорий
        public List<CategoryDto> GetAllCategories()
        {
            return db.categories
                .Select(c => new CategoryDto(c))
                .ToList();
        }

        // Метод для получения категории по ID
        public CategoryDto GetCategoryById(long id)
        {
            categories category = db.categories.Find(id);
            return category != null ? new CategoryDto(category) : null;
        }
    }

}
