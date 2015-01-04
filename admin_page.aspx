<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="admin_page.aspx.cs" Inherits="admin_page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <h1><%#Eval("UserName") %></h1>
                <p><%#Eval("RoleName") %></p>
                <a class="btn btn-default"
                href="change_user_status.aspx?UserId=<%#Eval("UserId") %>&role=admin">Make Admin</button>
                <a class="btn btn-default"
                href="change_user_status.aspx?UserId=<%#Eval("UserId") %>&role=editor">Make Editor</button>
                <a class="btn btn-default"
                href="change_user_status.aspx?UserId=<%#Eval("UserId") %>&role=user">Make Normal</button>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
</asp:Content>