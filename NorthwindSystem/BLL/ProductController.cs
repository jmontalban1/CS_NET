
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using Northwind.Data.Entities;
using NorthwindSystem.DAL;
using System.Data.SqlClient;
using System.ComponentModel;
#endregion

namespace NorthwindSystem.BLL
{
    [DataObject]
    public class ProductController
    {
       
        //interface method for obtaining all of the
        //    Products Table records
        public List<Product> Products_List()
        {
            //setup a transaction process within this
            //class that uses the NorthwindContext class
            using (var context = new NorthwindContext())
            {
                //Entityframework has built-in a number of
                //   precode extensions that do very common
                //   process, such as retrieve all records
                //   from a DbSet<T>
                //By using the context instance which inherits
                //   the EntityFramework DbContext class
                //   we can get to these extensions
                //The dataset is return from DbContext as an
                //   IEnumerable<T> datatype
                //We can use .ToList() to convert the IEnumerable
                //   dataset into a List<T> dataset
                return context.Products.ToList();
            }
        }

        //interface method to get a specific Product table record
        //     by a given parameter value
        //in this case the parameter is the Product table primary key
        public Product Products_Get(int productid)
        {
            using (var context = new NorthwindContext())
            {
                //we will use the EntityFramework extension method
                //   .Find()
                //this method takes a primary key value and searches
                //   the associated sql table for that primary key
                return context.Products.Find(productid);
            }
        }

        //calling an sql procedure using EntityFramework
        //this mthod is not an extension like List or Get above
        //this method will need an additional namespace System.Data.SqlClient
        //this method needs to know the sql procedure name
        //     and any parameters
        //Syntax and spelling will be important
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<Product> Product_GetByPartialName(string partialname)
        {
            using (var context = new NorthwindContext())
            {
                //data will be returned as an IEnumerable<T> dataset
                //this dataset can be converted to a List<T> by using 
                //     .ToList()
                //the DbSet<T> is not used
                //the method Database.SqlQuery<T>() is used to
                //     execute the database query
                //<T> represents the data class container description
                //     which in this case is also the DbSet<T> description
                //the parameters of the query is 
                //  a) the call of the sql procedure with parameters
                //  b) a list of SqlParameter() instance(s); each instance
                //          representing a parameter in the sql procedure call
                //     the instance has two entries, the parameter name and
                //          the value for the parameter
                var results = context.Database.SqlQuery<Product>(
                    "Products_GetByPartialProductName @PartialName",
                    new SqlParameter("PartialName",partialname));
                return results.ToList();
            }
        }
        //Add Method
        [DataObjectMethod(DataObjectMethodType.Insert,false)]
        public int Product_Add(Product item)
        {
            //input is an instance of all data for an entity
            //one could send in individual values in separate
            //    parameters BUT eventaully, they would need
            //    to be place in an instance of the entity
            //changes to the database should be done in a transaction
            using (var context = new NorthwindContext())
            {
                //stage new record to DbSet<T> for commitment to 
                //    the database
                //the new record is not YET physically on the 
                //    database
                context.Products.Add(item);

                //commit the staged record to the database table
                //if this statement is NOT executed then the
                //    insert is not completed on the database table (Rollback)
                //the new identity value is created on the successful
                //    execution of the statement
                //the identity value is NOT available until the execution
                //    of the statement is complete
                //during execution of this statemtent ANY entity validation
                //    annotation is executed
                context.SaveChanges();

                //optionally one could return the new identity value
                //    after the SaveChanges has been done
                return item.ProductID;
            }
        }
        //Update Method
        [DataObjectMethod(DataObjectMethodType.Update,false)]
        public int Product_Update(Product item)
        {
            //if you wish to return the number of rows affected
            //   your rdt should be an int; otherwise use a void

            //action is done as a transaction
            using (var context = new NorthwindContext())
            {
                //stage the ENTIRE record for saving
                //all fields are altered
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;

                //capture the number of rows affected for the update
                //   commit and return
                return context.SaveChanges();
            }
        }
        //Delete Method for an ObjectDataSource 
        //The delete method will want an entity level parameter
        //The delete method will NOW be overloaded
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int ProductDelete(Product Item)
        {
            //this delete method calls the delete method
            //  which actually does the code passing in 
            //  the required parameter(s) for that alternate
            //  delete method
            return Product_Delete(Item.ProductID);//instance
        }
        //Delete Method
        public int Product_Delete(int productid)//integer
        {
            //if you wish to return the number of rows affected
            //   your rdt should be an int; otherwise use a void

            using (var context = new NorthwindContext())
            {
                //Physical removal of a record
                //locate the reocrd to be removed from the database
                //var existing = context.Products.Find(productid);

                //stage the removal of the record
                //context.Products.Remove(existing);

                //commit of the staged action
                //capture the number of rows affected for the update
                //   commit and return
                //return context.SaveChanges();

                //         OR

                //Logical deletion of a record
                //this usually involves some field on the record
                //     which acts as a flag.
                //for Product, there is a field called Discontinued
                //to indicate that the records is logically deleted
                //    this flag is set to a particular value: true
                //if a logical delete is necessary for your system
                //    you do an Update of the field for the record
                var existing = context.Products.Find(productid);
                existing.Discontinued = true;
                context.Entry(existing).State = System.Data.Entity.EntityState.Modified;
                //capture the number of rows affected for the update
                //   commit and return
                return context.SaveChanges();
            }
        }
    }
}
