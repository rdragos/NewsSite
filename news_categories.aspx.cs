using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
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
                "SELECT     Articles.Title " +
                "FROM         Articles INNER JOIN " +
                "Categories ON Articles.CategoryName = Categories.Name " +
                "WHERE     (Categories.Name =@fcatName)";

            SqlCommand oCmd = new SqlCommand(queryString, myConnection);
            oCmd.Parameters.AddWithValue("@fcatName", category_name);
            myConnection.Open();
            using (SqlDataReader oReader = oCmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    Debug.WriteLine(oReader["Title"].ToString());
                }
                myConnection.Close();
            }
        }
    }
}