using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.Security;

public partial class review_news : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole("editor")) {
            Response.Redirect("MainPage.aspx");
        }
        var con = ConfigurationManager.ConnectionStrings["SqlServices"].ToString();
        using (SqlConnection myConnection = new SqlConnection(con))
        {
            string queryString =
                "SELECT     Articles.Title, Articles.ArticleId, Articles.PublishDate " +
                "FROM         Articles " +
                "WHERE     (Articles.PendingStatus = 1)";

            SqlCommand oCmd = new SqlCommand(queryString, myConnection);
            myConnection.Open();

            var gridDataTable = new DataTable();
            gridDataTable.Columns.Add("Title");
            gridDataTable.Columns.Add("ArticleId");
            gridDataTable.Columns.Add("PublishDate");
            using (SqlDataReader oReader = oCmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    var newRow = gridDataTable.NewRow();
                    newRow["Title"] = oReader["Title"];
                    newRow["ArticleId"] = oReader["ArticleId"];
                    newRow["PublishDate"] = oReader["PublishDate"];
                    gridDataTable.Rows.Add(newRow);
                }
                myConnection.Close();
            }
            GridView1.DataSource = gridDataTable;
        }
        GridView1.DataBind();
    }
}