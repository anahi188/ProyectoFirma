<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="archivo.aspx.cs" Inherits="InterfazIT.archivo" %>

<%@ Register assembly="DevExpress.Web.Bootstrap.v21.1, Version=21.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Firma Electrónica</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }

        .container {
            background: white;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.2);
            width: 400px;
            text-align: left;
        }

        .title {
            color: #333;
            font-size: 24px;
            text-align: center;
            margin-bottom: 20px;
        }

        .logo {
            display: block;
            margin: 0 auto 10px;
            width: 130px;
        }

        h4, label {
            color: #555;
            font-size: 16px;
            margin-top: 10px;
            display: block;
        }

        .upload, select, input[type="text"], input[type="password"] {
            width: 100%;
            padding: 8px;
            margin-top: 5px;
            border: 1px solid #ccc;
            border-radius: 5px;
            box-sizing: border-box;
        }

        .btn {
            background-color: #007bff;
            color: white;
            border: none;
            padding: 10px;
            width: 100%;
            border-radius: 5px;
            cursor: pointer;
            margin-top: 10px;
            font-size: 16px;
        }

        .btn:hover {
            background-color: #0056b3;
        }

        .error-message {
            color: red;
            font-size: 14px;
            margin-top: 5px;
            display: none;
        }
    </style>

    <script>
        function validarFormulario(event) {
            let valid = true;

            let pdf = document.getElementById("FileUpload1");
            let certificado = document.getElementById("FileUpload2");
            let razon = document.getElementById("txtRazon");
            let password = document.getElementById("txtPassword");

            let errorPdf = document.getElementById("errorPdf");
            let errorCertificado = document.getElementById("errorCertificado");
            let errorRazon = document.getElementById("errorRazon");
            let errorPassword = document.getElementById("errorPassword");

            errorPdf.style.display = "none";
            errorCertificado.style.display = "none";
            errorRazon.style.display = "none";
            errorPassword.style.display = "none";

            if (!pdf.value) {
                errorPdf.style.display = "block";
                valid = false;
            }
            if (!certificado.value) {
                errorCertificado.style.display = "block";
                valid = false;
            }
            if (razon.value.trim() === "") {
                errorRazon.style.display = "block";
                valid = false;
            }
            if (password.value.trim() === "") {
                errorPassword.style.display = "block";
                valid = false;
            }

            if (!valid) {
                event.preventDefault();
            }
        }

        function ocultarError(id) {
            document.getElementById(id).style.display = "none";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" onsubmit="validarFormulario(event)">
        <div class="container">
            <img src="logoIT.png" alt="Logo" class="logo" /> 
            <h1 class="title">Firma Electrónica</h1>

            <label for="FileUpload1">Seleccione su PDF:</label>
            <asp:FileUpload id="FileUpload1" runat="server" CssClass="upload" onchange="ocultarError('errorPdf')" />
            <div id="errorPdf" class="error-message">Este campo es obligatorio.</div>

            <label for="FileUpload2">Seleccione su certificado p12:</label>
            <asp:FileUpload id="FileUpload2" runat="server" CssClass="upload" onchange="ocultarError('errorCertificado')" />
            <div id="errorCertificado" class="error-message">Este campo es obligatorio.</div>

            <asp:Label ID="lblData" runat="server" Text="Ingrese los siguientes datos:" CssClass="label" />

            <label for="CmbLocacion">Seleccione su locación actual:</label>
            <asp:DropDownList ID="CmbLocacion" runat="server" CssClass="upload">
                <asp:ListItem>Ecuador</asp:ListItem>
            </asp:DropDownList>

            <label for="txtRazon">Ingrese la razón de su firma:</label>
            <asp:TextBox ID="txtRazon" runat="server" CssClass="upload" oninput="ocultarError('errorRazon')" />
            <div id="errorRazon" class="error-message">Este campo es obligatorio.</div>

            <label for="txtPassword">Ingrese la contraseña del certificado:</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="upload" placeholder="Ingrese la contraseña" onkeyup="ocultarError('errorPassword')" />
            <div id="errorPassword" class="error-message">Este campo es obligatorio.</div>

            <asp:Label id="PassStatus" runat="server" CssClass="label" Style="color: red;" />

            <asp:Button ID="btnPDF" Text="Subir Firma" OnClick="UploadButton_Click" runat="server" CssClass="btn"/>  

            <asp:Label id="UploadStatusLabel" runat="server" CssClass="label" Style="color: red;" />

        </div>
    </form>
</body>
</html>




