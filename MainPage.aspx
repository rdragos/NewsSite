<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MainPage.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="CategoryName"
        DataSourceID="SqlDataSource1">
        <Columns>
        <asp:TemplateField HeaderText="Categorii">
           <ItemTemplate> 
           <h1><a href=<%#"news_categories.aspx?category=" + Eval("CategoryName").ToString().Replace(" ", "%20")%>> <%#Eval("CategoryName")%></a></h1>
           </ItemTemplate>
        </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SqlServices %>"
        SelectCommand="SELECT DISTINCT CategoryName FROM Categories;">
    </asp:SqlDataSource>
    <asp:Button ID="Button1" runat="server" Text="Generate SQL" OnClick="GenerateSQL"/>
</asp:Content>