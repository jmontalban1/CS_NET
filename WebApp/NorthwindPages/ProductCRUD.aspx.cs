
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using NorthwindSystem.BLL;
using Northwind.Data.Entities;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;
#endregion

namespace WebApp.NorthwindPages
{
    public partial class ProductCRUD : System.Web.UI.Page
    {
        //a data collection to hold multipe strings
        List<string> errormsgs = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageList.DataSource = null;
            MessageList.DataBind();

            if(!Page.IsPostBack)
            {
                ProductsDataBind();
                BindCategoryList();
                BindSupplierList();
            }
        }
        //use this method to discover the inner most error message.
        //this rotuing has been created by the user
        protected Exception GetInnerException(Exception ex)
        {
            //drill down to the inner most exception
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }
        //use this method to load a DataList with a variable
        //number of message lines.
        //each line is a string
        //the strings (lines) are passed to this routine in
        //   a List<string>
        //second parameter is the bootstrap cssclass
        protected void LoadMessageDisplay(List<string> errormsglist, string cssclass)
        {
            MessageList.CssClass = cssclass;
            MessageList.DataSource = errormsglist;
            MessageList.DataBind();
        }

        protected void ProductsDataBind()
        {
            //setup user friendly error handling
            try
            {
                //the web page needs to access the BLL class method
                //   to obtain its data
                ProductController sysmgr = new ProductController();
                //get the actual data
                List<Product> info = sysmgr.Products_List();

                //sort a data collection, you decide which field to
                //     sort; normally this is the DataTextField property
                // x and y represent any two instances in your collection
                // => (lamda sign) basically says "do the following
                // following the lamda sign, indicate what is to be done
                // x.property.CompareTo(y.property) is an ascending sort
                // y.property.CompareTo(x.property) is an descending sort
                info.Sort((x, y) => x.ProductName.CompareTo(y.ProductName));

                //inform control of the data source
                ProductList.DataSource = info;
                //set the DisplayText and ValueText fields to the
                //    appropriate Property names in the Entity
                ProductList.DataTextField = "ProductName";
                ProductList.DataValueField = "ProductID";
                //physically attach data to control
                ProductList.DataBind();

                //add a prompt line to the start of the ddl control
                ProductList.Items.Insert(0, "select ...");
            }
            catch (DbUpdateException ex)
            {
                UpdateException updateException = (UpdateException)ex.InnerException;
                if (updateException.InnerException != null)
                {
                    errormsgs.Add(updateException.InnerException.Message.ToString());
                }
                else
                {
                    errormsgs.Add(updateException.Message);
                }
                LoadMessageDisplay(errormsgs, "alert alert-danger");
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        errormsgs.Add(validationError.ErrorMessage);
                    }
                }
                LoadMessageDisplay(errormsgs, "alert alert-danger");
            }
            catch (Exception ex)
            {
                errormsgs.Add(GetInnerException(ex).ToString());
                LoadMessageDisplay(errormsgs, "alert alert-danger");
            }


        }

        protected void BindCategoryList()
        {
            
            try
            {
                CategoryController sysmgr = new CategoryController();
                List<Category> info = sysmgr.Category_List();
                CategoryList.DataSource = info;
                CategoryList.DataTextField = "CategoryName";
                CategoryList.DataValueField = "CategoryID";
                CategoryList.DataBind();
                CategoryList.Items.Insert(0, "select ...");
            }
            catch (DbUpdateException ex)
            {
                UpdateException updateException = (UpdateException)ex.InnerException;
                if (updateException.InnerException != null)
                {
                    errormsgs.Add(updateException.InnerException.Message.ToString());
                }
                else
                {
                    errormsgs.Add(updateException.Message);
                }
                LoadMessageDisplay(errormsgs, "alert alert-danger");
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        errormsgs.Add(validationError.ErrorMessage);
                    }
                }
                LoadMessageDisplay(errormsgs, "alert alert-danger");
            }
            catch (Exception ex)
            {
                errormsgs.Add(GetInnerException(ex).ToString());
                LoadMessageDisplay(errormsgs, "alert alert-danger");
            }


        }

        protected void BindSupplierList()
        {

            try
            {
                SupplierController sysmgr = new SupplierController();
                List<Supplier> info = sysmgr.Supplier_List();
                SupplierList.DataSource = info;
                SupplierList.DataTextField = "CompanyName";
                SupplierList.DataValueField = "SupplierID";
                SupplierList.DataBind();
                SupplierList.Items.Insert(0, "select ...");
            }
            catch (DbUpdateException ex)
            {
                UpdateException updateException = (UpdateException)ex.InnerException;
                if (updateException.InnerException != null)
                {
                    errormsgs.Add(updateException.InnerException.Message.ToString());
                }
                else
                {
                    errormsgs.Add(updateException.Message);
                }
                LoadMessageDisplay(errormsgs, "alert alert-danger");
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        errormsgs.Add(validationError.ErrorMessage);
                    }
                }
                LoadMessageDisplay(errormsgs, "alert alert-danger");
            }
            catch (Exception ex)
            {
                errormsgs.Add(GetInnerException(ex).ToString());
                LoadMessageDisplay(errormsgs, "alert alert-danger");
            }


        }

        protected void Search_Click(object sender, EventArgs e)
        {
            //does my search value exist
            //search values may come from textboxes, dropdownlists,
            //    radiobuttonlists, etc.
            //check to see if you have a search value
            if (ProductList.SelectedIndex == 0)
            {
                //ProductList has a prompt line at index 0
                errormsgs.Add("Select a product to search.");
                LoadMessageDisplay(errormsgs, "alert alert-warning");
            }
            else
            {
                //product was selected
                
                //check any other requirements that may be part
                //   of your search criteria

                //if all is good, do a standard pattern lookup
                try
                {
                    ProductController sysmgr = new ProductController();
                    //call the desired method
                    Product info = sysmgr.Products_Get(int.Parse(ProductList.SelectedValue));
                    //check the result of the method execution
                    //if the record was not found, the Product instance
                    //     will be null
                    if (info == null)
                    {
                       
                        errormsgs.Add("Product not found. Try again");
                        LoadMessageDisplay(errormsgs, "alert alert-warning");
                        //this should not happen
                        //the value was taken from a list generated by
                        //    the system from the database
                        //HOWEVER: since we are in a multiple user
                        //    environment, between loading the ddl and
                        //    actually doing a search,  another user
                        //    could have deleted the desired product record
                        //THEREFORE: the ddl should be refreshed so that
                        //    it only shows the current product list
                        ProductsDataBind();
                    }
                    else
                    {
                        //record was found
                        //load the web form controls with the data
                        //  that was returned
                        //controls are loaded with datatype string
                        ProductID.Text = info.ProductID.ToString();
                        ProductName.Text = info.ProductName;
                        //ddl are positioned using SelectedValue
                        SupplierList.SelectedValue = info.SupplierID.ToString();
                        CategoryList.SelectedValue = info.CategoryID.ToString();
                        QuantityPerUnit.Text = info.QuantityPerUnit;
                        //UnitPrice, UnitsInStock, UnitsOnOrder and 
                        //    ReorderLevel are nullable numerics
                        UnitPrice.Text = info.UnitPrice == null ? "" : string.Format("{0:0.00}", info.UnitPrice);
                       
                        UnitsInStock.Text = info.UnitsInStock == null ? "" : info.UnitsInStock.ToString();
                        UnitsOnOrder.Text = info.UnitsOnOrder == null ? "" : info.UnitsOnOrder.ToString();
                        ReorderLevel.Text = info.ReorderLevel == null ? "" : info.ReorderLevel.ToString();
                        //Discontinued is a checkbox which is a bool set
                        Discontinued.Checked = info.Discontinued;
                    }
                }
                catch (DbUpdateException ex)
                {
                    UpdateException updateException = (UpdateException)ex.InnerException;
                    if (updateException.InnerException != null)
                    {
                        errormsgs.Add(updateException.InnerException.Message.ToString());
                    }
                    else
                    {
                        errormsgs.Add(updateException.Message);
                    }
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            errormsgs.Add(validationError.ErrorMessage);
                        }
                    }
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }
                catch (Exception ex)
                {
                    errormsgs.Add(GetInnerException(ex).ToString());
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }

            }
        }

        protected void Clear_Click(object sender, EventArgs e)
        {
            ProductID.Text = "";
            ProductName.Text = "";
            SupplierList.ClearSelection(); //SupplierList.SelectedIndex = -1;
            CategoryList.ClearSelection(); //CategoryList.SelectedIndex = -1;
            QuantityPerUnit.Text = "";
            UnitPrice.Text = "";
            UnitsInStock.Text = "";
            UnitsOnOrder.Text = "";
            ReorderLevel.Text = "";
            Discontinued.Checked = false;
        }

        protected void AddProduct_Click(object sender, EventArgs e)
        {
            //we need valid data
            //if you have validation controls, re-execute them
            if (Page.IsValid)
            {
                //you may have additional testing to do before
                //    all data is deemed valid
                //in this example, we will assume that the
                //    SupplierID and CategoryID is required.
                if (SupplierList.SelectedIndex == 0)
                {
                    errormsgs.Add("Please select a supplier");
                }
                if (CategoryList.SelectedIndex == 0)
                {
                    errormsgs.Add("Please select a category");
                }
                if (errormsgs.Count>0)
                {
                    LoadMessageDisplay(errormsgs, "alert alert-info");
                }
                else
                {
                    //at this point, according to the program logic
                    //    your data is valid
                    //pass the data on for processing
                    //    a) collection of data from the form controls
                    //    b) load of an entity instance with the data
                    //    c) connect to the necessary controller 
                    //    d) call the necessary controller method
                    //    e) report results
                    try
                    {
                        Product item = new Product();
                        item.ProductName = ProductName.Text;
                        item.SupplierID = int.Parse(SupplierList.SelectedValue);
                        item.CategoryID = int.Parse(CategoryList.SelectedValue);
                        //nullable string
                        item.QuantityPerUnit = string.IsNullOrEmpty(QuantityPerUnit.Text) ?
                                                    null : QuantityPerUnit.Text;
                        //nullable numerics
                        if (string.IsNullOrEmpty(UnitPrice.Text))
                        {
                            item.UnitPrice = null;
                        }
                        else
                        {
                            item.UnitPrice = decimal.Parse(UnitPrice.Text);
                        }
                        if (string.IsNullOrEmpty(UnitsInStock.Text))
                        {
                            item.UnitsInStock = null;
                        }
                        else
                        {
                            item.UnitsInStock = Int16.Parse(UnitsInStock.Text);
                        }
                        if (string.IsNullOrEmpty(UnitsOnOrder.Text))
                        {
                            item.UnitsOnOrder = null;
                        }
                        else
                        {
                            item.UnitsOnOrder = Int16.Parse(UnitsOnOrder.Text);
                        }
                        if (string.IsNullOrEmpty(ReorderLevel.Text))
                        {
                            item.ReorderLevel = null;
                        }
                        else
                        {
                            item.ReorderLevel = Int16.Parse(ReorderLevel.Text);
                        }
                        item.Discontinued = Discontinued.Checked;

                        //connect
                        ProductController sysmgr = new ProductController();

                        //method call
                        int pkey = sysmgr.Product_Add(item);

                        //if the call was successful the following actions
                        //    can be done
                        ProductID.Text = pkey.ToString();  ///optional
                        errormsgs.Add("Product was added");
                        LoadMessageDisplay(errormsgs, "alert alert-success");

                        //remember to refresh any other necessary associated controls
                        ProductsDataBind();
                        ProductList.SelectedValue = ProductID.Text;
                    }
                    catch (DbUpdateException ex)
                    {
                        UpdateException updateException = (UpdateException)ex.InnerException;
                        if (updateException.InnerException != null)
                        {
                            errormsgs.Add(updateException.InnerException.Message.ToString());
                        }
                        else
                        {
                            errormsgs.Add(updateException.Message);
                        }
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                errormsgs.Add(validationError.ErrorMessage);
                            }
                        }
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }
                    catch (Exception ex)
                    {
                        errormsgs.Add(GetInnerException(ex).ToString());
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }
                }
            }

        }

        protected void UpdateProduct_Click(object sender, EventArgs e)
        {
            //we need valid data
            //if you have validation controls, re-execute them
            if (Page.IsValid)
            {
                //this test is to ensure that a ProductID exists
                // before attempting to do an update
                if (string.IsNullOrEmpty(ProductID.Text))
                {
                    errormsgs.Add("Update requires you to search for the product first.");
                }
                else
                {
                    int temp = 0;
                    if (!int.TryParse(ProductID.Text,out temp))
                    {
                        errormsgs.Add("Product ID is invalid");
                    }
                }


                //you may have additional testing to do before
                //    all data is deemed valid
                //in this example, we will assume that the
                //    SupplierID and CategoryID is required.
                if (SupplierList.SelectedIndex == 0)
                {
                    errormsgs.Add("Please select a supplier");
                }
                if (CategoryList.SelectedIndex == 0)
                {
                    errormsgs.Add("Please select a category");
                }
                if (errormsgs.Count > 0)
                {
                    LoadMessageDisplay(errormsgs, "alert alert-info");
                }
                else
                {
                    //at this point, according to the program logic
                    //    your data is valid
                    //pass the data on for processing
                    //    a) collection of data from the form controls
                    //    b) load of an entity instance with the data
                    //    c) connect to the necessary controller 
                    //    d) call the necessary controller method
                    //    e) report results
                    try
                    {
                        Product item = new Product();
                        //on the update, you need to include the prmary key
                        //    in the instance
                        item.ProductID = int.Parse(ProductID.Text);
                        item.ProductName = ProductName.Text;
                        item.SupplierID = int.Parse(SupplierList.SelectedValue);
                        item.CategoryID = int.Parse(CategoryList.SelectedValue);
                        //nullable string
                        item.QuantityPerUnit = string.IsNullOrEmpty(QuantityPerUnit.Text) ?
                                                    null : QuantityPerUnit.Text;
                        //nullable numerics
                        if (string.IsNullOrEmpty(UnitPrice.Text))
                        {
                            item.UnitPrice = null;
                        }
                        else
                        {
                            item.UnitPrice = decimal.Parse(UnitPrice.Text);
                        }
                        if (string.IsNullOrEmpty(UnitsInStock.Text))
                        {
                            item.UnitsInStock = null;
                        }
                        else
                        {
                            item.UnitsInStock = Int16.Parse(UnitsInStock.Text);
                        }
                        if (string.IsNullOrEmpty(UnitsOnOrder.Text))
                        {
                            item.UnitsOnOrder = null;
                        }
                        else
                        {
                            item.UnitsOnOrder = Int16.Parse(UnitsOnOrder.Text);
                        }
                        if (string.IsNullOrEmpty(ReorderLevel.Text))
                        {
                            item.ReorderLevel = null;
                        }
                        else
                        {
                            item.ReorderLevel = Int16.Parse(ReorderLevel.Text);
                        }
                        item.Discontinued = Discontinued.Checked;

                        //connect
                        ProductController sysmgr = new ProductController();

                        //method call returns the number of rows affected
                        int rowsaffect = sysmgr.Product_Update(item);

                        //if the call was successful and changed rows
                        //    or if no rows were changed
                        if (rowsaffect > 0)
                        {
                             errormsgs.Add("Product was updated");
                             LoadMessageDisplay(errormsgs, "alert alert-success");

                            //remember to refresh any other necessary associated controls
                             ProductsDataBind();
                            ProductList.SelectedValue = ProductID.Text;
                        }
                        else
                        {
                            errormsgs.Add("Product does not appear to be on file. Look up the product again.");
                            LoadMessageDisplay(errormsgs, "alert alert-warning");
                            ProductsDataBind();
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        UpdateException updateException = (UpdateException)ex.InnerException;
                        if (updateException.InnerException != null)
                        {
                            errormsgs.Add(updateException.InnerException.Message.ToString());
                        }
                        else
                        {
                            errormsgs.Add(updateException.Message);
                        }
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                errormsgs.Add(validationError.ErrorMessage);
                            }
                        }
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }
                    catch (Exception ex)
                    {
                        errormsgs.Add(GetInnerException(ex).ToString());
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }
                }
            }
        }

        protected void RemoveProduct_Click(object sender, EventArgs e)
        {
            //this test is to ensure that a ProductID exists
            // before attempting to do an update
            if (string.IsNullOrEmpty(ProductID.Text))
            {
                errormsgs.Add("Update requires you to search for the product first.");
            }
            else
            {
                int temp = 0;
                if (!int.TryParse(ProductID.Text, out temp))
                {
                    errormsgs.Add("Product ID is invalid");
                }
            }
            if (errormsgs.Count() > 0)
            {
                LoadMessageDisplay(errormsgs, "alert alert-info");
            }
            else
            {
                //at this point, according to the program logic
                //    your data is valid
                //pass the data on for processing

                //    c) connect to the necessary controller 
                //    d) call the necessary controller method
                //    e) report results
                try
                {
                    //connect
                    ProductController sysmgr = new ProductController();

                    //method call returns the number of rows affected
                    int rowsaffect = sysmgr.Product_Delete(int.Parse(ProductID.Text));

                    //if the call was successful and changed rows
                    //    or if no rows were changed
                    if (rowsaffect > 0)
                    {
                        errormsgs.Add("Product is discontinued");
                        LoadMessageDisplay(errormsgs, "alert alert-success");

                        //remember to refresh any other necessary associated controls
                        ProductsDataBind();
                        //reposition
                        ProductList.SelectedValue = ProductID.Text;
                        Discontinued.Checked = true;
                    }
                    else
                    {
                        errormsgs.Add("Product does not appear to be on file. Look up the product again.");
                        LoadMessageDisplay(errormsgs, "alert alert-warning");
                        ProductsDataBind();
                    }
                }
                catch (DbUpdateException ex)
                {
                    UpdateException updateException = (UpdateException)ex.InnerException;
                    if (updateException.InnerException != null)
                    {
                        errormsgs.Add(updateException.InnerException.Message.ToString());
                    }
                    else
                    {
                        errormsgs.Add(updateException.Message);
                    }
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            errormsgs.Add(validationError.ErrorMessage);
                        }
                    }
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }
                catch (Exception ex)
                {
                    errormsgs.Add(GetInnerException(ex).ToString());
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }
            }
        }
    }
}