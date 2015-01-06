<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="article_view.aspx.cs" Inherits="article_view" %>
<%@ Import Namespace="System.Globalization" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="Styles/comments.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="container">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" ShowHeader="false" GridLines="None">
        <Columns>
        <asp:TemplateField>
            <ItemTemplate>
            <div class="span6 pull-right" style="text-align:right;">
                <p>Category: <%# Eval("CategoryName") %></p>
                <p>Created on: <%# Eval("PublishDate") %></p>
            </div>
            <div class="row clearfix">
            <div class="col-md-12 column">
			<img class="img-thumbnail" alt="140x140" src="<%#Eval("Link") %>"/>
                <div class="page-header">
                <h1>
                    <%# Eval("Title")%>
                </h1>
                </div>
            <p>
                <%# Eval("Content") %>
            </p>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
        </asp:GridView>
        <%---
        Adding the comment box
        ---%>
        <div class="detailBox">
            <div class="titleBox">
            <label>Comment Box</label>
            <button type="button" class="close" aria-hidden="true">&times;</button>
            </div>
        <div class="commentBox">
            <p class="taskDescription">Much comments. Very wow.</p>
        </div>
        <div class="actionBox">
        <ul class="commentList">
        <%--- Render comments

        ---%>
        <asp:GridView ID="CommentGrid" runat="server" AutoGenerateColumns="false" GridLines="None">
        <Columns>
        <asp:TemplateField>
            <ItemTemplate>
            <li>
                <h4 class="media-heading text-uppercase reviews">
                    <%#Eval("UserName") %>
                </h4>
                <div class="commentText">
                    <p class=""><%#Eval("Content") %></p>
                    <span class="date sub-text"><%#Eval("PostDate") %></span>
                </div>
            </li>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
        </asp:GridView>

        </ul>
        <asp:LoginView ID="lvPostComment" runat="server">
            <LoggedInTemplate>
            <div class="form-group">
                <asp:TextBox ID="tbCommentText" runat="server" placeholder="Your comments" CssClass="form-control" >
                </asp:TextBox>
            </div>    
            <div class="form-group">
                <asp:Button ID="btnPostComment" runat="server" CssClass="btn btn-default" Text="Add" OnClick="postComment"></asp:Button>
            </div>
            </LoggedInTemplate>
        </asp:LoginView>
        </div>

        </div>
        <div id="EditorButtons" CssStyle="vertical-align:middle;" runat="server">
        </div>
    </div>
</asp:Content>

