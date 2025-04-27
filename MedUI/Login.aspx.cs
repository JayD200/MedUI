using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MedUI
{
    public partial class Login : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MedInfoDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["viewID"] == null)
            {
                Session["viewID"] = 0;
            }
            Session["PatientID"] = null;
            Session["AgentID"] = null;

            LoginError.Text = " ";
        }

        protected void viewButton(object sender, EventArgs e)
        {
            if ((int)Session["viewID"] == 0)
            {
                //System.Diagnostics.Debug.WriteLine("Changing to Staff view " + Session["viewID"].ToString());
                Session["viewID"] = 1;
                viewType.Text = "Patient View";
                LoginText.Text = "Agent Login";
                LastNameDiv.Visible = false;
                FirstName.Visible = false;
                Email.Visible = true;
            }
            else if ((int)Session["viewID"] == 1)
            {
                //System.Diagnostics.Debug.WriteLine("Changing to Patient view " + Session["viewID"].ToString()); 
                Session["viewID"] = 0;
                viewType.Text = "Agent View";
                LoginText.Text = "Patient Login";
                LastNameDiv.Visible = true;
                FirstName.Visible = true;
                Email.Visible = false;
            }
        }

        protected void LoginButton(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    if ((int)Session["viewID"] == 1)
                    {
                        string emailID = Email.Text.Trim();
                        object result;

                        if (emailID.Equals("Patient", StringComparison.OrdinalIgnoreCase))
                        {
                            result = null;
                        }
                        else
                        {
                            string sql = "Select AgentEmail FROM Agent WHERE AgentEmail = @Email";

                            cmd.CommandText = sql;
                            cmd.Parameters.AddWithValue("@Email", emailID);

                            result = cmd.ExecuteScalar();
                        }
                        if (result != null)
                        {
                            Session["AgentID"] = result;
                            System.Diagnostics.Debug.WriteLine(result);
                            Response.Redirect("PatientList.aspx");
                        }
                        else
                        {
                            LoginError.Text = "Agent does not exist.";
                        }

                    }
                    else
                    {
                        string fname = FirstName.Text.Trim();
                        string lname = LastName.Text.Trim();

                        string sql = "SELECT PatientID FROM Patient WHERE F_Name = @FName AND L_Name = @LName";
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@FName", fname);
                        cmd.Parameters.AddWithValue("@LName", lname);

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            Session["PatientID"] = Convert.ToInt32(result);
                            System.Diagnostics.Debug.WriteLine(result);
                            Response.Redirect("PatientScreen.aspx");
                        }
                        else
                        {
                            LoginError.Text = "Patient does not exist.";
                        }

                    }
                }
                catch (Exception ex) 
                {
                    LoginError.Text = "User does not exist";
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}