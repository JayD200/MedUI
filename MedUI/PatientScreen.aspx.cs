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
            else
            {
                if (Session["viewID"] != null && (int)Session["viewID"] == 1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "showDivs", "showAgentDivs();", true);
                    CheckForExistingFlag(Convert.ToInt32(Rx_Info.DataKeys[0].Value));
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "hideDivs", "hideAgentDivs();", true);
                }
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
            string query = @"
                SELECT PatientID, F_Name, L_Name, Phone, DateOfBirth, Gender, Email, Addrs_Street, Addrs_City, Addrs_State, Addrs_Zipcode 
                FROM Patient WHERE PatientID = @PatientID;

                SELECT InsuranceID, Name, CardholderID, RxBIN, RxGrp, RxPCN, InsuranceClassification, InsType 
                FROM Insurance WHERE PatientID = @PatientID;

                SELECT PrescriptionID, Prescription_Name, Prescription_Dose, Prescription_DaySupply, Prescription_Quantity, Doctor_Name, Doctor_Phone, Doctor_Address 
                FROM Prescription WHERE PatientID = @PatientID;

                SELECT Flag_status, Flag_DueDate, Flag_note, PrescriptionID 
                FROM Prescription WHERE PatientID = @PatientID AND Flag_status IS NOT NULL;

                SELECT Log.Notes, Log.Date, Log.AgentID 
                FROM Log 
                JOIN Prescription ON Log.PrescriptionID = Prescription.PrescriptionID 
                WHERE Prescription.PatientID = @PatientID
                ORDER BY Date DESC;
            ";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MedInfoDB"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PatientID", patientId);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // 1. Patient Info
                    Pt_Info.DataSource = reader;
                    Pt_Info.DataBind();

                    // 2. Insurance Info
                    reader.NextResult();
                    Ins_Info.DataSource = reader;
                    Ins_Info.DataBind();

                    // 3. Prescription Info
                    reader.NextResult();
                    Rx_Info.DataSource = reader;
                    Rx_Info.DataBind();

                    // 4. Flag Info (Only bind if viewID is 1)
                    reader.NextResult();
                    if ((int)Session["viewID"] == 1)
                    {
                        Flag_info.DataSource = reader;
                        Flag_info.DataBind();
                    }

                    // 5. Log Info (Only bind if viewID is 1)
                    reader.NextResult();
                    if ((int)Session["viewID"] == 1)
                    {
                        Log_Info.DataSource = reader;
                        Log_Info.DataBind();
                    }
                }

            }
            if ((int)Session["viewID"] == 1)
            {
                CheckForExistingFlag(Convert.ToInt32(Rx_Info.DataKeys[0].Value));
            }
        }

        private void CheckForExistingFlag(int prescriptionId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MedInfoDB"].ConnectionString))
            {
                string query = "SELECT COUNT(*) FROM Prescription WHERE PrescriptionID = @PrescriptionID AND Flag_status IS NOT NULL";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PrescriptionID", prescriptionId);
                    conn.Open();

                    int flagCount = (int)cmd.ExecuteScalar(); // Count number of existing flags
                    Button1.Visible = flagCount == 0; // Hide the button if there are existing flags
                }
            }
        }



        /*Patient info updating*/
        protected void Pt_Info_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            Pt_Info.EditIndex = e.NewEditIndex;
            BindListViewControls(); // rebind data
        }

        protected void Pt_Info_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            Pt_Info.EditIndex = -1;
            BindListViewControls();
        }

        protected void Pt_Info_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            ListViewItem item = Pt_Info.Items[e.ItemIndex];
            string patientId = Pt_Info.DataKeys[e.ItemIndex].Value.ToString();

            // Find all controls
            TextBox txtFName = (TextBox)item.FindControl("txtEditFName");
            TextBox txtLName = (TextBox)item.FindControl("txtEditLName");
            TextBox txtPhone = (TextBox)item.FindControl("txtEditPhone");
            TextBox txtDOB = (TextBox)item.FindControl("txtEditDOB");
            DropDownList ddlGender = (DropDownList)item.FindControl("ddlEditGender");
            TextBox txtEmail = (TextBox)item.FindControl("txtEditEmail");
            TextBox txtStreet = (TextBox)item.FindControl("txtStreet");
            TextBox txtCity = (TextBox)item.FindControl("txtCity");
            TextBox txtState = (TextBox)item.FindControl("txtState");
            TextBox txtZip = (TextBox)item.FindControl("txtZip");

            // Get new values
            string fName = txtFName.Text.Trim();
            string lName = txtLName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string dob = txtDOB.Text.Trim();
            string gender = ddlGender.SelectedValue;
            string email = txtEmail.Text.Trim();
            string street = txtStreet.Text.Trim();
            string city = txtCity.Text.Trim();
            string state = txtState.Text.Trim();
            string zip = txtZip.Text.Trim();

            DateTime parsedDOB;
            if (!DateTime.TryParse(dob, out parsedDOB))
            {
                // Optional: show error message
                return;
            }

            // Get current database values BEFORE updating
            Dictionary<string, string> currentValues = new Dictionary<string, string>();

            string connStr = ConfigurationManager.ConnectionStrings["MedInfoDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string selectQuery = @"SELECT F_Name, L_Name, Phone, DateOfBirth, Gender, Email, 
                                      Addrs_Street, Addrs_City, Addrs_State, Addrs_Zipcode 
                               FROM Patient WHERE PatientID = @PatientID";
                using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@PatientID", patientId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        currentValues["F_Name"] = reader["F_Name"].ToString();
                        currentValues["L_Name"] = reader["L_Name"].ToString();
                        currentValues["Phone"] = reader["Phone"].ToString();
                        currentValues["DateOfBirth"] = Convert.ToDateTime(reader["DateOfBirth"]).ToString("yyyy-MM-dd");
                        currentValues["Gender"] = reader["Gender"].ToString();
                        currentValues["Email"] = reader["Email"].ToString();
                        currentValues["Addrs_Street"] = reader["Addrs_Street"].ToString();
                        currentValues["Addrs_City"] = reader["Addrs_City"].ToString();
                        currentValues["Addrs_State"] = reader["Addrs_State"].ToString();
                        currentValues["Addrs_Zipcode"] = reader["Addrs_Zipcode"].ToString();
                    }
                    conn.Close();
                }
            }

            // Now update the database
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string updateQuery = @"UPDATE Patient SET 
                        F_Name = @FName,
                        L_Name = @LName,
                        Phone = @Phone,
                        DateOfBirth = @DOB,
                        Gender = @Gender,
                        Email = @Email,
                        Addrs_Street = @Street,
                        Addrs_City = @City,
                        Addrs_State = @State,
                        Addrs_Zipcode = @Zip
                       WHERE PatientID = @PatientID";

                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@FName", fName);
                    cmd.Parameters.AddWithValue("@LName", lName);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@DOB", parsedDOB);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Street", street);
                    cmd.Parameters.AddWithValue("@City", city);
                    cmd.Parameters.AddWithValue("@State", state);
                    cmd.Parameters.AddWithValue("@Zip", zip);
                    cmd.Parameters.AddWithValue("@PatientID", patientId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // Detect changes and log them
            List<string> changeNotes = new List<string>();
            if (currentValues["F_Name"] != fName) changeNotes.Add($"First Name changed from '{currentValues["F_Name"]}' to '{fName}'");
            if (currentValues["L_Name"] != lName) changeNotes.Add($"Last Name changed from '{currentValues["L_Name"]}' to '{lName}'");
            if (currentValues["Phone"] != phone) changeNotes.Add($"Phone changed from '{currentValues["Phone"]}' to '{phone}'");
            if (currentValues["DateOfBirth"] != parsedDOB.ToString("yyyy-MM-dd")) changeNotes.Add($"DOB changed from '{currentValues["DateOfBirth"]}' to '{parsedDOB:yyyy-MM-dd}'");
            if (currentValues["Gender"] != gender) changeNotes.Add($"Gender changed from '{currentValues["Gender"]}' to '{gender}'");
            if (currentValues["Email"] != email) changeNotes.Add($"Email changed from '{currentValues["Email"]}' to '{email}'");
            if (currentValues["Addrs_Street"] != street) changeNotes.Add($"Street changed from '{currentValues["Addrs_Street"]}' to '{street}'");
            if (currentValues["Addrs_City"] != city) changeNotes.Add($"City changed from '{currentValues["Addrs_City"]}' to '{city}'");
            if (currentValues["Addrs_State"] != state) changeNotes.Add($"State changed from '{currentValues["Addrs_State"]}' to '{state}'");
            if (currentValues["Addrs_Zipcode"] != zip) changeNotes.Add($"Zip changed from '{currentValues["Addrs_Zipcode"]}' to '{zip}'");

            if (changeNotes.Count > 0)
            {
                string agentId = (Session["viewID"] != null && (int)Session["viewID"] == 1) ? Session["AgentID"].ToString() : "Patient";
                int prescriptionId = 0; // default if you don't have one yet

                if (Rx_Info.Items.Count > 0)
                {
                    // Assuming you want the first PrescriptionID associated
                    prescriptionId = Convert.ToInt32(Rx_Info.DataKeys[0].Value);
                }

                foreach (var note in changeNotes)
                {
                    InsertLog(prescriptionId, agentId, note);
                }
            }

            Pt_Info.EditIndex = -1;
            BindListViewControls(); // refresh ListView
        }


        /*Insurance Info updating*/
        protected void Ins_Info_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            Ins_Info.EditIndex = e.NewEditIndex;
            BindListViewControls();
        }

        protected void Ins_Info_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            Ins_Info.EditIndex = -1;
            BindListViewControls();
        }

        protected void Ins_Info_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            ListViewItem item = Ins_Info.Items[e.ItemIndex];
            string insuranceId = Ins_Info.DataKeys[e.ItemIndex].Value.ToString();

            // Find controls
            TextBox txtName = (TextBox)item.FindControl("txtEditName");
            DropDownList ddlInsuranceClassification = (DropDownList)item.FindControl("ddlEditClassification");
            TextBox txtRxBin = (TextBox)item.FindControl("txtEditRxBin");
            TextBox txtRxGrp = (TextBox)item.FindControl("txtEditRxGrp");
            TextBox txtRxPCN = (TextBox)item.FindControl("txtEditRxPCN");
            //TextBox txtPersonCode = (TextBox)item.FindControl("txtEditPersonCode");
            //TextBox txtPhone = (TextBox)item.FindControl("txtEditPhone");
            DropDownList ddlInsType = (DropDownList)item.FindControl("ddlEditInsType");

            // New values
            string name = txtName.Text.Trim();
            string insuranceClassification = ddlInsuranceClassification.SelectedValue;
            string rxBin = txtRxBin.Text.Trim();
            string rxGrp = txtRxGrp.Text.Trim();
            string rxPCN = txtRxPCN.Text.Trim();
            //string personCode = txtPersonCode?.Text.Trim() ?? "";
            //string phone = txtPhone?.Text.Trim() ?? "";
            string insType = ddlInsType.SelectedValue;

            // 1. Get current values
            Dictionary<string, string> currentValues = new Dictionary<string, string>();
            string connStr = ConfigurationManager.ConnectionStrings["MedInfoDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string selectQuery = @"SELECT Name, InsuranceClassification, RxBin, RxGrp, RxPCN, PersonCode, Phone, InsType
                       FROM Insurance WHERE InsuranceID = @InsuranceID";
                using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@InsuranceID", insuranceId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        currentValues["Name"] = reader["Name"].ToString();
                        currentValues["InsuranceClassification"] = reader["InsuranceClassification"].ToString();
                        currentValues["RxBin"] = reader["RxBin"].ToString();
                        currentValues["RxGrp"] = reader["RxGrp"].ToString();
                        currentValues["RxPCN"] = reader["RxPCN"].ToString();
                        //currentValues["PersonCode"] = reader["PersonCode"].ToString();
                        //currentValues["Phone"] = reader["Phone"].ToString();
                        currentValues["InsType"] = reader["InsType"].ToString();
                    }
                    conn.Close();
                }
            }

            // 2. Update the database
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string updateQuery = @"UPDATE Insurance SET 
                           Name = @Name,
                           InsuranceClassification = @InsuranceClassification,
                           RxBin = @RxBin,
                           RxGrp = @RxGrp,
                           RxPCN = @RxPCN,
                           InsType = @InsType
                       WHERE InsuranceID = @InsuranceID";

                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", (object)name ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@InsuranceClassification", (object)insuranceClassification ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@RxBin", (object)rxBin ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@RxGrp", (object)rxGrp ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@RxPCN", (object)rxPCN ?? DBNull.Value);
                    //cmd.Parameters.AddWithValue("@PersonCode", (object)personCode ?? DBNull.Value);
                    //cmd.Parameters.AddWithValue("@Phone", (object)phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@InsType", (object)insType ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@InsuranceID", insuranceId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // 3. Compare and log
            List<string> changeNotes = new List<string>();
            if (currentValues["Name"] != name) changeNotes.Add($"Name changed from '{currentValues["Name"]}' to '{name}'");
            if (currentValues["InsuranceClassification"] != insuranceClassification) changeNotes.Add($"Insurance Classification changed from '{currentValues["InsuranceClassification"]}' to '{insuranceClassification}'");
            if (currentValues["RxBin"] != rxBin) changeNotes.Add($"RxBin changed from '{currentValues["RxBin"]}' to '{rxBin}'");
            if (currentValues["RxGrp"] != rxGrp) changeNotes.Add($"RxGrp changed from '{currentValues["RxGrp"]}' to '{rxGrp}'");
            if (currentValues["RxPCN"] != rxPCN) changeNotes.Add($"RxPCN changed from '{currentValues["RxPCN"]}' to '{rxPCN}'");
            //if (currentValues["PersonCode"] != personCode) changeNotes.Add($"PersonCode changed from '{currentValues["PersonCode"]}' to '{personCode}'");
            //if (currentValues["Phone"] != phone) changeNotes.Add($"Phone changed from '{currentValues["Phone"]}' to '{phone}'");
            if (currentValues["InsType"] != insType) changeNotes.Add($"Insurance Type changed from '{currentValues["InsType"]}' to '{insType}'");

            if (changeNotes.Count > 0)
            {
                string agentId = (Session["viewID"] != null && (int)Session["viewID"] == 1) ? Session["AgentID"].ToString() : "Patient";
                int prescriptionId = 0;

                if (Rx_Info.Items.Count > 0)
                {
                    prescriptionId = Convert.ToInt32(Rx_Info.DataKeys[0].Value);
                }

                foreach (var note in changeNotes)
                {
                    InsertLog(prescriptionId, agentId, note);
                }
            }

            Ins_Info.EditIndex = -1;
            BindListViewControls();
        }

        protected void FlagCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "CompleteFlag")
            {
                int prescriptionId = Convert.ToInt32(e.CommandArgument); // Retrieve PrescriptionID from CommandArgument.

                string flagNote = ""; // Example note when completing a flag. Adjust as necessary.

                // Update the database to mark the flag as completed
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MedInfoDB"].ConnectionString))
                {
                    string updateQuery = @"
                        SELECT Flag_note
                        FROM Prescription
                        WHERE PrescriptionID = @PrescriptionID;
                        
                        UPDATE Prescription
                        SET Flag_status = NULL, Flag_DueDate = NULL
                        WHERE PrescriptionID = @PrescriptionID;";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PrescriptionID", prescriptionId);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        flagNote = cmd.ExecuteScalar()?.ToString() ?? "Flag Complete";
                    }
                }

                // Log the flag completion
                string logMessage = $"Flag Completed: '{flagNote}'";
                string agentId = Session["AgentID"].ToString(); // Assuming the agent's ID is stored in session.
                InsertLog(prescriptionId, agentId, logMessage);

                // Refresh the ListView after completing the flag
                BindListViewControls();
            }
        }


        protected void btnSubmitFlag_Click(object sender, EventArgs e)
        {
            // Retrieve the PrescriptionID from the DataKeys collection
            int prescriptionId = Convert.ToInt32(Rx_Info.DataKeys[0].Value); // Use the correct index based on your item in the ListView

            string flagNote = txtFlagNote.Value.Trim(); // Assuming you have a TextBox or similar control for flag note.
            DateTime flagDueDate = Convert.ToDateTime(txtFlagDate.Value); // Assuming a TextBox for the due date.

            // Insert the flag into the database
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MedInfoDB"].ConnectionString))
            {
                string updateQuery = @"
                    UPDATE Prescription
                    SET Flag_status = 1, Flag_DueDate = @FlagDueDate, Flag_note = @FlagNote
                    WHERE PrescriptionID = @PrescriptionID";

                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@PrescriptionID", prescriptionId);
                    cmd.Parameters.AddWithValue("@FlagDueDate", flagDueDate);
                    cmd.Parameters.AddWithValue("@FlagNote", flagNote);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // Log the flag creation
            string logMessage = $"Flag Created: '{flagNote}'";
            string agentId = Session["AgentID"].ToString(); // Assuming the agent's ID is stored in session.
            InsertLog(prescriptionId, agentId, logMessage);

            // Refresh the ListView after submitting the flag
            BindListViewControls();
        }

        /*
        private void LogFlagCompletion(int flagId)
        {
            // You can log this flag completion to a log table if needed
            string query = "INSERT INTO Log (FlagID, AgentID, Notes, Date) VALUES (@FlagID, @AgentID, 'Flag Completed', @Date)";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MedInfoDB"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@FlagID", flagId);
                cmd.Parameters.AddWithValue("@AgentID", Session["AgentID"] ?? "Patient");  // Use the appropriate agent ID
                cmd.Parameters.AddWithValue("@Date", DateTime.Now);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void BindFlagInfo()
        {
            // Query the database to bind updated flag data
            string query = "SELECT FlagID, Flag_status, Flag_DueDate, Flag_note FROM Flag WHERE PatientID = @PatientID";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MedInfoDB"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PatientID", Session["PatientID"]);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                Flag_info.DataSource = reader;
                Flag_info.DataBind();
            }
        }

        protected void btnCreateFlag_Click(object sender, EventArgs e)
        {
            // Show the modal when Create Flag is clicked
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "showModal();", true);
        }

        protected void btnSubmitFlag_Click(object sender, EventArgs e)
        {
            // Get the patient ID and other necessary details
            string patientId = Session["PatientID"].ToString();
            string flagDate = txtFlagDate.Value;  // Assuming this is a DateTime field
            string flagNote = txtFlagNote.Value;

            if (string.IsNullOrEmpty(flagDate) || string.IsNullOrEmpty(flagNote))
            {
                // Optionally show an error message if the fields are empty
                return;
            }

            // Insert the new flag into the database
            string query = "INSERT INTO Flag (PatientID, Flag_status, Flag_DueDate, Flag_note) VALUES (@PatientID, 'Active', @Flag_DueDate, @Flag_note)";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MedInfoDB"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PatientID", patientId);
                cmd.Parameters.AddWithValue("@Flag_DueDate", flagDate); // Date should be passed in the correct format
                cmd.Parameters.AddWithValue("@Flag_note", flagNote);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Close the modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", "cancelFlag();", true);

            // Rebind the ListView to show the new flag
            BindFlagInfo();
        }
        */



        private void InsertLog(int prescriptionId, string agentId, string note)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MedInfoDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string insertQuery = @"INSERT INTO Log (PrescriptionID, AgentID, Notes, Date) 
                               VALUES (@PrescriptionID, @AgentID, @Notes, @Date)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@PrescriptionID", prescriptionId);
                    cmd.Parameters.AddWithValue("@AgentID", agentId);
                    cmd.Parameters.AddWithValue("@Notes", note);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}