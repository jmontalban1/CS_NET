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
    public partial class ODSQuery : System.Web.UI.Page
    {
        List<string> errormsgs = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageList.DataSource = null;
            MessageList.DataBind();
         
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
                        //if you wish to force a refresh of the ODS related 
                        //      control (ddl)
                        CategoryList.DataBind();
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
    }
}