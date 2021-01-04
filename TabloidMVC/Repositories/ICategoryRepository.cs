using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();

        public void Add(Category category);

        public void Delete(Category category);

        public Category GetCategoryById(int id);

        public void Edit(Category category);
    }
}