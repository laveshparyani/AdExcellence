using System;
using System.Collections;
using System.Configuration;
using System.Data;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.Sql;
using System.Data.SqlClient;

namespace AdExcellence
{
    public partial class WebForm14 : System.Web.UI.Page
    {
        String passwd;
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        SqlCommand cmd = new SqlCommand();

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        public void connection()
        {
            try
            {
                cn.Close();
                cn.Open();
            }
            catch (Exception)
            {
                Response.Write("<script>alert('An error occurred. Please try again.')</script>");

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Page.IsPostBack == true))
            {
                connection();
                try
                {
                    SqlCommand selCmd = new SqlCommand("select Password from Login where Email=@email", cn);
                    selCmd.Parameters.AddWithValue("@email", Session["LoginId"]);
                    da = new SqlDataAdapter(selCmd);
                    dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        Session["pwd"] = dt.Rows[0]["Password"].ToString();
                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
            }
        }


        protected void CVPass_ServerValidate(object source, ServerValidateEventArgs args)
        {
            {
                string str = txtNewPass.Text;
                if (str.Length < 8)
                {
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }

        }
        protected void ImgChangePass_Click(object sender, EventArgs e)
        {
            if (Page.IsValid == true)
            {
                passwd = txtOldPass.Text;
                if (SecurityHelper.VerifyPassword(passwd, Session["pwd"].ToString()))
                {

                    cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandText = "update Login set Password=@password where Email=@email";
                    cmd.Parameters.AddWithValue("@password", SecurityHelper.HashPassword(txtNewPass.Text));
                    cmd.Parameters.AddWithValue("@email", Session["LoginId"]);
                    cmd.ExecuteNonQuery();
                    Response.Write("<script>alert('Password changed successfully') </script>");
                    cmd = null;

                }
                else
                {
                    Response.Write("<script>alert('Incorrect old password') </script>");
                    txtOldPass.Text = "";
                    txtOldPass.Focus();
                }

            }

        }

    }
}