<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MedUI.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style>
        body {
            margin: 0;
            padding: 0;
        }

        .center {
            display: grid;
            margin: auto;
            width: 25%;
            height: 100%;
            border: 3px solid green;
            padding: 10px;
        }

            .center div {
                padding: 5px;
            }
    </style>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="padding: 30px;">
            <div align="right">
                <asp:Button ID="viewType" runat="server" Text="Agent View" OnClick="viewButton" align="right" />
            </div>
            <div class="center">
                <div>
                    <asp:Label ID="LoginText" runat="server" Text="Patient Login"></asp:Label>
                </div>
                <div>
                    <asp:TextBox ID="FirstName" runat="server" placeholder="First Name" Visible="true" />
                    <asp:TextBox ID="Email" runat="server" placeholder="Agent Email" Visible="false" />
                </div>
                <div id="LastNameDiv" runat="server" Visible="true" >
                    <asp:TextBox ID="LastName" runat="server" placeholder="Last Name" />
                </div>

                <div style="float:left">
                    <a style="color:red; float: left;"><asp:Label ID="LoginError" runat="server" Text="Test" /> </a>
                </div>
                <div style="float:right">
                    <asp:Button ID="SignIn" runat="server" Text="Login" OnClick="LoginButton" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
