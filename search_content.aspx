<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="search_content.aspx.cs" Inherits="search_content" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:TextBox ID="tb_search" runat="server"></asp:TextBox>
    <asp:Button ID="btn_search" runat="server" Text="Magic search" OnClick="searchAction"/>
    <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <h1> <a href='article_view.aspx?article_id=<%# Eval("ArticleId") %>'><%# Eval("Title") %></a></h1>
                <p>Score: <%#Eval("Score") %></p>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    </asp:GridView>
</asp:Content>