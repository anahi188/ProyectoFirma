using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Pdf;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using DevExpress.BarCodes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Security.Cryptography;

namespace InterfazIT
{
    public partial class archivo : System.Web.UI.Page
    {
        protected void Page_load(object sender, EventArgs e)
        {
            // Evento de cambio de texto en la contraseña para borrar la alerta
            txtPassword.Attributes.Add("oninput", "document.getElementById('PassStatus').innerText = '';");
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            //Especificar la ruta del servidor donde se guardaran los archivos.
            //cambiar ruta 
            String savePath = @"C:\Users\Usuario\Documents\RecursosFirma\";

            // Antes de iniciar validaremos que los FileUpload contengan datos.
            if (FileUpload1.HasFile && FileUpload2.HasFile)
            {
                
                string locacion = CmbLocacion.SelectedValue;
                string razon = txtRazon.Text;

                // Obtención de los archivos a subir
                string filePDF = Server.HtmlEncode(FileUpload1.FileName);
                string fileFirma = Server.HtmlEncode(FileUpload2.FileName);

                // Obtenemos la extension del archivo a subir.
                string extensionPDF = System.IO.Path.GetExtension(filePDF);
                string extensionFirma = System.IO.Path.GetExtension(fileFirma);

                if (extensionPDF == ".pdf" && extensionFirma == ".p12")
                {
                    // Guardar los archivos en el servidor
                    String savePathPDF = savePath + filePDF;
                    String savePathFirma = savePath + fileFirma;

                    FileUpload1.SaveAs(savePathPDF);
                    FileUpload2.SaveAs(savePathFirma);

                    // Validar la contraseña del certificado
                    X509Certificate2 certificate = null;
                    bool passwordCorrecta = false;

                    while (!passwordCorrecta)
                    {
                        try
                        {
                            string password = txtPassword.Text; // Obtener contraseña del usuario
                            certificate = new X509Certificate2(savePathFirma, password);
                            passwordCorrecta = true; // Si no lanza excepción, la contraseña es correcta
                        }
                        catch (CryptographicException)
                        {
                            PassStatus.Text = "⚠️ Contraseña incorrecta. Inténtelo nuevamente.";
                            txtPassword.Text = ""; // Borra solo el campo de contraseña
                            return; // Sale del evento sin continuar el proceso
                        }
                    }

                    // Crear una instancia de PdfDocumentProcessor
                    DevExpress.Pdf.PdfDocumentProcessor documentProcessor = new DevExpress.Pdf.PdfDocumentProcessor();

                    // Cargar el documento PDF para firmar
                    documentProcessor.LoadDocument(savePathPDF);

                    string nombre = certificate.FriendlyName;
                    var fechaCaducidad = Convert.ToDateTime(certificate.GetExpirationDateString());
                    var fechaActual = Convert.ToDateTime(DateTime.Now);
                    var rest = fechaCaducidad - fechaActual;
                    var restDays = rest.ToString("dd");

                    // Generar código QR
                    BarCode barCode = new BarCode();
                    barCode.Symbology = Symbology.QRCode;
                    barCode.CodeText = nombre + "\n" + "Días de vigencia: " + restDays + "\n" + "www.firmadigital.gob.ec";
                    barCode.BackColor = Color.White;
                    barCode.ForeColor = Color.Black;
                    barCode.RotationAngle = 0;
                    barCode.CodeBinaryData = Encoding.Default.GetBytes(barCode.CodeText);
                    barCode.Options.QRCode.CompactionMode = QRCodeCompactionMode.Byte;
                    barCode.Options.QRCode.ErrorLevel = QRCodeErrorLevel.Q;
                    barCode.Options.QRCode.ShowCodeText = true;
                    barCode.CodeTextHorizontalAlignment = StringAlignment.Near;
                    barCode.DpiX = 90;
                    barCode.DpiY = 90;
                    barCode.Module = 3f;

                    String savePathImg = Path.Combine(savePath, "BarCodeImage.png");
                    barCode.Save(savePathImg, System.Drawing.Imaging.ImageFormat.Png);

                    // Firmar el documento
                    byte[] imageData = File.ReadAllBytes(savePathImg);
                    int pageNumber = 1;
                    PdfOrientedRectangle signatureBounds = new PdfOrientedRectangle(new PdfPoint(70, 90), 110, 80);
                    PdfSignature signature = new PdfSignature(certificate, imageData, pageNumber, signatureBounds);

                    signature.Location = locacion;
                    signature.Reason = razon;

                    // Obtener nombre original sin extensión
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePDF);
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                    // Guardar el documento firmado
                    string FinalPath = Path.Combine(savePath, $"{fileNameWithoutExtension}_Signed_{timestamp}.pdf");

                    documentProcessor.SaveDocument(FinalPath, new PdfSaveOptions() { Signature = signature });

                    // Eliminar el archivo original después de firmarlo
                    if (File.Exists(savePathPDF))
                    {
                        File.Delete(savePathPDF);
                    }

                    // Previsualización del documento firmado
                    FileStream fs = File.OpenRead(FinalPath);
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, (int)fs.Length);
                    fs.Close();

                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.BinaryWrite(data);
                    Response.End();
                }
                else
                {
                    UploadStatusLabel.Text = "Seleccione archivos válidos.";
                }

            }
            else
            {
                UploadStatusLabel.Text = "Seleccione los archivos necesarios.";
            }
        }
    }
}