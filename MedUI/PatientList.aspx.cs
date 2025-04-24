using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Reflection.Emit;
using System.Text;

namespace MedUI
{
    public partial class PatientList : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MedInfoDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Session["viewID"] != null && (int)Session["viewID"] == 1 && Session["AgentID"] != null))
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                // Only bind once to avoid overwriting user's selections
                BindPatientList();
            }
        }

        private void BindPatientList()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT p.PatientID, p.F_Name, p.L_Name, pr.Prescription_Name, pr.Doctor_Name, pr.Flag_status FROM Prescription pr INNER JOIN Patient p ON pr.PatientID = p.PatientID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        ListView1.DataSource = dt;
                        ListView1.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<p style='color:red;'>Error: " + ex.Message + "</p>");
                }
            }
        }

        protected void ViewProfile_Command(object sender, CommandEventArgs e)
        {
            int selectedId = Convert.ToInt32(e.CommandArgument);
            Session["PatientID"] = selectedId;
            Response.Redirect("PatientScreen.aspx");
        }

    }
}