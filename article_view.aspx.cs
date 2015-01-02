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
public partial class article_view : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.Params["article_id"] == null)
            {
                Response.Redirect("MainPage.aspx");
                return;
            }

            Debug.WriteLine("In PostBack");
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
        }
    }
    protected void WriteHtmlForArticle(Guid article_id)
    {
        var con = ConfigurationManager.ConnectionStrings["SqlServices"].ToString();
        using (SqlConnection myConnection = new SqlConnection(con))
        {
            //TODO: Fix the query string
            string queryString =
                "SELECT     Title, Content " +
                "FROM         Articles " +
                "WHERE     (Articles.ArticleId =@fArticleId)";
            SqlCommand oCmd = new SqlCommand(queryString, myConnection);
            oCmd.Parameters.AddWithValue("@fArticleId", article_id.ToString());
            var gridDataTable = new DataTable();
            gridDataTable.Columns.Add("Title");
            gridDataTable.Columns.Add("Content");

            myConnection.Open();
            using (SqlDataReader oReader = oCmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    var newRow = gridDataTable.NewRow();                    
                    newRow["Title"] = oReader["Title"];
                    newRow["Content"] = oReader["Content"];
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

        Debug.WriteLine("YOLO");
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
        Guid ArticleId = new Guid(Request.Params["article_id"]);
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
    }

    private bool CreateNewCommentIntoDB(DateTime PostDate, Guid CommentId, Guid ArticleId, string Content, Guid UserId)
    {
        var cursor = new SqlConnection(
            ConfigurationManager.ConnectionStrings["SqlServices"].ConnectionString
        );
        cursor.Open();
        const string insertComment =
            "INSERT INTO Comments VALUES(" +
            "@PostDate, @CommentId, @ArticleId, @Content, @UserId)";
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