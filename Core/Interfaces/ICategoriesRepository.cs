using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
   public interface ICategoriesRepository
    {
        IEnumerable<Category> GetCategories();

        IEnumerable<Category> SearchCategoryByText(string searchText);
        bool Delete(int id);
        Category GetCategory(int? userId);

        int AddCategory(Category category);
        bool UpdateCategory(Category category);
    }
}
