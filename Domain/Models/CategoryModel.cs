using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CategoryModel
    {
        public int ID { get; set; }
        [Display(Name ="Name")]
        [Required(ErrorMessage ="Category Name must be required")]
        public string CategoryName { get; set; }
        [Display(Name = "Description")]
        //[Required(ErrorMessage = "Description Name must be required")]
        public string Description { get; set; }
        [Display(Name = "Parent Category")]
        public Nullable<int> ParentCategoryID { get; set; }
        public bool IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string ParentCategoryName { get; set; }
    }
}
