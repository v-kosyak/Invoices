<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Invoices.Models.InvoiceLine>" %>
<%@ Import Namespace="Invoices.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Add a product
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Add a product</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

        <fieldset>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.ProductId) %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownListFor(model => model.ProductId, ViewData["Products"] as IEnumerable<SelectListItem> ) %>
                <%: Html.ValidationMessageFor(model => model.ProductId) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Quantity) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Quantity) %>
                <%: Html.ValidationMessageFor(model => model.Quantity) %>
            </div> 
            
            <p>
                <input type="submit" value="Add" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%: Html.ActionLink("Back to Invoice", "Create", new {CustomerId = (ViewContext.Controller as InvoicesBaseController).Invoice.CustomerId, Controller = "Invoice"}) %>
    </div>

</asp:Content>

