using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;

public partial class propose_news : System.Web.UI.Page
{

    public const int IN_QUEUE_STATUS = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!User.Identity.IsAuthenticated)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "ErrorMessage", "alert('Unable to create post because no user is logged in.')", true);
            Response.Redirect("MainPage.aspx");
            return;
        }
        
    }
    protected void final_proposal(Object sender, EventArgs e)
    {
        Debug.WriteLine("Clicked");
        var membershipUser = Membership.GetUser(User.Identity.Name);
        if (membershipUser == null || membershipUser.ProviderUserKey == null)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "ErrorMessage", "alert('Unable to create post because no logged user cant be found.')", true);
            return;
        }
        Guid userId = (Guid)membershipUser.ProviderUserKey;

        string categoryName = CategoryTag.Text;
        string title = TitleTag.Text;
        string content = ContentTag.Text;
        string imgLink = ImageTag.Text;

        Debug.WriteLine(categoryName);
        Debug.WriteLine(title);
        Debug.WriteLine(content);
        if (!insertArticle(categoryName, title, content,userId, imgLink))
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "ErrorMessage",
               "alert('Uai, Uai, si fasi aci?')", true);
        }
    }
    public bool insertArticle(
        string categoryName, string title,
        string content,
        Guid userId, string imgLink) {
        var cursor = new SqlConnection(
            ConfigurationManager.ConnectionStrings["SqlServices"].ConnectionString
        );
        cursor.Open();

        const string insertArticles =
            "INSERT INTO Articles VALUES(" +
            "@ArticleId, @Title, @Content, @PublishDate, @CategoryName, @CreatedBy, @PendingStatus)";

        const string insertCategories = "INSERT INTO Categories VALUES(@CategoryName)";

        Guid ArticleId = Guid.NewGuid();
        using (SqlCommand command = new SqlCommand(insertCategories, cursor))
        {
            command.Parameters.AddWithValue("CategoryName", categoryName);
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
        using (SqlCommand command = new SqlCommand(insertArticles, cursor))
        {

            command.Parameters.AddWithValue("ArticleId", ArticleId);
            command.Parameters.AddWithValue("Title", title);
            command.Parameters.AddWithValue("Content", content);
            DateTime mytime = DateTime.Now;
            string sqlformattedtime = mytime.ToString("yyyy-MM-dd HH:mm:ss");
            command.Parameters.AddWithValue("PublishDate", sqlformattedtime);
            command.Parameters.AddWithValue("CategoryName", categoryName);
            command.Parameters.AddWithValue("CreatedBy", userId);
            command.Parameters.AddWithValue("PendingStatus", IN_QUEUE_STATUS);
            Debug.WriteLine(ArticleId);
            Debug.WriteLine(userId);
            
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

        if (imgLink.Length != 0)
        {
            //insert image
            const string insertImage = "INSERT INTO Thumbnails Values(" +
                "@ArticleId, @ThumbLink)";
            using (SqlCommand command = new SqlCommand(insertImage, cursor))
            {
                command.Parameters.AddWithValue("ArticleId", ArticleId);
                command.Parameters.AddWithValue("ThumbLink", imgLink);
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

        }
        cursor.Close();
        return true;

    }

}