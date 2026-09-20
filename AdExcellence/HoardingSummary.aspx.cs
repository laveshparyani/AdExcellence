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
using System.Data.SqlClient;

namespace AdExcellence
{
    public partial class WebForm19 : System.Web.UI.Page
    {
        String product_name = "";
        String table = "";
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
                if (Request.QueryString["pname"] == null)
                {
                    throw new ArgumentException("No parameter specified");
                }
                else
                {
                    product_name = Convert.ToString(Request.QueryString["pname"]);

                    try
                    {
                        SqlCommand prodCmd = new SqlCommand("select * from Hoarding where hid=@hid ", cn);
                        prodCmd.Parameters.AddWithValue("@hid", product_name);
                        da = new SqlDataAdapter(prodCmd);
                        dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            Session["proid"] = dt.Rows[0]["hid"].ToString();
                            table = table + "<table broder='1' style='color:black'>";//<tr><td>Hoarding ID:</td><td valign='top'>" + dt.Rows[0]["hid"].ToString() + "<br /></td></tr><tr><td>&nbsp;</td></tr>";

                            table = table + "<tr><td><b>Size:</b></td><td>" + dt.Rows[0]["size"].ToString() + "</td></tr><tr><td>&nbsp;</td></tr>";
                            table = table + "<tr><td valign='top'><b>Description: </b><br />&nbsp;</td><td>" + dt.Rows[0]["description"].ToString() + "</td></tr><tr><td>&nbsp;</td></tr>";
                            table = table + "<tr><td><b>Location: </b><br />&nbsp;</td><td>" + dt.Rows[0]["location"].ToString() + "</td></tr><tr><td>&nbsp;</td></tr>";
                            table = table + "<tr><td><b>Price: </b></td><td>" + dt.Rows[0]["cost"].ToString() + "</td></tr></table>";

                            lbltext.Text = table;

                            table = "<table><tr><td valign='top'><div id='dynamicimg1'><img class='h-[70vh] rounded-xl object-cover' src='" + "images/hoarding/" + dt.Rows[0]["hid"].ToString() + ".jpg" + "' '/></div></td></tr></table>";
                            lblimage.Text = table;
                            Session["cost"] = dt.Rows[0]["cost"].ToString();
                            table = "";
                            da = null;
                            dt = null;


                            for (int i = 1; i <= 6; i++)
                            {
                                ddlMonth.Items.Add(i.ToString());
                            }

                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
        private void autogenerate()
        {
            int a = 0;
            da = new SqlDataAdapter("select * from booking", cn);
            dt = new DataTable();
            da.Fill(dt);
            Session["count"] = dt.Rows.Count;
            if (a.Equals(Session["count"]))
            {
                Session["count"] = 1;
            }
            else
            {
                Session["count"] = (int)Session["count"] + 1;
            }

            da = null;
            dt = null;
        }

        protected void btnCart_Click1(object sender, EventArgs e)
        {
            // Session["count"] = "1";
            Session["month"] = ddlMonth.Text;
            connection();
            Session["amt"] = "0";

            try
            {
            }
            catch (Exception)
            {
            }

            // code for insert record.
            try
            {

                connection();


                //QUERYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY
                String ss = "select * from booking where hid=@proid and CONVERT(DATE, fromdate, 105)<=@today and CONVERT(DATE, todate, 105)>=@today and status='true'";



                SqlCommand chkCmd = new SqlCommand(ss, cn);
                chkCmd.Parameters.AddWithValue("@proid", Session["proid"].ToString());
                chkCmd.Parameters.AddWithValue("@today", DateTime.Now.ToString("yyyy-MM-dd"));
                da = new SqlDataAdapter(chkCmd);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Response.Write("<script>alert('Sorry!!!Hoarding Already Booked!!!!')</script>");
                }
                else
                {



                    autogenerate();
                    cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = cn;
                    String count = Session["count"].ToString();


                    try
                    {
                        cn.Open();
                    }
                    catch
                    {
                        cn.Close();
                        cn.Open();
                    }


                    double tc = (Convert.ToDouble(Session["cost"].ToString())) * (Convert.ToDouble(ddlMonth.Text));


                    cmd.CommandText = "insert into booking values(@bookid,@proid,@fromdate,@todate,@tc,@loginid,'false')";
                    cmd.Parameters.AddWithValue("@bookid", count);
                    cmd.Parameters.AddWithValue("@proid", Session["proid"].ToString());
                    cmd.Parameters.AddWithValue("@fromdate", DateTime.Now.Date.ToShortDateString());
                    cmd.Parameters.AddWithValue("@todate", DateTime.Now.Date.AddMonths(Convert.ToInt32(ddlMonth.Text)).ToShortDateString());
                    cmd.Parameters.AddWithValue("@tc", tc);
                    cmd.Parameters.AddWithValue("@loginid", Session["LoginId"].ToString());
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    Session["finalamount"] = tc + "";
                    Session["bookid"] = count + "";
                    Response.Redirect("Payment.aspx");





                }


            }
            catch (Exception)
            {
            }


        }

        protected void btnCart_Click(object sender, EventArgs e)
        {
            btnCart_Click1(sender, e);
        }
    }
}