using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void UpdateRole(object sender, EventArgs arg)
    {
        Roles.AddUserToRole(CreateUserWizard1.UserName, "user");
    }
    public void ProposeNews(object sender, EventArgs arg)
    {
        Response.Redirect("propose_news.aspx");
    }
    public void Review(object sender, EventArgs arg)
    {
        Response.Redirect("review_news.aspx");
    }
    public void dir_to_admin_page(object sender, EventArgs arg)
    {
        Response.Redirect("admin_page.aspx");
    }

}
