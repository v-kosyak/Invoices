﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Invoice Submitted
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Invoice Successfully Submitted</h2>

    <p>
    <%: Html.ActionLink("Show PDF of the booked invoice", "GetPdf", new {InvoiceId = ViewData["InvoiceId"]}) %>
    </p>
    <p>
    <%: Html.ActionLink("New invoice", "Create") %>
    </p>
</asp:Content>
