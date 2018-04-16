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
    public partial class MultiRecordQuery : System.Web.UI.Page
    {
        //a data collection to hold multipe strings
        List<string> errormsgs = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageList.DataSource = null;
            MessageList.DataBind();

            if (!Page.IsPostBack)
            {
                BindCategoryList();
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

        protected void DbUpdateException_Catch(DbUpdateException ex)
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
        protected void DbEntityValidationException_Catch(DbEntityValidationException ex)
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
        protected void  General_Catch (Exception ex)
        {
            errormsgs.Add(GetInnerException(ex).ToString());
            LoadMessageDisplay(errormsgs, "alert alert-danger");
        }
        
        protected void BindCategoryList()
        {
            try
            {
                CategoryController sysmgr = new CategoryController();
                List<Category> info = sysmgr.Category_List();
                //for sorting
                info.Sort((x, y) => x.CategoryName.CompareTo(y.CategoryName)); //ascending
                CategoryList.DataSource = info;
                CategoryList.DataTextField = "CategoryName";
                CategoryList.DataValueField = "CategoryID";
                CategoryList.DataBind();
                CategoryList.Items.Insert(0, "select...");
            }
            catch (DbUpdateException ex)
            {
                DbUpdateException_Catch(ex);
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationException_Catch(ex);
            }
            catch (Exception ex)
            {
                General_Catch(ex);
            }
        }
        protected void SearchCatgeory_Click(object sender, EventArgs e)
        {
            if (CategoryList.SelectedIndex == 0)
            {
                errormsgs.Add("Select a category to look up.");
                LoadMessageDisplay(errormsgs, "alert alert-info");
            }
            else
            {
                try
                {
                    //standard lookup
                    CategoryController sysmgr = new CategoryController();
                    //Primary Key lookup
                    //0 or 1
                    Category info = sysmgr.Category_Get(int.Parse(CategoryList.SelectedValue));
                    //check results
                    if (info == null)
                    {
                        errormsgs.Add("Category no longer on file. Select a different category");
                        LoadMessageDisplay(errormsgs, "alert alert-info");
                        //refresh the ddl
                        BindCategoryList();
                    }
                    else
                    {
                        CategoryID.Text = info.CategoryID.ToString();
                        CategoryName.Text = info.CategoryName;
                        Description.Text = info.Description == null ?
                            "no description available" : info.Description;
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

        protected void SearchProduct_Click(object sender, EventArgs e)
        {
            //take the text string 
            //pass to the BLL controller
            //receive back a collection of List<T> (Product)
            //fill the GridView
            if (string.IsNullOrEmpty(SearchPartialName.Text))
            {
                errormsgs.Add("Enter a partial product name to search");
                LoadMessageDisplay(errormsgs, "alert alert-info");
            }
            else
            {
                try
                {
                    ProductController sysmgr = new ProductController();
                    List<Product> info = sysmgr.Product_GetByPartialName(SearchPartialName.Text);
                    ProductSelectionList.DataSource = info;
                    ProductSelectionList.DataBind();
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

        protected void ProductSelectionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            //extract a value (primary key) from the GridViewRow
            //pass to the BLL controller
            //receive back a record (Product)
            //fill the display Labels with data
            try
            {
                //get a pointer to the gridview row that was selected
                GridViewRow agvrow = ProductSelectionList.Rows[ProductSelectionList.SelectedIndex];
                //access a specific control on the gridview row
                //data in the control is accessed by the control access method

                int productid = int.Parse((agvrow.FindControl("ProductID") as Label).Text);
                ProductController sysmgr = new ProductController();
                Product info = sysmgr.Products_Get(productid);
                ProductID.Text = info.ProductID.ToString();
                ProductName.Text = info.ProductName;
                UnitPrice.Text = string.Format("{0:0.00}", info.UnitPrice);
                UnitsInStock.Text = info.UnitsInStock.ToString();
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

        protected void ProductSelectionList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //step 1) change the PageIndex of the GridView using the NewPageIndex under e
            ProductSelectionList.PageIndex = e.NewPageIndex;

            //step 2) refresh your GridView by re-executing the call to the BLL.
            SearchProduct_Click(sender, new EventArgs());
         
        }
    }
}