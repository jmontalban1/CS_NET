using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using System.Data.Entity;
using Northwind.Data.Entities;
#endregion

namespace NorthwindSystem.DAL
{
    internal class NorthwindContext:DbContext
    {
        //this class needs to pass to DbContext the connection string
        //  name
        //to do this, we will create a default constructor that passes
        //  the connection string name to DbContext
        //use: :base("connectionstringname")
        public NorthwindContext():base("NWDB")
        {

        }

        //you need to create a DbContext interface property using DbSet<T>
        //<T> is the entity definition for the data set
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
    }
}
