using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.Net;
using System.Net.Mail;

using System.Data.Sql;
using System.Data.SqlClient;

namespace AdExcellence
{
    public partial class WebForm1 : System.Web.UI.Page
    {
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
                clearAll();
                getdata();
                autogenerate();
                lblid.Text = Session["count"].ToString();

            }
            else
            {
            }

        }

        protected void getdata()
        {
            da = new SqlDataAdapter("select * from Login", cn);
            dt = new DataTable();
            da.Fill(dt);
        }

        private void autogenerate()
        {
            // Next id = highest existing numeric id + 1 (collision-safe even after deletes).
            try
            {
                if (cn.State != ConnectionState.Open) cn.Open();
                SqlCommand idCmd = new SqlCommand("select ISNULL(MAX(TRY_CAST(id AS INT)), 0) + 1 from Login", cn);
                Session["count"] = (int)idCmd.ExecuteScalar();
            }
            catch (Exception)
            {
                Session["count"] = 1;
            }
        }

        public void clearAll()
        {

            txtname.Text = "";
            txtemail.Text = "";
            txtadd.Text = "";
            txtcity.Text = "";
            txtState.Text = "";
            txtLandmark.Text = "";
            txtMobile.Text = "";



        }


        protected void btnSave_Click1(object sender, EventArgs e)
        {
            if (Page.IsValid == true)
            {
                //image

                //code for new record
                checkit();
                //send mail code
                clearAll();
            }

        }
        public void checkit()
        {
            if (FileUpload1.HasFile)
            {
                try
                {
                    string dir = Server.MapPath("~/images/aadhar/");
                    if (!System.IO.Directory.Exists(dir))
                        System.IO.Directory.CreateDirectory(dir);
                    FileUpload1.SaveAs(dir + lblid.Text + ".jpg");
                    Image1.ImageUrl = "~/images/aadhar/" + lblid.Text + ".jpg";
                }
                catch (Exception)
                {
                }
            }


            SqlCommand chkCmd = new SqlCommand("select * from Login where Email=@email", cn);
            chkCmd.Parameters.AddWithValue("@email", txtemail.Text);
            da = new SqlDataAdapter(chkCmd);
            dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cn.Open();
                cmd.Connection = cn;

                cmd.CommandText = "insert into Login values(@id,@name,@email,@mobile,@address,@city,@state,@landmark,@password,'howner','no')";
                cmd.Parameters.AddWithValue("@id", lblid.Text);
                cmd.Parameters.AddWithValue("@name", txtname.Text);
                cmd.Parameters.AddWithValue("@email", txtemail.Text);
                cmd.Parameters.AddWithValue("@mobile", txtMobile.Text);
                cmd.Parameters.AddWithValue("@address", txtadd.Text);
                cmd.Parameters.AddWithValue("@city", txtcity.Text);
                cmd.Parameters.AddWithValue("@state", txtState.Text);
                cmd.Parameters.AddWithValue("@landmark", txtLandmark.Text);
                cmd.Parameters.AddWithValue("@password", SecurityHelper.HashPassword(txtMobile.Text));
                cmd.ExecuteNonQuery();
                Response.Write("<script>alert('Verification Under Process') </script>");
                cmd = null;
                getdata();
                autogenerate();
                lblid.Text = Session["count"].ToString();
            }
            else
            {
                Response.Write("<script>alert('Email-Id Already Present') </script>");
                cmd = null;
            }

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clearAll();
        }
        protected void txtadd_TextChanged(object sender, EventArgs e)
        {

        }
    }
}