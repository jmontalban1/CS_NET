using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using Northwind.Data.Entities;
using NorthwindSystem.DAL;
using System.ComponentModel; // is to expose the methods for use by ODS
#endregion

namespace NorthwindSystem.BLL
{
    [DataObject]
    public class CategoryController
    {

        //to expose a method for use by ODS
        //you need to add the annotation
        //[DataObjectMethod(...)]
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<Category> Category_List()
        {
            using (var context = new NorthwindContext())
            {
                return context.Categories.ToList();
            }
        }

        public Category Category_Get(int categoryid)
        {
            using (var context = new NorthwindContext())
            {
                return context.Categories.Find(categoryid);
            }
        }
    }
}
