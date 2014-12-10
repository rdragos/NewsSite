<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
    DataKeyNames="ApplicationId,LoweredUserName" DataSourceID="SqlDataSource1">
  <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <h1><%# Eval ("UserId") %></h1>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                        CommandName="Edit" Text="Edit"></asp:LinkButton>
                </ItemTemplate>
                <EditItemTemplate>
                    <h1><%# Eval ("UserId") %></h1>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                        CommandName="Update" Text="LOL"></asp:LinkButton>
                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                        CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <p><%# Eval ("UserName") %></p>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind ("UserName") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            
        </Columns>
    </asp:GridView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
    SelectCommand="SELECT * FROM [vw_aspnet_Users]"
    UpdateCommand="UPDATE [vw_aspnet_Users] SET UserName = @UserName WHERE ApplicationId = @ApplicationId AND LoweredUserName = @LoweredUserName" ></asp:SqlDataSource>
</asp:Content>

