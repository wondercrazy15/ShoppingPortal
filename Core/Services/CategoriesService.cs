using Core.Interfaces;
using Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class CategoriesService : ICategoriesRepository
    {
        private ShoppingPortalEntities _context;

        public CategoriesService(ShoppingPortalEntities context)
        {
            this._context = context;
        }
        public IEnumerable<Category> GetCategories()
        {
            IEnumerable<Category> categoryDetails = null;
            try
            {
                var data = _context.Categories.ToList().Where(x=>x.IsDelete==false);
                //Need to convert data to Category list because of self join.
                categoryDetails = data.Select(x => new Category
                {
                    CategoryName = x.CategoryName,
                    Description = x.Description,
                    ID = x.ID,
                    ParentCategoryID = x.ParentCategoryID,
                    IsActive = x.IsActive,
                    IsDelete = x.IsDelete,
                    CreatedDate = x.CreatedDate,
                    ModifiedDate = x.ModifiedDate,
                    Depth=x.Depth
                    
                }).ToList();
            }
            catch (Exception ex)
            {

            }
            return categoryDetails;
        }

        public IEnumerable<Category> SearchCategoryByText(string searchText)
        {
            IEnumerable<Category> categoryDetails = null;
            try
            {
                var data = _context.Categories.Where(x => x.CategoryName.ToLower().Contains(searchText) && x.IsDelete==false).ToList();
                if (data != null) { 
                categoryDetails = data.Select(x => new Category
                {
                    CategoryName = x.CategoryName,
                    Description = x.Description,
                    ID = x.ID,
                    ParentCategoryID = x.ParentCategoryID,
                    IsActive = x.IsActive,
                    IsDelete = x.IsDelete,
                    CreatedDate = x.CreatedDate,
                    ModifiedDate = x.ModifiedDate,
                    Depth = x.Depth

                }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return categoryDetails;
        }
        
        public Category GetCategory(int? catId)
        {
            Category result = new Category(); 
            try
            {
                if (catId > 0)
                {
                    var res = _context.Categories.AsNoTracking().Where(x => x.ID == catId).FirstOrDefault();
                    //Need to convert res to Category class because of self join
                    result.ID = res.ID;
                    result.CategoryName = res.CategoryName;
                    result.Description = res.Description;
                    result.IsActive = (res.IsActive == null || res.IsActive==false ? false : true);
                    result.IsDelete = res.IsDelete;
                    result.ParentCategoryID = res.ParentCategoryID;
                    result.CreatedDate = res.CreatedDate;
                    result.ModifiedDate = res.ModifiedDate;
                    result.Depth = res.Depth;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public bool Delete(int catId)
        {
            Category categoryExists = null;
            try
            {
                if (catId > 0)
                {
                    categoryExists = GetCategory(catId);

                    if (categoryExists != null)
                    {
                        categoryExists.IsDelete = true;
                        _context.Entry(categoryExists).State = EntityState.Modified;
                        if (_context.SaveChanges() > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        };
                        //_context.Categories.Remove(categoryExists);

                        //if (_context.SaveChanges() > 0)
                        //    return true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public int AddCategory(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return category.ID;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool UpdateCategory(Category category)
        {
            try
            {                
                _context.Entry(category).State = EntityState.Modified;
                if (_context.SaveChanges() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                };

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
