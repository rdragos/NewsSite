using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e){
    }
    public void GenerateSQL(object sender, EventArgs e) {
        if (!User.Identity.IsAuthenticated)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "ErrorMessage", "alert('Unable to create post because no user is logged in.')", true);
            return;
        }
        var membershipUser = Membership.GetUser(User.Identity.Name);
        if (membershipUser == null || membershipUser.ProviderUserKey == null)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "ErrorMessage", "alert('Unable to create post because no logged user cant be found.')", true);
            return;
        }

        Guid userId = (Guid)membershipUser.ProviderUserKey;
        if (!insertArticle("Matrimoniale", "Astazi", "Baiat Gigel caut fata Miruna",userId))
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "ErrorMessage",
               "alert('Uai, Uai, si fasi aci?')", true);
        }
        insertArticle("Politica", "Rezultate Alegeri", "bla bla bla", userId);
        insertArticle("Meteo", "Vremea in urmatorii 10 ani", "Astazi va fi viscol", userId);
    }
    public bool insertArticle(
        string categoryName, string title,
        string content,
        Guid userId) {
        var cursor = new SqlConnection(
            ConfigurationManager.ConnectionStrings["SqlServices"].ConnectionString
        );
        cursor.Open();

        const string insertArticles =
            "INSERT INTO Articles VALUES(" +
            "@Title, @Content, @PublishDate, @CategoryName, @CreatedBy, @ArticleId)";

        const string insertCategories = "INSERT INTO Categories VALUES(@Name)";

        using (SqlCommand command = new SqlCommand(insertCategories, cursor))
        {
            command.Parameters.AddWithValue("Name", categoryName);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception) { }
        }
        using (SqlCommand command = new SqlCommand(insertArticles, cursor))
        {

            command.Parameters.AddWithValue("ArticleId", Guid.NewGuid());
            command.Parameters.AddWithValue("Title", title);
            command.Parameters.AddWithValue("Content", content);
            DateTime mytime = DateTime.Now;
            string sqlformattedtime = mytime.ToString("yyyy-MM-dd HH:mm:ss");

            command.Parameters.AddWithValue("PublishDate", sqlformattedtime);
            command.Parameters.AddWithValue("CategoryName", categoryName);
            command.Parameters.AddWithValue("CreatedBy", userId);

            command.ExecuteNonQuery();

        }
        cursor.Close();
        return true;

    }

}