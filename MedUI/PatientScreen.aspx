<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PatientScreen.aspx.cs" Inherits="MedUI.PatientScreen" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="padding: 30px;">
            <div style="text-align: center;">
                <asp:LinkButton ID="LoginReturn" runat="server" Style="text-decoration: none; color: black;">Back to Login</asp:LinkButton>
            </div>
            <div id="PatientInfo" style="padding-top: 5px;">
                <div>
                    <asp:Button ID="PatientList" runat="server" Text="All Patients" Visible="false" />
                </div>
                <div id="PageInfo" style="width: auto; height: auto;">
                    <div id="contentDiv" style="float: left; width: 80%;">
                        <h5>Patient Information</h5>
                        <asp:ListView ID="ListView1" runat="server">
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
                                    <td><%# Eval("F_Name" + "L_Name") %></td>
                                    <td><%# Eval("Phone") %></td>
                                    <td><%# Eval("DateOfBirth") %></td>
                                    <td><%# Eval("Gender") %></td>
                                    <td><%# Eval("Email") %></td>
                                    <td><%# Eval("Addrs_Zipcode") %></td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr>
                                    <td><%# Eval("F_Name" + "L_Name") %></td>
                                    <td><%# Eval("Phone") %></td>
                                    <td><%# Eval("DateOfBirth") %></td>
                                    <td><%# Eval("Gender") %></td>
                                    <td><%# Eval("Email") %></td>
                                    <td><%# Eval("Addrs_Zipcode") %></td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:ListView>

                        <h5>Insurance</h5>
                        <asp:ListView ID="ListView2" runat="server">
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
                                    <td><%# Eval("Insurance_Classification") %></td>
                                    <td><%# Eval("InsType") %></td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr>
                                    <td><%# Eval("Name") %></td>
                                    <td><%# Eval("CardholderID") %></td>
                                    <td><%# Eval("RxBIN") %></td>
                                    <td><%# Eval("RxGrp") %></td>
                                    <td><%# Eval("RxPCN") %></td>
                                    <td><%# Eval("Insurance_Classification") %></td>
                                    <td><%# Eval("InsType") %></td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:ListView>

                        <h5>Prescription information</h5>
                        <asp:ListView ID="ListView3" runat="server">
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
                            <AlternatingItemTemplate>
                                <tr>
                                    <td><%# Eval("Prescription_Name") %></td>
                                    <td><%# Eval("Prescription_Dose") %></td>
                                    <td><%# Eval("Prescription_DaySupply") %></td>
                                    <td><%# Eval("Prescription_Quantity") %></td>
                                    <td><%# Eval("Doctor_Name") %></td>
                                    <td><%# Eval("Doctor_Phone") %></td>
                                    <td><%# Eval("Doctor_Address") %></td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:ListView>

                        <h5>Flag <i>(default hidden)</i> </h5>
                        <asp:ListView ID="ListView4" runat="server">
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
                            <AlternatingItemTemplate>
                                <tr>
                                    <td><%# Eval("Flag_status") %></td>
                                    <td><%# Eval("Flag_DueDate") %></td>
                                    <td><%# Eval("Flag_note") %></td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:ListView>
                    </div>
                    <div id="logDiv" style="float: right; width: 20%;">
                        <h5>Logs</h5>
                        <label id="Logs">Log info</label>
                    </div>
                </div>

            </div>
        </div>
    </form>
</body>
</html>
