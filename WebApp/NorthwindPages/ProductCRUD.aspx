<%@ Page Title="CRUD" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductCRUD.aspx.cs" Inherits="WebApp.NorthwindPages.ProductCRUD" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <div class="page-header">
        <h1>Product CRUD Maintenance</h1>
    </div>

    <div class="row col-md-12">
        <div class="alert alert-warning">
            <blockquote style="font-style: italic">
                This illustrates a single web form page CRUD maintenance. This form will use basic bootstrap formatting
            </blockquote>
        </div>
    </div>

    <div class="row">
      <%--  this will be the lookup control area--%>
         <div class="col-md-12"> 
             <asp:Label ID="Label5" runat="server" Text="Select a Product"></asp:Label>&nbsp;&nbsp;
             <asp:DropDownList ID="ProductList" runat="server"></asp:DropDownList>&nbsp;&nbsp;
             <asp:Button ID="Search" runat="server" Text="Search" OnClick="Search_Click" CausesValidation="false"    />&nbsp;&nbsp;
             <asp:Button ID="Clear" runat="server" Text="Clear" height="26px" width="63px" OnClick="Clear_Click"  />&nbsp;&nbsp;
             <asp:LinkButton ID="AddProduct" runat="server" Font-Size="X-Large" OnClick="AddProduct_Click" >Add</asp:LinkButton>&nbsp;&nbsp;
             <asp:LinkButton ID="UpdateProduct" runat="server" Font-Size="X-Large" OnClick="UpdateProduct_Click" >Update</asp:LinkButton>&nbsp;&nbsp;
             <asp:LinkButton ID="RemoveProduct" runat="server" Font-Size="X-Large" OnClick="RemoveProduct_Click" CausesValidation="false" >Remove</asp:LinkButton>
             <ajaxToolkit:ConfirmButtonExtender ID="RemoveProduct_ConfirmButtonExtender" runat="server" BehaviorID="RemoveProduct_ConfirmButtonExtender" ConfirmText="Do you wish to discontinue this product?" TargetControlID="RemoveProduct" />
             &nbsp;&nbsp;
         
            
         
             <br /><br />
             <%--Basic error message display can be as little as a Label
                 <asp:Label ID="Message" runat="server" ></asp:Label>--%>
             <%-- A more complex control that will allow for a list
                        of errors to be display is the DataList control 
                  The DataList requires at minimum one template called the
                        ItemTemplate which does display
                  The escape sequence within the template surrounds the
                        data to be displayed
                  Container refers to the data collection (list)
                  DataItem refers to a row in the data collection--%>
             <asp:DataList ID="MessageList" runat="server">
                 <ItemTemplate>
                     <%# Container.DataItem %>
                 </ItemTemplate>
             </asp:DataList>
             <br /><br />
             <asp:ValidationSummary ID="ProductValidation" runat="server"
                  HeaderText="Please correct your input to resolve the following issues." />
             <asp:RequiredFieldValidator ID="RequiredFieldProductName" runat="server" 
                 ErrorMessage="Product Name is required."
                 Display="None" ControlToValidate="ProductName" SetFocusOnError="True">
             </asp:RequiredFieldValidator>
             <asp:CompareValidator ID="CompareUnitPrice" runat="server" 
                 ErrorMessage="Unit price is a dollar amount of 0 or greater" 
                 Display="None" SetFocusOnError="True"
                 ControlToValidate="UnitPrice" Operator="GreaterThanEqual" Type="Double" ValueToCompare="0" >
             </asp:CompareValidator>
               <asp:CompareValidator ID="CompareUnitsInStock" runat="server" 
                 ErrorMessage="Units in Stock is an amount of 0 or greater" 
                 Display="None" SetFocusOnError="True"
                 ControlToValidate="UnitsInStock" Operator="GreaterThanEqual" Type="Integer" ValueToCompare="0" >
             </asp:CompareValidator>
                <asp:CompareValidator ID="CompareUnitsOnOrder" runat="server" 
                 ErrorMessage="Units on Order is an amount of 0 or greater" 
                 Display="None" SetFocusOnError="True"
                 ControlToValidate="UnitsOnOrder" Operator="GreaterThanEqual" Type="Integer" ValueToCompare="0" >
             </asp:CompareValidator>
                <asp:CompareValidator ID="CompareReorderLevel" runat="server" 
                 ErrorMessage="ROL is an amount of 0 or greater" 
                 Display="None" SetFocusOnError="True"
                 ControlToValidate="ReorderLevel" Operator="GreaterThanEqual" Type="Integer" ValueToCompare="0" >
             </asp:CompareValidator>
