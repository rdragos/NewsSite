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
public partial class news_categories : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.Params["category"] == null)
            {
                Response.Redirect("MainPage.aspx");
                return;
            }

            Debug.WriteLine("In PostBack");
            WriteHtml(Request.Params["category"]);
        }
    }
    protected void WriteHtml(String category_name)
    {
        var con = ConfigurationManager.ConnectionStrings["SqlServices"].ToString();
        using (SqlConnection myConnection = new SqlConnection(con))
        {
            string queryString =
                "SELECT     Articles.Title, Articles.ArticleId, Articles.PublishDate, " +
            "aspnet_Users.UserName " +
            "FROM Articles INNER JOIN aspnet_Users ON " +
            "Articles.CreatedBy = aspnet_Users.UserId " + 
            "WHERE     (Articles.CategoryName =@fcatName) AND Articles.PendingStatus=0";

            SqlCommand oCmd = new SqlCommand(queryString, myConnection);
            oCmd.Parameters.AddWithValue("@fcatName", category_name);
            myConnection.Open();

            var gridDataTable = new DataTable();
            gridDataTable.Columns.Add("Title");
            gridDataTable.Columns.Add("ArticleId");
            gridDataTable.Columns.Add("PublishDate");
            gridDataTable.Columns.Add("PublisherName");
            using (SqlDataReader oReader = oCmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    var newRow = gridDataTable.NewRow();
                    newRow["Title"] = oReader["Title"];
                    newRow["ArticleId"] = oReader["ArticleId"];
                    newRow["PublishDate"] = oReader["PublishDate"];
                    newRow["PublisherName"] = oReader.GetString(3);
                    gridDataTable.Rows.Add(newRow);
                }
                myConnection.Close();
            }
            Session["TaskTable"] = gridDataTable;
            GridView1.DataSource = Session["TaskTable"];
        }
        GridView1.DataBind();
    }
    protected void gvName_Sorting( object sender, GridViewSortEventArgs e )
    {
        DataTable dt = Session["TaskTable"] as DataTable;
        if (dt != null)
        {
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            GridView1.DataSource = Session["TaskTable"];
            GridView1.DataBind();
        }
    }
    private string GetSortDirection(string column)
    {
        string sortDirection = "ASC";

        string sortExpression = ViewState["SortExpression"] as string;
        if (sortExpression != null)
        {
            if (sortExpression == column)
            {
                string lastDirection = ViewState["SortDirection"] as string;
                if ((lastDirection != null) && (lastDirection == "ASC"))
                {
                    sortDirection = "DESC";
                }
            }
        }
        ViewState["SortDirection"] = sortDirection;
        ViewState["SortExpression"] = column;

        return sortDirection;
    }
}
