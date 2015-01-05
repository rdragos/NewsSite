<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="news_categories.aspx.cs" Inherits="news_categories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" OnSorting="gvName_Sorting" AllowSorting="true">
        <Columns>
        <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title"/>
        <asp:BoundField DataField="PublishDate" HeaderText="PublishDate" SortExpression="PublishDate"/>
        <asp:BoundField DataField="PublisherName" HeaderText="Author" SortExpression="PublisherName"/>
            <asp:TemplateField>
                <ItemTemplate>                    
                    <h1> <a href='article_view.aspx?article_id=<%# Eval("ArticleId") %>'><%# Eval("Title") %></a></h1>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>

