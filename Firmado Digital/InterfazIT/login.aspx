<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="InterfazIT.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <style>
         .color-span{
            color:red;
        }

        .hidden {
            opacity: 0;
            display: none;
        }
    </style>
    <form id="form1" runat="server">
        <div>
            <h2>Bienvenido</h2>
            <asp:Label id ="lblUser" runat="server" text="Usuario"></asp:Label>
            <asp:TextBox id ="txtUser" runat="server" ></asp:TextBox>
            <span id="notificationUser" class="hidden color-span">Por favor, rellene este campo.</span>
            <br/>
            <asp:Label ID="lblKey" runat="server" text="Contraseña"></asp:Label>
            <asp:TextBox ID="txtKey" runat="server" Type="password"></asp:TextBox>
            <span id="notificationKey" class="hidden color-span">Por favor, rellene este campo.</span>
            <br />
            <asp:Button ID="btnIniciar" runat="server" Text="Ingresar" CssClass="btn-primary" Onclick="btnIniciar_Click" />
            <br />
        </div>
    </form>
</body>
</html>
