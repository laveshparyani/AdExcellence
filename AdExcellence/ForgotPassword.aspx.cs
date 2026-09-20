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

using System.Data.Sql;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace AdExcellence
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        String email;
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
            }
        }
        protected void btnSendMail_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (Page.IsValid == true)
            {
                try
                {

                    email = txtEmailID.Text;




                    SqlCommand selCmd = new SqlCommand("select * from Login where Email=@email and status='yes'", cn);
                    selCmd.Parameters.AddWithValue("@email", txtEmailID.Text);
                    da = new SqlDataAdapter(selCmd);
                    dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        Session["Email"] = dt.Rows[0]["Email"].ToString();
                        Session["epwd"] = dt.Rows[0]["password"].ToString();
                        if (txtEmailID.Text.Equals(Session["Email"].ToString()))
                        {

                            String pwd = "Your Password: " + Session["epwd"].ToString();


                            //Send password
                            lblpass.Text = pwd;




                        }
                        else
                            Response.Write("<script>alert('Email-Id Not Found')</script>");

                    }
                }
                catch (Exception)
                { }

            }


        }
    }
}