<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="article_view.aspx.cs" Inherits="article_view" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <h1>LOL</h1>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
    <Columns>
    <asp:TemplateField>
        <ItemTemplate>
            <h1><%# Eval("Content") %></h1>
        </ItemTemplate>
    </asp:TemplateField>
    </Columns>
    </asp:GridView>
</asp:Content>

