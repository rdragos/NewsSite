<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="propose_news.aspx.cs" Inherits="propose_news" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
    <div class="row clearfix" style="text-align:right">
    <asp:TextBox runat="server" id="CategoryTag" placeholder="Here goes Category"></asp:TextBox>
    </div>
    <div class="row clearfix">
        <div class="col-md-12 column">
        <img class="img-thumbnail" alt="140x140" src="http://lorempixel.com/140/140/"/>
            <div class="page-header">
            <asp:TextBox runat="server" ID="TitleTag" placeholder="Here goes Title"></asp:TextBox>
            </div>
            <asp:TextBox runat="server" ID="ContentTag" placeholder="Here goes Content"></asp:TextBox>
        </div>
    </div>
        <asp:Button ID="Button1" runat="server"
            CssClass="btn btn-danger pull-right"
            Text="Submit to proposals!"
            Onclick="final_proposal" />
    </div>
</asp:Content>

 