using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


namespace MedUI
{
    public partial class PatientScreen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PatientID"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (!Page.IsPostBack)
            {
                if (Session["viewID"] != null && (int)Session["viewID"] == 1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "showDivs", "showAgentDivs();", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "hideDivs", "hideAgentDivs();", true);
                }
                BindListViewControls();
            }            
        }

        protected void viewAllPatients(object sender, EventArgs e)
        {
            Session["PatientID"] = null;
            Response.Redirect("PatientList.aspx");
        }
        private void BindListViewControls()
        {
            int patientId = Convert.ToInt32(Session["PatientID"]);

            LoadPatientInfo(patientId);
            LoadInsuranceInfo(patientId);
            LoadPrescriptionInfo(patientId);
            LoadFlagInfo(patientId);
        }

        private void LoadPatientInfo(int patientId)
        {
            string query = "SELECT F_Name, L_Name, Phone, DateOfBirth, Gender, Email, Addrs_Zipcode FROM Patient WHERE PatientID = @PatientID";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MedInfoDB"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PatientID", patientId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                Pt_Info.DataSource = reader;
                Pt_Info.DataBind();
            }
        }

        private void LoadInsuranceInfo(int patientId)
        {
            string query = "SELECT Name, CardholderID, RxBIN, RxGrp, RxPCN, InsuranceClassification, InsType FROM Insurance WHERE PatientID = @PatientID";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MedInfoDB"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PatientID", patientId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                Ins_Info.DataSource = reader;
                Ins_Info.DataBind();
            }
        }

        private void LoadPrescriptionInfo(int patientId)
        {
            string query = @"SELECT Prescription_Name, Prescription_Dose, Prescription_DaySupply, Prescription_Quantity,
                            Doctor_Name, Doctor_Phone, Doctor_Address
                     FROM Prescription
                     WHERE PatientID = @PatientID";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MedInfoDB"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PatientID", patientId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                Rx_Info.DataSource = reader;
                Rx_Info.DataBind();
            }
        }

        private void LoadFlagInfo(int patientId)
        {
            string query = @"SELECT Flag_status, Flag_DueDate, Flag_note 
                     FROM Prescription 
                     WHERE PatientID = @PatientID AND Flag_status IS NOT NULL";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MedInfoDB"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PatientID", patientId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                Flag_info.DataSource = reader;
                Flag_info.DataBind();
            }
        }
    }
}