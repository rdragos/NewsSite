<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="search_content.aspx.cs" Inherits="search_content" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:TextBox ID="tb_search" runat="server" CssClass="form-control"></asp:TextBox>
    <center style="margin-top:10px;">
    <asp:Button ID="btn_search"  CssClass="btn btn-default" runat="server" Text="Magic search" OnClick="searchAction"/>
    </center>
    <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="false"
    GridLines="Horizontal"
    Border="0" Width="100%"
    >
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