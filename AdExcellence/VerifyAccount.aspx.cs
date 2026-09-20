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


namespace AdExcellence
{
    public partial class WebForm5 : System.Web.UI.Page
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
            try
            {
                if (!Session["LoginId"].ToString().Equals(""))
                {
                    if (!(Page.IsPostBack == true))
                    {
                        connection();
                        try
                        {
                            SqlCommand pwdCmd = new SqlCommand("select Password from Login where Email=@email", cn);
                            pwdCmd.Parameters.AddWithValue("@email", Session["LoginId"]);
                            da = new SqlDataAdapter(pwdCmd);
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
                    else
                    {

                    }
                }

                else
                {
                    Response.Redirect("404.aspx");
                }
            }
            catch (Exception)
            {
                Response.Redirect("404.aspx");
            }

            try
            {
                if (!(Page.IsPostBack == true))
                {


                    da = new SqlDataAdapter("select * from Login where type='howner' and status='no'", cn);
                    dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        ddlemail.Items.Clear();
                        ddlemail.Items.Add("Select");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ddlemail.Items.Add(dt.Rows[i]["Email"].ToString());
                        }
                    }
                }
            }
            catch { }

        }
        public void clear()
        {
            txtname.Text = "";
            txtemail.Text = "";
            lbladdress.Text = "";
            lblcity.Text = "";
            lblland.Text = "";
            lblMobile.Text = "";
            lblsid.Text = "";
            lblstate.Text = "";
        }
        protected void ddlemail_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


                connection();
                clear();

                SqlCommand emailCmd = new SqlCommand("select * from Login where Email=@email and type='howner'", cn);
                emailCmd.Parameters.AddWithValue("@email", ddlemail.Text);
                da = new SqlDataAdapter(emailCmd);
                dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    lblsid.Text = dt.Rows[0]["Id"].ToString();
                    txtname.Text = dt.Rows[0]["name"].ToString();
                    txtemail.Text = dt.Rows[0]["Email"].ToString();
                    lbladdress.Text = dt.Rows[0]["address"].ToString();
                    lblcity.Text = dt.Rows[0]["city"].ToString();
                    lblstate.Text = dt.Rows[0]["state"].ToString();
                    lblland.Text = dt.Rows[0]["landmark"].ToString();
                    lblMobile.Text = dt.Rows[0]["mobile"].ToString();

                    Image1.ImageUrl = "~/images/aadhar/" + lblsid.Text + ".jpg";

                }





            }


            catch (Exception)
            {
                Response.Redirect("404.aspx");
            }

        }
        

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cn.Open();
                cmd.Connection = cn;

                cmd.CommandText = "update Login set status='yes' where Email=@email";
                cmd.Parameters.AddWithValue("@email", ddlemail.Text);
                cmd.ExecuteNonQuery();
                Response.Write("<script>alert('Verification Completed') </script>");
                cmd = null;

                clear();
                da = new SqlDataAdapter("select * from Login where type='howner' and status='no'", cn);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    ddlemail.Items.Clear();
                    ddlemail.Items.Add("Select");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddlemail.Items.Add(dt.Rows[i]["Email"].ToString());
                    }
                }

            }
            catch
            {
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {

            try
            {
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cn.Open();
                cmd.Connection = cn;

                cmd.CommandText = "delete from Login where Email=@email";
                cmd.Parameters.AddWithValue("@email", ddlemail.Text);
                cmd.ExecuteNonQuery();
                Response.Write("<script>alert('Verification Completed') </script>");
                cmd = null;

                clear();
                da = new SqlDataAdapter("select * from Login where type='howner' and status='no'", cn);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    ddlemail.Items.Clear();
                    ddlemail.Items.Add("Select");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddlemail.Items.Add(dt.Rows[i]["Email"].ToString());
                    }
                }

            }
            catch
            {
            }

        }
    }
}