<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PatientList.aspx.cs" Inherits="MedUI.PatientList" %>

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
            <div>
                <h4>Patient List</h4>
                <asp:ListView ID="ListView1" runat="server">
                    <LayoutTemplate>
                        <table style="border: solid 2px #336699;" cellspacing="0" cellpadding="3" rules="all">
                            <tr style="background-color: #336699; color: White;">
                                <th>View Profile</th>
                                <th>Prescription</th>
                                <th>Name</th>
                                <th>Doctor</th>
                                <th>Flag Status</th>
                            </tr>
                            <tbody>
                                <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                            </tbody>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Button ID="ViewProfile" runat="server" Text="View Patient" />
                            </td>
                            <td><%# Eval("Prescription_Name") %></td>
                            <td><%# Eval("F_Name" + "L_Name") %></td>
                            <td><%# Eval("Doctor_Name") %></td>
                            <td><%# Eval("Flag_status") %></td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td>
                                <asp:Button ID="ViewProfile" runat="server" Text="View Patient" />
                            </td>
                            <td><%# Eval("Prescription_Name") %></td>
                            <td><%# Eval("F_Name" + "L_Name") %></td>
                            <td><%# Eval("Doctor_Name") %></td>
                            <td><%# Eval("Flag_status") %></td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:ListView>
            </div>
        </div>
    </form>
</body>
</html>
