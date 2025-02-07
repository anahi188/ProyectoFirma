using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Security;
using System.Windows.Forms;

namespace InterfazIT
{
    public partial class login : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection("Data Source = .; Integrated Security=true; Initial Catalog = XamarinIT");
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnIniciar_Click(object sender, EventArgs e)
        {
        con.Open();
        SqlCommand cmd = new SqlCommand("select * from cliente where nombre = '" + txtUser.Text.Trim() + "' and contrasenia = '" + txtKey.Text.Trim() + "'", con);
        SqlDataReader dr = cmd.ExecuteReader();

        if (is_validate())
            {   
            MessageBox.Show("Datos Agregados Correctamente", "Validaciones", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (dr.HasRows)
            {
                Response.Redirect("archivo.aspx");

            } else 
                {
                    Response.Write("<script>alert('Credenciales Incorretas'); </script>");
                    txtUser.Text = "";
                    txtKey.Text = "";
                }
            }
        }
        private bool is_validate()
        {
            bool no_error = true;
            if (txtUser.Text == string.Empty || txtKey.Text == string.Empty)
            {
                Response.Write("<script>alert('Ingrese todas las credenciales'); </script>");
                no_error = false;
            }
            return no_error;
        }
    }
}