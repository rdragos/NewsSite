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

    }

}
