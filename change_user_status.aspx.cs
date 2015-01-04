using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

public partial class change_user_status : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole("admin"))
        {
            Response.Redirect("MainPage.aspx");
        }
        Debug.WriteLine("LOL IN change user status page load");
        var userId = Request.Params["userId"];
        var status = Request.Params["role"];
        Debug.WriteLine(userId);
        Debug.WriteLine(status);
        if (userId == null || status == null)
        {
            Response.Redirect("MainPage.aspx");
        }

        if (!changeStatus(userId, status))
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "ErrorMessage",
               "alert('Date incorecte sa caut in DB')", true);
        }
        Response.Redirect("admin_page.aspx");
    }

    private bool changeStatus(string userId, string status)
    {
        var role_id = getRoleId(status);
        if (role_id == null)
        {
            return false;
        }

        //now modify the DB with userId and proper role_id
        var con = ConfigurationManager.ConnectionStrings["SqlServices"].ToString();
        using (SqlConnection myConnection = new SqlConnection(con))
        {
            string updateString =
                "UPDATE aspnet_UsersInRoles SET RoleId=@fRoleId " +
                "WHERE (aspnet_UsersInRoles.UserId = @fUserId)";
            SqlCommand oCmd = new SqlCommand(updateString, myConnection);

            oCmd.Parameters.AddWithValue("@fRoleId", role_id);
            oCmd.Parameters.AddWithValue("@fUserId", new Guid(userId));
            myConnection.Open();
            try
            {
                oCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "ErrorMessage",
                   "alert('Cannot change role in db')", true);
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        return true;

    }
    string getRoleId(string status)
    {

        var result = "";
        var con = ConfigurationManager.ConnectionStrings["SqlServices"].ToString();
        using (SqlConnection myConnection = new SqlConnection(con))
        {
            string queryString = "SELECT TOP (1) RoleId " +
            "FROM         aspnet_Roles " +
            "WHERE     (RoleName = @fstatus) ";

            SqlCommand oCmd = new SqlCommand(queryString, myConnection);
            oCmd.Parameters.AddWithValue("@fstatus", status);
            myConnection.Open();

            try
            {
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {

                    while (oReader.Read())
                    {
                        result = oReader.GetGuid(0).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        return result;
    }
}