<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="propose_news.aspx.cs" Inherits="propose_news" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
    <div class="clearfix pull-right">
        <p runat="server" id="CategoryTag" contenteditable="true">Some Random Category</p>                
    </div>
    <div class="row clearfix">
        <div class="col-md-12 column">
        <img class="img-thumbnail" alt="140x140" src="http://lorempixel.com/140/140/"/>
            <div class="page-header">
            <h1 runat="server" id="TitleTag" contenteditable="true">LALALAL</h1>
            </div>
            <p runat="server" id="ContentTag" contenteditable="true">Text in paragraph</p>
        </div>
    </div>
        <asp:Button ID="Button1" runat="server"
            CssClass="btn btn-danger pull-right"
            Text="Submit to proposals!"
            Onclick="final_proposal" />
    </div>
</asp:Content>

 