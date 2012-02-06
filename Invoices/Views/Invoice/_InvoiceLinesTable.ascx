<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Invoices.Models.InvoiceLine>>" %>

<table>
        <tr>            
            <th>
                Product
            </th>
            <th>
                Quantity
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: item.Product.Name %>
            </td>
            <td>
                <%: item.Quantity %>
            </td>
            <td>
                <%: Html.ActionLink("Remove", "RemoveProduct", new { ProductId=item.Product.Number })%>
            </td>
        </tr>
    
    <% } %>      


    </table>