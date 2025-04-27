<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PatientScreen.aspx.cs" Inherits="MedUI.PatientScreen" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function showAgentDivs() {
            var divs = document.getElementsByClassName("AgentElement");
            for (var i = 0; i < divs.length; i++) {
                divs[i].style.display = "block";
            }
        }
        function hideAgentDivs() {
            var divs = document.getElementsByClassName("AgentElement");
            for (var i = 0; i < divs.length; i++) {
                divs[i].style.display = "none";
            }
        }
        function showModal() {
            document.getElementById("flagModal").style.display = "block";
        }

        // Close the modal when Cancel is clicked
        function cancelFlag() {
            document.getElementById("flagModal").style.display = "none";
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div style="padding: 30px;">
            <div style="text-align: center; text-decoration: none; color: black;">
                <a href="Login.aspx">Back to Login</a>
            </div>
            <div id="PatientInfo" style="padding-top: 5px;">
                <div runat="server" class="AgentElement" style="display: none;">
                    <asp:Button ID="PatientList" runat="server" Text="All Patients" OnClick="viewAllPatients" />
                </div>
                <div id="PageInfo" style="width: auto; height: auto;">
                    <div id="contentDiv" style="float: left; width: 80%;">
                        <div>
                            <h5>Patient Information</h5>
                            <asp:ListView ID="Pt_Info" runat="server" DataKeyNames="PatientID" OnItemEditing="Pt_Info_ItemEditing" OnItemUpdating="Pt_Info_ItemUpdating" OnItemCanceling="Pt_Info_ItemCanceling">
                                <LayoutTemplate>
                                    <table style="border: solid 2px #336699;" cellspacing="0" cellpadding="3" rules="all">
                                        <tr style="background-color: #336699; color: White;">
                                            <th>Name</th>
                                            <th>Phone Number</th>
                                            <th>Date of Birth</th>
                                            <th>Gender</th>
                                            <th>Email</th>
                                            <th>Address</th>
                                            <th>Edit</th>
                                        </tr>
                                        <tbody>
                                            <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                                        </tbody>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# string.Format("{0} {1}", Eval("F_Name"), Eval("L_Name")) %></td>
                                        <td><%# Eval("Phone") %></td>
                                        <td><%# Eval("DateOfBirth") %></td>
                                        <td><%# Eval("Gender") %></td>
                                        <td><%# Eval("Email") %></td>
                                        <td><%# string.Format("{0} {1}, {2} {3}", Eval("Addrs_Street"), Eval("Addrs_City"), Eval("Addrs_State"), Eval("Addrs_Zipcode")) %></td>
                                        <td>
                                            <asp:LinkButton runat="server" CommandName="Edit" Text="Edit" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <tr>
                                        <!-- Name Fields -->
                                        <td>
                                            <div class="form-group">
                                                <asp:TextBox ID="txtEditFName" runat="server" Text='<%# Bind("F_Name") %>' CssClass="form-control" Placeholder="First Name" />
                                                <asp:RequiredFieldValidator ID="rfvFName" runat="server" ControlToValidate="txtEditFName"
                                                    ErrorMessage="First name is required" ForeColor="Red" Display="Dynamic" CssClass="text-danger" />
                                            </div>
                                            <div class="form-group mt-2">
                                                <asp:TextBox ID="txtEditLName" runat="server" Text='<%# Bind("L_Name") %>' CssClass="form-control" Placeholder="Last Name" />
                                                <asp:RequiredFieldValidator ID="rfvLName" runat="server" ControlToValidate="txtEditLName"
                                                    ErrorMessage="Last name is required" ForeColor="Red" Display="Dynamic" CssClass="text-danger" />
                                            </div>
                                        </td>

                                        <!-- Phone -->
                                        <td>
                                            <div class="form-group">
                                                <asp:TextBox ID="txtEditPhone" runat="server" Text='<%# Bind("Phone") %>' CssClass="form-control" MaxLength="10" />
                                                <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtEditPhone"
                                                    ValidationExpression="^\d{10}$" ErrorMessage="Enter a valid 10-digit phone number" ForeColor="Red" Display="Dynamic" CssClass="text-danger" />
                                            </div>
                                        </td>

                                        <!-- DOB -->
                                        <td>
                                            <div class="form-group">
                                                <asp:TextBox ID="txtEditDOB" runat="server" Text='<%# Bind("DateOfBirth", "{0:yyyy-MM-dd}") %>'
                                                    TextMode="Date" CssClass="form-control" />
                                                <asp:RequiredFieldValidator ID="rfvDOB" runat="server" ControlToValidate="txtEditDOB"
                                                    ErrorMessage="Date of Birth is required" ForeColor="Red" Display="Dynamic" CssClass="text-danger" />
                                            </div>
                                        </td>

                                        <!-- Gender -->
                                        <td>
                                            <div class="form-group">
                                                <asp:DropDownList ID="ddlEditGender" runat="server" SelectedValue='<%# Bind("Gender") %>' CssClass="form-control">
                                                    <asp:ListItem Text="M" Value="M" />
                                                    <asp:ListItem Text="F" Value="F" />
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvGender" runat="server" ControlToValidate="ddlEditGender"
                                                    InitialValue="" ErrorMessage="Gender is required" ForeColor="Red" Display="Dynamic" CssClass="text-danger" />
                                            </div>
                                        </td>

                                        <!-- Email -->
                                        <td>
                                            <div class="form-group">
                                                <asp:TextBox ID="txtEditEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="form-control" TextMode="Email" />
                                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEditEmail"
                                                    ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Enter a valid email" ForeColor="Red" Display="Dynamic" CssClass="text-danger" />
                                            </div>
                                        </td>

                                        <!-- Address -->
                                        <td>
                                            <div class="form-group">
                                                <asp:TextBox ID="txtStreet" runat="server" Text='<%# Bind("Addrs_Street") %>' CssClass="form-control" Placeholder="Street" />
                                                <asp:RequiredFieldValidator ID="rfvStreet" runat="server" ControlToValidate="txtStreet"
                                                    ErrorMessage="Street is required" ForeColor="Red" Display="Dynamic" CssClass="text-danger" />
                                            </div>
                                            <div class="form-group mt-2">
                                                <asp:TextBox ID="txtCity" runat="server" Text='<%# Bind("Addrs_City") %>' CssClass="form-control" Placeholder="City" />
                                                <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity"
                                                    ErrorMessage="City is required" ForeColor="Red" Display="Dynamic" CssClass="text-danger" />
                                            </div>
                                            <div class="form-group mt-2">
                                                <asp:TextBox ID="txtState" runat="server" Text='<%# Bind("Addrs_State") %>' CssClass="form-control" Placeholder="State" />

                                                <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="txtState"
                                                    ErrorMessage="State is required" ForeColor="Red" Display="Dynamic" CssClass="text-danger" />

                                                <asp:RegularExpressionValidator ID="revState" runat="server" ControlToValidate="txtState"
                                                    ValidationExpression="^[A-Z]{2}$" ErrorMessage="Enter a valid 2-letter state code (e.g., WV)"
                                                    ForeColor="Red" Display="Dynamic" CssClass="text-danger" />
                                            </div>
                                            <div class="form-group mt-2">
                                                <asp:TextBox ID="txtZip" runat="server" Text='<%# Bind("Addrs_Zipcode") %>' CssClass="form-control" Placeholder="Zipcode" />

                                                <asp:RequiredFieldValidator ID="rfvZip" runat="server" ControlToValidate="txtZip"
                                                    ErrorMessage="Zipcode is required" ForeColor="Red" Display="Dynamic" CssClass="text-danger" />

                                                <asp:RegularExpressionValidator ID="revZip" runat="server" ControlToValidate="txtZip"
                                                    ValidationExpression="^\d{5}$" ErrorMessage="Enter a valid 5-digit ZIP code"
                                                    ForeColor="Red" Display="Dynamic" CssClass="text-danger" />
                                            </div>
                                        </td>

                                        <!-- Action Buttons -->
                                        <td>
                                            <asp:LinkButton runat="server" CommandName="Update" Text="Save" CssClass="btn btn-sm btn-success" />
                                            <asp:LinkButton runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn btn-sm btn-secondary" />
                                        </td>
                                    </tr>
                                </EditItemTemplate>

                            </asp:ListView>

                            <h5>Insurance</h5>
                            <asp:ListView ID="Ins_Info" DataKeyNames="InsuranceID" runat="server"
                                OnItemEditing="Ins_Info_ItemEditing"
                                OnItemUpdating="Ins_Info_ItemUpdating"
                                OnItemCanceling="Ins_Info_ItemCanceling">
                                <LayoutTemplate>
                                    <table style="border: solid 2px #336699;" cellspacing="0" cellpadding="3" rules="all">
                                        <tr style="background-color: #336699; color: White;">
                                            <th>Name</th>
                                            <th>Cardholder ID</th>
                                            <th>RxBIN</th>
                                            <th>RxGRP</th>
                                            <th>RxPCN</th>
                                            <th>Classification</th>
                                            <th>Type</th>
                                            <th>Actions</th>
                                            <!-- New column for Edit -->
                                        </tr>
                                        <tbody>
                                            <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                                        </tbody>
                                    </table>
                                </LayoutTemplate>

                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Name") %></td>
                                        <td><%# Eval("CardholderID") %></td>
                                        <td><%# Eval("RxBIN") %></td>
                                        <td><%# Eval("RxGrp") %></td>
                                        <td><%# Eval("RxPCN") %></td>
                                        <td><%# Eval("InsuranceClassification") %></td>
                                        <td><%# Eval("InsType") %></td>
                                        <td>
                                            <asp:LinkButton runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-sm btn-primary" />
                                        </td>
                                    </tr>
                                </ItemTemplate>

                                <EditItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtEditName" runat="server" Text='<%# Bind("Name") %>' CssClass="form-control" />
                                        </td>

                                        <td>
                                            <asp:TextBox ID="txtEditCardholderID" runat="server" Text='<%# Bind("CardholderID") %>' CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="rfvCardholderID" runat="server" ControlToValidate="txtEditCardholderID"
                                                ErrorMessage="Cardholder ID is required" Display="Dynamic" ForeColor="Red" CssClass="text-danger" />
                                        </td>

                                        <td>
                                            <asp:TextBox ID="txtEditRxBIN" runat="server" Text='<%# Bind("RxBIN") %>' CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="rfvRxBIN" runat="server" ControlToValidate="txtEditRxBIN"
                                                ErrorMessage="RxBIN is required" Display="Dynamic" ForeColor="Red" CssClass="text-danger" />
                                            <asp:RegularExpressionValidator ID="revRxBIN" runat="server" ControlToValidate="txtEditRxBIN"
                                                ValidationExpression="^\d{6,}$" ErrorMessage="RxBIN must be at least 6 digits" Display="Dynamic" ForeColor="Red" CssClass="text-danger" />
                                        </td>

                                        <td>
                                            <asp:TextBox ID="txtEditRxGrp" runat="server" Text='<%# Bind("RxGrp") %>' CssClass="form-control" />
                                        </td>

                                        <td>
                                            <asp:TextBox ID="txtEditRxPCN" runat="server" Text='<%# Bind("RxPCN") %>' CssClass="form-control" />
                                        </td>

                                        <td>
                                            <asp:TextBox ID="txtEditClassification" runat="server" Text='<%# Bind("InsuranceClassification") %>' CssClass="form-control" />
                                        </td>

                                        <td>
                                            <asp:TextBox ID="txtEditInsType" runat="server" Text='<%# Bind("InsType") %>' CssClass="form-control" />
                                        </td>

                                        <td>
                                            <asp:LinkButton runat="server" CommandName="Update" Text="Save" CssClass="btn btn-sm btn-success" />
                                            <asp:LinkButton runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn btn-sm btn-secondary" />
                                        </td>
                                    </tr>
                                </EditItemTemplate>

                            </asp:ListView>

                            <h5>Prescription information</h5>
                            <asp:ListView ID="Rx_Info" runat="server" DataKeyNames="PrescriptionID">
                                <LayoutTemplate>
                                    <table style="border: solid 2px #336699;" cellspacing="0" cellpadding="3" rules="all">
                                        <tr style="background-color: #336699; color: White;">
                                            <th>Name</th>
                                            <th>Dose</th>
                                            <th>Supply</th>
                                            <th>Quantity</th>
                                            <th>Doctor</th>
                                            <th>Doctor Number</th>
                                            <th>Doctor Address</th>
                                        </tr>
                                        <tbody>
                                            <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                                        </tbody>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Prescription_Name") %></td>
                                        <td><%# Eval("Prescription_Dose") %></td>
                                        <td><%# Eval("Prescription_DaySupply") %></td>
                                        <td><%# Eval("Prescription_Quantity") %></td>
                                        <td><%# Eval("Doctor_Name") %></td>
                                        <td><%# Eval("Doctor_Phone") %></td>
                                        <td><%# Eval("Doctor_Address") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                        <div runat="server" class="AgentElement">
                            <h5>Flag</h5>
                            <asp:Button ID="btnCreateFlag" runat="server" Text="Create Flag" OnClick="btnCreateFlag_Click" CssClass="btn btn-primary" />

                            <!-- Flag List -->
                            <asp:ListView ID="Flag_info" runat="server">
                                <LayoutTemplate>
                                    <table style="border: solid 2px #336699;" cellspacing="0" cellpadding="3" rules="all">
                                        <tr style="background-color: #336699; color: White;">
                                            <th>Flag Status</th>
                                            <th>Due Date</th>
                                            <th>Flag Note</th>
                                            <th>Actions</th>
                                            <!-- Added actions column for the buttons -->
                                        </tr>
                                        <tbody>
                                            <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                                        </tbody>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Flag_status") %></td>
                                        <td><%# Eval("Flag_DueDate") %></td>
                                        <td><%# Eval("Flag_note") %></td>
                                        <td>
                                            <!-- Complete Flag Button -->
                                            <asp:Button ID="btnCompleteFlag" runat="server" Text="Complete Flag" CommandName="CompleteFlag" CommandArgument='<%# Eval("FlagID") %>' OnCommand="FlagCommand" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>

                            <!-- Modal for Creating Flag -->
                            <div id="flagModal" style="display: none;">
                                <div class="modal-content">
                                    <h3>Create Flag</h3>
                                    <label for="txtFlagDate">Due Date:</label>
                                    <input type="date" id="txtFlagDate" runat="server" class="form-control" />
                                    <label for="txtFlagNote">Flag Note:</label>
                                    <textarea id="txtFlagNote" runat="server" class="form-control"></textarea>
                                    <br />
                                    <asp:Button ID="btnSubmitFlag" runat="server" Text="Submit Flag" OnClick="btnSubmitFlag_Click" CssClass="btn btn-success" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClientClick="cancelFlag(); return false;" CssClass="btn btn-danger" />
                                </div>
                            </div>
                        </div>

                    </div>
                    <div runat="server" class="AgentElement" style="float: right; width: 20%;">
                        <h5>Logs</h5>
                        <asp:ListView ID="Log_Info" runat="server">
                            <ItemTemplate>
                                <div style="border-bottom: 1px solid #ccc; padding: 5px 0;">
                                    <%# Eval("Date") %> <%# Eval("AgentID") %><br />
                                    <%# Eval("Notes") %>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </div>

            </div>
        </div>
    </form>
</body>
</html>
