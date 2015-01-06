<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="IndexPage.aspx.cs" Inherits="IndexPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false"
        AllowPaging="true"
        PageSize="5"
        DataSourceID="SqlDataSource1">
        <Columns>
        <asp:TemplateField HeaderText="Ultimele stiri">
           <ItemTemplate> 
           <h1><a href=<%#"article_view.aspx?article_id=" + Eval("ArticleId")%>> <%#Eval("Title")%></a></h1>
           </ItemTemplate>
        </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SqlServices %>"
        SelectCommand="SELECT Articles.ArticleId, Articles.Title FROM Articles WHERE Articles.PendingStatus=0 ORDER BY Articles.PublishDate DESC">
    </asp:SqlDataSource>
</asp:Content>