<%--             <asp:RangeValidator ID="RangeValidator1" runat="server"
            ErrorMessage="RangeValidator"
               Display="None"
               ControlToValidate="controlid"
                SetFocusOnError="true"
                MinimumValue="0"
                  MaximumValue="100"
                  Type="Integer"></asp:RangeValidator>
             <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="RegularExpressionValidator"
                  Display="None"
                  ControlToValidate="controlid"
                  SetFocusOnError="true"
                  ValidationExpression="whatever your expression is"></asp:RegularExpressionValidator>--%>
        </div>
      <%--  this will be the entity CRUD area--%>
        <div class ="col-md-12">
            <fieldset class="form-horizontal">
                <legend>Product Information</legend>
<%--                each control group will consist of a label and the associated control--%>
                <asp:Label ID="Label1" runat="server" Text="Product ID"
                     AssociatedControlID="ProductID"></asp:Label>
                <asp:Label ID="ProductID" runat="server" ></asp:Label> 
                  
                  <asp:Label ID="Label2" runat="server" Text="Name"
                     AssociatedControlID="ProductName"></asp:Label>
                <asp:TextBox ID="ProductName" runat="server" ></asp:TextBox> 
                  
                  <asp:Label ID="Label3" runat="server" Text="Supplier"
                     AssociatedControlID="SupplierList"></asp:Label>
                <asp:DropDownList ID="SupplierList" runat="server" Width="300px" ></asp:DropDownList> 

                    <asp:Label ID="Label6" runat="server" Text="Category"
                     AssociatedControlID="CategoryList"></asp:Label>
                <asp:DropDownList ID="CategoryList" runat="server" Width="300px" ></asp:DropDownList> 
               
                    <asp:Label ID="Label7" runat="server" Text="Quantity/Unit"
                     AssociatedControlID="QuantityPerUnit"></asp:Label>
                <asp:TextBox ID="QuantityPerUnit" runat="server" ></asp:TextBox>

             
                <asp:Label ID="Label8" runat="server" Text="Unit Price"
                     AssociatedControlID="UnitPrice"></asp:Label>
                <asp:TextBox ID="UnitPrice" runat="server" ></asp:TextBox> 

                    <asp:Label ID="Label9" runat="server" Text="In Stock"
                     AssociatedControlID="UnitsInStock"></asp:Label>
                <asp:TextBox ID="UnitsInStock" runat="server" ></asp:TextBox> 

                    <asp:Label ID="Label10" runat="server" Text="On Order"
                     AssociatedControlID="UnitsOnOrder"></asp:Label>
                <asp:TextBox ID="UnitsOnOrder" runat="server" ></asp:TextBox> 

                    <asp:Label ID="Label11" runat="server" Text="ROL"
                     AssociatedControlID="ReorderLevel"></asp:Label>
                <asp:TextBox ID="ReorderLevel" runat="server" ></asp:TextBox> 

                      <asp:Label ID="Label4" runat="server" Text="Status"
                     AssociatedControlID="Discontinued"></asp:Label>
                <asp:CheckBox ID="Discontinued" runat="server" Text="Discontinued" ></asp:CheckBox> 
            
            </fieldset>
        </div>
       
    </div>
    <script src="../Scripts/bootwrap-freecode.js"></script>
</asp:Content>
