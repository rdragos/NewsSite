using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
public partial class admin_page : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole("admin"))
        {
            Response.Redirect("MainPage.aspx");
            return;
        }
        writeHtmlWithUsers();
    }

    private void writeHtmlWithUsers()
    {
        var con = ConfigurationManager.ConnectionStrings["SqlServices"].ToString();
        using (SqlConnection myConnection = new SqlConnection(con))
        {
            string queryString = "SELECT aspnet_Users.UserName, aspnet_Roles.RoleName, aspnet_Users.UserId " +
                      "FROM aspnet_Roles INNER JOIN " + 
                      "aspnet_UsersInRoles ON aspnet_Roles.RoleId = aspnet_UsersInRoles.RoleId " +
                      "INNER JOIN " + 
                      "aspnet_Users ON aspnet_UsersInRoles.UserId = aspnet_Users.UserId";

            SqlCommand oCmd = new SqlCommand(queryString, myConnection);
            myConnection.Open();

            var gridDataTable = new DataTable();
            gridDataTable.Columns.Add("UserName");
            gridDataTable.Columns.Add("RoleName");
            gridDataTable.Columns.Add("UserId");
            try
            {
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        var newRow = gridDataTable.NewRow();
                        newRow["UserName"] = oReader.GetString(0);
                        newRow["RoleName"] = oReader.GetString(1);
                        newRow["UserId"] = oReader.GetGuid(2);
                        gridDataTable.Rows.Add(newRow);
                    }
                    myConnection.Close();

                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine("OH NO ANOTHER EXCEPTION");
                Debug.WriteLine(ex.Message); 
            }
            GridView1.DataSource = gridDataTable;
            GridView1.DataBind();
        }
    }
}