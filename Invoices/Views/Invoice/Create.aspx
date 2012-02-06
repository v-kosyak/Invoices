<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Invoices.Models.Invoice>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Invoice
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        Select a customer and add product(s) to the invoice.
    </p>
    <fieldset>
        <legend>Details</legend>
    <% using (Html.BeginForm("Create", "Invoice", FormMethod.Post)) {%>
        <%:Html.ValidationSummary(true) %>
        <div class="editor-label">
            <%:Html.LabelFor(model => model.CustomerId)%>
        </div>
        <div class="editor-field">
            <%:Html.DropDownListFor(model => model.CustomerId, ViewData["Customers"] as IEnumerable<SelectListItem>, new { onchange = "this.form.submit();" })%>
            <%:Html.ValidationMessageFor(model => model.CustomerId) %>
        </div>
            <div class="editor-label">
            <%:Html.LabelFor(model => model.CustomerAddress) %>
    </div>
    <div class="editor-field">
            <%:Html.TextAreaFor(model => model.CustomerAddress, new { ReadOnly = "True", Width = "200px"}) %>
    </div>      

    <% } %>

    <% using (Html.BeginForm("Submit", "Invoice", FormMethod.Post)) {%>
        <%:Html.ValidationSummary(true) %>         
        <%:Html.HiddenFor(model => model.CustomerId) %>
        <% Html.RenderPartial("_InvoiceLinesTable",Model.Lines); %>    
        <%:Html.ValidationMessageFor(model => model.Lines) %>
    <p>
        <%: Html.ActionLink("Add product", "AddProduct", "InvoiceLine") %>
    </p>
    <p>
        <input type="submit" value="Submit" />
    </p>    
    
    <% } %>
    </fieldset>
    <div>
        <%: Html.ActionLink("Reset", "Index") %>
    </div>
</asp:Content>
