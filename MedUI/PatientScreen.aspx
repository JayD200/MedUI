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
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div style="padding: 30px;">
            <div style="text-align: center; text-decoration: none; color: black;">
                <a href="Login.aspx">Back to Login</a>
            </div>
            <div id="PatientInfo" style="padding-top: 5px;">
                <div runat="server" class="AgentElement" style="display:none;">
                    <asp:Button ID="PatientList" runat="server" Text="All Patients" OnClick="viewAllPatients" />
                </div>
                <div id="PageInfo" style="width: auto; height: auto;">
                    <div id="contentDiv" style="float: left; width: 80%;">
                        <div>
                            <h5>Patient Information</h5>
                            <asp:ListView ID="Pt_Info" runat="server">
                                <LayoutTemplate>
                                    <table style="border: solid 2px #336699;" cellspacing="0" cellpadding="3" rules="all">
                                        <tr style="background-color: #336699; color: White;">
                                            <th>Name</th>
                                            <th>Phone Number</th>
                                            <th>Date of Birth</th>
                                            <th>Gender</th>
                                            <th>Email</th>
                                            <th>Address</th>
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
                                        <td><%# Eval("Addrs_Zipcode") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>

                            <h5>Insurance</h5>
                            <asp:ListView ID="Ins_Info" runat="server">
                                <LayoutTemplate>
                                    <table style="border: solid 2px #336699;" cellspacing="0" cellpadding="3" rules="all">
                                        <tr style="background-color: #336699; color: White;">
                                            <th>Name</th>
                                            <th>ID</th>
                                            <th>RxBIN</th>
                                            <th>RxGRP</th>
                                            <th>RxPCN</th>
                                            <th>Classification</th>
                                            <th>Type</th>
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
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>

                            <h5>Prescription information</h5>
                            <asp:ListView ID="Rx_Info" runat="server">
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
                        <div runat="server" class="AgentElement" style="display: none;">
                            <h5>Flag </h5>
                            <asp:ListView ID="Flag_info" runat="server">
                                <LayoutTemplate>
                                    <table style="border: solid 2px #336699;" cellspacing="0" cellpadding="3" rules="all">
                                        <tr style="background-color: #336699; color: White;">
                                            <th>Flag Status</th>
                                            <th>Due Date</th>
                                            <th>Flag Note</th>
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
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                    <div runat="server" class="AgentElement" style="display: none; float: right; width: 20%;">
                        <h5>Logs</h5>
                        <label id="Logs">Log info</label>
                    </div>
                </div>

            </div>
        </div>
    </form>
</body>
</html>
