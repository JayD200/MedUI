using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MedUI
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["viewID"] == null)
            {
                Session["viewID"] = 0;
            }
        }

        protected void viewButton(object sender, EventArgs e)
        {
            if ((int)Session["viewID"] == 0)
            {
                //System.Diagnostics.Debug.WriteLine("Changing to Staff view " + Session["viewID"].ToString());
                Session["viewID"] = 1;
                viewType.Text = "Staff View";
                LoginText.Text = "Agent Login";
                LastNameDiv.Visible = false;
                FirstName.Visible = false;
                Email.Visible = true;
            }
            else if ((int)Session["viewID"] == 1)
            {
                //System.Diagnostics.Debug.WriteLine("Changing to Patient view " + Session["viewID"].ToString()); 
                Session["viewID"] = 0;
                viewType.Text = "Patient View";
                LoginText.Text = "Patient Login";
                LastNameDiv.Visible = true;
                FirstName.Visible = true;
                Email.Visible = false;
            }
        }

        protected void Connect()
        {

        }
    }
}