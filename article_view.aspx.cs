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
                WriteHtml(newGuid);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Response.Redirect("google.ro");
                return;
            }
        }
    }
    protected void WriteHtml(Guid article_id)
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

        var comment_grid = new DataTable();
        comment_grid.Columns.Add("Comments");
        var yelRow = comment_grid.NewRow();
        yelRow["Comments"] = "Yellow world";
        comment_grid.Rows.Add(yelRow);
        CommentGrid.DataSource = comment_grid;
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
        Debug.WriteLine(postCommentTextBox.Text);
    }
    
}