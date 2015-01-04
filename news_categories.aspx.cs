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
                "SELECT     Articles.Title, Articles.ArticleId, Articles.PublishDate " +
                "FROM         Articles INNER JOIN " +
                "Categories ON Articles.CategoryName = Categories.CategoryName " +
                "WHERE     (Categories.CategoryName =@fcatName)";

            SqlCommand oCmd = new SqlCommand(queryString, myConnection);
            oCmd.Parameters.AddWithValue("@fcatName", category_name);
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