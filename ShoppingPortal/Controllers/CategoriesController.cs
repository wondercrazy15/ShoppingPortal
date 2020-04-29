using AutoMapper;
using Core.Interfaces;
using Data;
using Domain;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingPortal.Controllers
{
    //[Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesRepository _repo;

        public CategoriesController(ICategoriesRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Index 
        /// </summary>
        /// <returns>Index View</returns>
        public ActionResult Index()
        {
            try
            {
                // var data = _repo.GetCategories();

                // List<CategoryModel> categories = Mapper.Map<List<Category>, List<CategoryModel>>(data);
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }

        }
        
        /// <summary>
        /// Add Category into database
        /// </summary>
        /// <returns>Json Object with it status</returns>
        [HttpPost]
        public JsonResult AddCategory(CategoryModel entity)
        {
            try
            {
                Category cate = new Category();
                cate.CategoryName = entity.CategoryName;
                cate.Description = entity.Description;
                cate.IsActive = entity.IsActive;
                cate.ParentCategoryID = entity.ParentCategoryID;
                cate.IsDelete = false;
                cate.CreatedDate = DateTime.Now;
                cate.ModifiedDate = DateTime.Now;
                if (entity.ParentCategoryID != null) {
                    cate.Depth = (_repo.GetCategory(cate.ParentCategoryID).Depth + 1);
                }
                else
                {
                    cate.Depth = 1;
                    cate.ParentCategoryID = 1;
                }
              
                var data = _repo.AddCategory(cate);
                if (data > 0)
                {
                    var resp = new
                    {
                        result = true,
                        message = "Category Added Successfully.",
                        AddedId = data
                    };
                    return Json(resp, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var resp = new
                    {
                        result = false,
                        message = "Something went wrong will adding category.",
                        AddedId = 0
                    };
                    return Json(resp, JsonRequestBehavior.AllowGet);

                }

            }
            catch (Exception ex)
            {

                var resp = new
                {
                    result = false,
                    message = ex.Message,
                    AddedId = 0
                };
                return Json(resp, JsonRequestBehavior.AllowGet);

            }
        }


        /// <summary>
        /// Update Category into database
        /// </summary>
        /// <returns>Json Object with it status</returns>
        [HttpPost]
        public JsonResult UpdateCategory(CategoryModel entity)
        {
            try
            {
                if (entity.ID > 0)
                {
                    Category cate = _repo.GetCategory(entity.ID);
                    cate.CategoryName = entity.CategoryName;
                    cate.Description = entity.Description;
                    cate.IsActive = entity.IsActive;
                    
                    cate.ModifiedDate = DateTime.Now;
                    if (entity.ParentCategoryID != null)
                    {
                        if (cate.ParentCategoryID != entity.ParentCategoryID) {
                            cate.Depth = (_repo.GetCategory(entity.ParentCategoryID).Depth + 1);
                        }
                        cate.ParentCategoryID = entity.ParentCategoryID;

                    }
                    else {
                        //1 is root category id
                        cate.ParentCategoryID = 1;
                        cate.Depth = 1;
                    }

                    var data = _repo.UpdateCategory(cate);
                    if (data)
                    {
                        var resp = new
                        {
                            result = true,
                            message = "Category Update Successfully.",
                            AddedId = data
                        };
                        return Json(resp, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var resp = new
                        {
                            result = false,
                            message = "Something went wrong will updating category.",
                            AddedId = 0
                        };
                        return Json(resp, JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                    var resp = new
                    {
                        result = false,
                        message = "Something went wrong will updating category.",
                        AddedId = 0
                    };
                    return Json(resp, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {

                var resp = new
                {
                    result = false,
                    message = ex.Message,
                };
                return Json(resp, JsonRequestBehavior.AllowGet);

            }
        }

        /// <summary>
        /// Delete Category into database
        /// </summary>
        /// <returns>Json Object with it status</returns>
        [HttpPost]
        public ActionResult Delete(int cateId = 0)
        {
            try
            {
                bool status = false;
                if (cateId > 0)
                {
                    status = _repo.Delete(cateId);
                    if (status)
                    {
                        var resp = new
                        {
                            result = true,
                            message = "Category Deleted Successfully."
                        };
                        return Json(resp, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var resp = new
                        {
                            result = false,
                            message = "Something went wrong will deleting category."
                        };
                        return Json(resp, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var resp = new
                    {
                        result = false,
                        message = "Id must be required."
                    };

                    return Json(resp, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                var resp = new
                {
                    result = false,
                    message = ex.Message,
                    AddedId = 0
                };
                return Json(resp, JsonRequestBehavior.AllowGet);

            }
        }

        /// <summary>
        /// Retrive all categories except delete
        /// </summary>
        /// <returns>Json Object with it status</returns>
        [HttpPost]
        public JsonResult GetAllCategories()
        {
            try
            {
                var data = _repo.GetCategories();//.Select(x=>new  { x.CategoryName,x.Description});
                var resp = new
                {
                    result = true,
                    message = "Category Retrive Successfully.",
                    list = data
                };
                return Json(resp, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var resp = new
                {
                    result = false,
                    message = ex.Message
                };
                return Json(resp, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Retrive single category
        /// </summary>
        /// <returns>Json Object with it status</returns>
        [HttpPost]
        public JsonResult GetCategoryById(int cateId = 0)
        {
            try
            {
                var categ = _repo.GetCategory(cateId);
                if (categ != null)
                {
                    CategoryModel categoryModel = new CategoryModel();
                    categoryModel.ID = categ.ID;
                    categoryModel.CategoryName = categ.CategoryName;
                    categoryModel.Description = categ.Description;
                    categoryModel.IsActive = (categ.IsActive == null || categ.IsActive == false ? false : true);
                    categoryModel.IsDelete = categ.IsDelete;
                    categoryModel.ParentCategoryID = categ.ParentCategoryID;
                    categoryModel.CreatedDate = categ.CreatedDate;
                    categoryModel.ModifiedDate = categ.ModifiedDate;
                    if (categ.ParentCategoryID != null)
                    {
                        var paraentcateg = _repo.GetCategory(categ.ParentCategoryID);
                        if (paraentcateg != null)
                            categoryModel.ParentCategoryName = paraentcateg.CategoryName;
                    }

                    var resp = new
                    {
                        result = true,
                        message = "Category Retive Successfully",
                        obj = categoryModel
                    };
                    return Json(resp, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var resp = new
                    {
                        result = false,
                        message = "Category Not Found",
                    };
                    return Json(resp, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var resp = new
                {
                    result = false,
                    message = ex.Message,
                    AddedId = 0
                };
                return Json(resp, JsonRequestBehavior.AllowGet);

            }
        }

        /// <summary>
        /// Search category by passing search text
        /// </summary>
        /// <returns>Json Object with it status</returns>
        [HttpPost]
        public JsonResult SearchCategory(string searchtext)
        {
            try
            {
                if (String.IsNullOrEmpty(searchtext))
                {
                    var resp = new
                    {
                        result = true,
                        message = "Category Retrive Successfully.",
                        list = _repo.GetCategories()
                    };
                    return Json(resp, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var listwithroot = new List<Category>();
                    var data = _repo.SearchCategoryByText(searchtext);//.Select(x=>new  { x.CategoryName,x.Description});
                    if (data != null)
                    {
                        //Root category is required for list category and its sun category
                        //Do not delete root category
                        //Make sure your root category id is 1 and it is alwas at first row in category table.
                        var root = new Category
                        {
                            ID = 1,
                            CategoryName = "Root"
                        };
                        listwithroot.Add(root);
                        listwithroot.AddRange(data);
                        var resp = new
                        {
                            result = true,
                            message = "Category Retrive Successfully.",
                            list = listwithroot
                        };
                        return Json(resp, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        var resp = new
                        {
                            result = false,
                            message = "Category Not Found",
                        };
                        return Json(resp, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                var resp = new
                {
                    result = false,
                    message = ex.Message,
                };
                return Json(resp, JsonRequestBehavior.AllowGet);

            }
        }

        /// <summary>
        /// Update Category status
        /// </summary>
        /// <returns>Json Object with it status</returns>
        [HttpPost]
        public JsonResult UpdateStatus(int id,bool status)
        {
            try
            {
                if (id > 0)
                {
                    Category cate = _repo.GetCategory(id);
                    cate.IsActive = status;
                    cate.ModifiedDate = DateTime.Now;                    
                    var data = _repo.UpdateCategory(cate);
                    if (data)
                    {
                        var resp = new
                        {
                            result = true,
                            message = "Status Update Successfully.",
                            AddedId = data
                        };
                        return Json(resp, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var resp = new
                        {
                            result = false,
                            message = "Something went wrong will updating category.",
                            AddedId = 0
                        };
                        return Json(resp, JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                    var resp = new
                    {
                        result = false,
                        message = "Something went wrong will updating category.",
                        AddedId = 0
                    };
                    return Json(resp, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {

                var resp = new
                {
                    result = false,
                    message = ex.Message,
                };
                return Json(resp, JsonRequestBehavior.AllowGet);

            }
        }
        

    }
}