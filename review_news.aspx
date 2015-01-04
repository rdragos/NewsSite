<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="review_news.aspx.cs" Inherits="review_news" %>
<%@ Import Namespace="System.Globalization" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false"> 
        <Columns>
        <asp:TemplateField>
        <ItemTemplate>
        <div class="container">
            <p> <a href='article_view.aspx?article_id=<%# Eval("ArticleId") %>'><%# Eval("Title") %></a></p>
            <p><%# Eval("PublishDate") %></p>
        </div>
        </ItemTemplate>
        </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>