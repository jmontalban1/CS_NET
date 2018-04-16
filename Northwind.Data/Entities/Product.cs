using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
#endregion

namespace Northwind.Data.Entities
{
    //the entity needs to know which table in the data
    //  it is describing
    //We do this with Component Data Annotation
    [Table("Products")]
    public class Product
    {
        // create a list of auto implemented properties
        // each properties describes a single field in the table

        //By default EntityFramework expects primary keys to end
        //    with the letters ID or Id
        //IF your pkey property has these letters the [Key]
        //    annotation is optional
        //Your primary is by default treated as an IDENTITY(n,m)
        //    field on your SQL table
        //Other forms of this annotation can cover different
        //    types of primary keys
        
        //User enterred primary key
        // [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]

        //Compound primary keys (physical order is IMPORTANT)
        //[Key, Column(Order=1)]
        // public int ....
        //[Key, Column(Order=2)]
        // public int ....
        //and so on
        [Key]
        public int ProductID { get; set; }
        [Required(ErrorMessage ="Product Name is required")]
        [StringLength(40,ErrorMessage ="Product Name is limited to 40 characters")]
        public string ProductName { get; set; }
        public int? SupplierID { get; set; }
        public int? CategoryID { get; set; }
        [StringLength(20, ErrorMessage = "Quantity Per Unit is limited to 20 characters")]
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public Int16? UnitsInStock { get; set; }
        public Int16? UnitsOnOrder { get; set; }
        public Int16? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        //create a read only property
        //this property IS NOT a physical field on the table
        [NotMapped]
        public string ProductIdentifier
        {
            get
            {
                return ProductName + " (" + ProductID + ")";
            }
        }

    }
}
