<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="news_categories.aspx.cs" Inherits="news_categories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" AllowSorting="true">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>                    
                    <asp:Label ID="Label1" runat="server" ForeColor="Red">
                    <h1> <a href='article_view.aspx?article_id=<%# Eval("ArticleId") %>'><%# Eval("Title") %></a></h1>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>

