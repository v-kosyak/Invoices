<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HandleErrorInfo>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Exception
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Error occured</h2>
    <p>
      Controller: <%=ViewData.Model.ControllerName %>
    </p>
    <p>
      Action: <%=ViewData.Model.ActionName %>
    </p>
    <p>
      Message: <%=ViewData.Model.Exception.Message %>
    </p>
    <p>
      Stack Trace: <%=ViewData.Model.Exception.StackTrace %>
    </p>

</asp:Content>
