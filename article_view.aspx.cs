using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
public partial class article_view : System.Web.UI.Page
{
    public const int APPROVED = 0;
    public const int REJECTED = 2;
    public const int IN_QUEUE_STATUS = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PendingStatus"] = 0;

        if (Request.Params["article_id"] == null)
        {
            Response.Redirect("MainPage.aspx");
            return;
        }

        Debug.WriteLine("In page load");
        try
        {
            Guid newGuid = Guid.Parse(Request.Params["article_id"]);
            WriteHtmlForArticle(newGuid);
            WriteHtmlForComments(newGuid);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Response.Redirect("google.ro");
            return;
        }
        if (Roles.IsUserInRole("editor"))
        {
            if (Session["PendingStatus"].ToString().Equals(IN_QUEUE_STATUS.ToString()))
            {
                add_editor_controls();
            }
        }
    }
    private void add_editor_controls()
    {

        Debug.WriteLine("adding more buttons to editor");
        Button bt1 = new Button();
        bt1.CssClass = "btn btn-primary";
        bt1.Text += "Approve";
        var icon = new HtmlGenericControl("i");
        icon.Attributes["class"] = "glyphicon glyphicon-ok";
        icon.Attributes["aria-hidden"] = "true";

        bt1.Controls.Add(icon);
        bt1.Click += new EventHandler(ApproveArticle);

        EditorButtons.Controls.Add(bt1);

        Button bt2 = new Button();
        bt2.CssClass = "btn btn-danger";
        bt2.Text += "Reject";

        var icon2 = new HtmlGenericControl();
        icon2.Attributes["class"] = "glyphicon glyphicon-reject";
        icon2.Attributes["aria-hidden"] = "true";
        bt2.Controls.Add(icon2);

        EditorButtons.Controls.Add(bt2);
        bt2.Click += new EventHandler(RejectArticle);
    }
    private void setPendingStatus(int pendingStatus, int fromStatus)
    {

        Debug.WriteLine("Trying to set some status!");
        var con = ConfigurationManager.ConnectionStrings["SqlServices"].ToString();
        using (SqlConnection myConnection = new SqlConnection(con))
        {
            string updateString =
                "UPDATE Articles SET PendingStatus=@fPendingStatus " +
                "WHERE (Articles.ArticleId = @fArticleId AND Articles.PendingStatus=@fFromStatus)";
            SqlCommand oCmd = new SqlCommand(updateString, myConnection);

            oCmd.Parameters.AddWithValue("@fArticleId", Session["ArticleId"].ToString());
            oCmd.Parameters.AddWithValue("@fPendingStatus", pendingStatus.ToString());
            oCmd.Parameters.AddWithValue("@fFromStatus", fromStatus.ToString());
            myConnection.Open();
            try
            {
                oCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            Debug.WriteLine("OK UPDATE");
        }
    }
    protected void ApproveArticle(object sender, EventArgs e)
    {
        Debug.WriteLine("Clicked Approved");
        setPendingStatus(APPROVED, IN_QUEUE_STATUS);
        Response.Redirect("review_news.aspx");
    }
    protected void RejectArticle(object sender, EventArgs e)
    {
        setPendingStatus(REJECTED, IN_QUEUE_STATUS);
        Response.Redirect("review_news.aspx");
    }
    protected void WriteHtmlForArticle(Guid article_id)
    {
        var con = ConfigurationManager.ConnectionStrings["SqlServices"].ToString();
        using (SqlConnection myConnection = new SqlConnection(con))
        {
            string queryString =
                "SELECT     Articles.Title, Articles.Content, Articles.ArticleId, " + 
                "Articles.PendingStatus, Articles.CategoryName, Articles.PublishDate " +
                "FROM         Articles " + 
                "WHERE     (Articles.ArticleId =@fArticleId)";
            SqlCommand oCmd = new SqlCommand(queryString, myConnection);
            oCmd.Parameters.AddWithValue("@fArticleId", article_id.ToString());
            var gridDataTable = new DataTable();
            gridDataTable.Columns.Add("Title");
            gridDataTable.Columns.Add("Content");
            gridDataTable.Columns.Add("ArticleId");
            gridDataTable.Columns.Add("CategoryName");
            gridDataTable.Columns.Add("PublishDate");
            gridDataTable.Columns.Add("Link");
            myConnection.Open();

            string queryImg = "SELECT ThumbLink FROM Thumbnails " +
                "WHERE (ArticleId=@fArticleId)";
            SqlCommand iCmd = new SqlCommand(queryImg, myConnection);
            iCmd.Parameters.AddWithValue("@fArticleId", article_id);
            string imageLink = "";
            using (SqlDataReader iReader = iCmd.ExecuteReader())
            {
                while (iReader.Read())
                {
                    imageLink = iReader["ThumbLink"].ToString();
                }
            }
            using (SqlDataReader oReader = oCmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    var newRow = gridDataTable.NewRow();                    
                    newRow["Title"] = oReader["Title"];
                    newRow["Content"] = oReader["Content"];
                    newRow["ArticleId"] = oReader["ArticleId"];
                    newRow["CategoryName"] = oReader["CategoryName"];
                    newRow["PublishDate"] = oReader["PublishDate"];
                    //newRow["Link"] = oReader["Link"];
                    Session["ArticleId"] = oReader["ArticleId"];
                    Session["PendingStatus"] = oReader["PendingStatus"];
                    newRow["Link"] = imageLink;
                    gridDataTable.Rows.Add(newRow);           
                }
                myConnection.Close();
            }

            GridView1.DataSource = gridDataTable;
        }
        GridView1.DataBind();
    }
    private void WriteHtmlForComments(Guid articleId) {

        var con = ConfigurationManager.ConnectionStrings["SqlServices"].ToString();
        using (SqlConnection myConnection = new SqlConnection(con))
        {
            string queryString =
                "SELECT     Comments.PostDate, Comments.[Content], aspnet_Users.UserName " +
                "FROM         Comments INNER JOIN " +
                "aspnet_Users ON Comments.UserId = aspnet_Users.UserId " +
                "WHERE     (Comments.ArticleId =@fArticleId) " +
                "ORDER BY Comments.PostDate";
            SqlCommand oCmd = new SqlCommand(queryString, myConnection);
            oCmd.Parameters.AddWithValue("@fArticleId", articleId.ToString());

            var gridDataTable = new DataTable();

            gridDataTable.Columns.Add("PostDate");
            gridDataTable.Columns.Add("Content");
            gridDataTable.Columns.Add("UserName");
            myConnection.Open();

            using (SqlDataReader oReader = oCmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    var newRow = gridDataTable.NewRow();
                    newRow["PostDate"] = oReader["PostDate"];
                    newRow["Content"] = oReader["Content"];
                    newRow["UserName"] = oReader["UserName"];
                    gridDataTable.Rows.Add(newRow);
                }
                myConnection.Close();
            }
            CommentGrid.DataSource = gridDataTable;
        }
        CommentGrid.DataBind();
    }
    protected void postComment(object sender, EventArgs e)
    {
        Debug.WriteLine(Request.Params["article_id"]);
        var userMembership = Membership.GetUser(User.Identity.Name);

        if (userMembership == null)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Erorr message", "alert(Only registered users can post comments')", true);
            return;
        }
        var postCommentTextBox = lvPostComment.FindControl("tbCommentText") as TextBox;
        //preprocess some stuff

        DateTime PostDate = DateTime.Now;
        Guid CommentId = Guid.NewGuid();
        Guid ArticleId;
        try {
            ArticleId = Guid.Parse(Request.Params["article_id"]);
        } catch(Exception ex) {
            Debug.WriteLine("Cannot parse guid at line 204 in article_view.aspx");
            Debug.WriteLine(ex.Message);
             ScriptManager.RegisterClientScriptBlock(this, GetType(),
                "Erorr message", "alert('Cannot convert guid :(')", true);
            return ;
        }
        var userId = (Guid)userMembership.ProviderUserKey;

        if (!CreateNewCommentIntoDB(PostDate, CommentId,
            ArticleId, postCommentTextBox.Text, userId))
        {

            ScriptManager.RegisterClientScriptBlock(this, GetType(),
                "Erorr message", "alert('Cannot insert comment in DB :(')", true);
        }
        else
        {
            Debug.WriteLine("Succesfully inserted comment");
        }
        Debug.WriteLine(postCommentTextBox.Text);
        Response.Redirect("article_view.aspx?article_id=" + ArticleId);
    }

    private bool CreateNewCommentIntoDB(DateTime PostDate, Guid CommentId, Guid ArticleId, string Content, Guid UserId)
    {
        var cursor = new SqlConnection(
            ConfigurationManager.ConnectionStrings["SqlServices"].ConnectionString
        );
        cursor.Open();
        const string insertComment =
            "INSERT INTO Comments VALUES(" +
            "@PostDate, @Content, @UserId, @ArticleId, @CommentId)";
        using (SqlCommand command = new SqlCommand(insertComment, cursor))
        {

            string sqlformattedtime = PostDate.ToString("yyyy-MM-dd HH:mm:ss");
            command.Parameters.AddWithValue("PostDate", sqlformattedtime);
            command.Parameters.AddWithValue("CommentId", CommentId);
            command.Parameters.AddWithValue("ArticleId", ArticleId);
            command.Parameters.AddWithValue("Content", Content);
            command.Parameters.AddWithValue("UserId", UserId);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        cursor.Close();
        return true;
    }
    
}