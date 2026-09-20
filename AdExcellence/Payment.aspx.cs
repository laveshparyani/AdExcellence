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
    using System.Net.Mail;
    using System.Net;

    namespace AdExcellence
    {
        public partial class WebForm20 : System.Web.UI.Page
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataTable dt1 = new DataTable();
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
                // Guard against an expired or missing session (page opened directly or after timeout)
                if (Session["LoginId"] == null || Session["finalamount"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                if (!(Page.IsPostBack == true))
                {
                    connection();
                    Ddlyear.Items.Clear();
                    for (int i = 1; i <= 20; i++)
                    {
                        Ddlyear.Items.Add((Convert.ToInt16(DateTime.Today.Year - 1) + i).ToString());
                    }

                    // call auto generate for order ID
                    autogenerate();
                    lblorderid.Text = Session["count"].ToString();

                    lblcid.Text = Session["LoginId"].ToString();
                    lbltotalamt.Text = Session["finalamount"].ToString();

                    //code for customer id.

                    //code to display total amount
                }
            }

            private void autogenerate()
            {
                int a = 0;
                da = new SqlDataAdapter("select max(payid) from payment", cn);
                dt = new DataTable();
                da.Fill(dt);
                Session["count"] = dt.Rows[0][0].ToString();
                try
                {
                    if (a.Equals(Session["count"]))
                    {
                        Session["count"] = 1;
                    }
                    else
                    {
                        Session["count"] = Convert.ToInt32(Session["count"].ToString()) + 1;
                    }
                }
                catch
                {
                    Session["count"] = 1;
                }
                da = null;
                dt = null;
            }


        

        

        protected void btnbook1_Click(object sender, EventArgs e)
        {
            String month, year;
            month = DateTime.Now.Date.Month.ToString();
            year = DateTime.Now.Date.Year.ToString();

            if (((txtn1.Text.Length) + (txtn2.Text.Length) + (txtn3.Text.Length) + (txtn4.Text.Length)) < 16)
            {
                lblerror.Text = "Enter proper 16 digit Card Number.";

            }
            else
            {
                if (Convert.ToInt32(Ddlyear.Text) == Convert.ToInt32(year))
                {
                    if (Convert.ToInt32(Ddlmonth.Text) < Convert.ToInt32(month))
                    {
                        lblerror.Text = "Your card is Expired.";
                    }
                    else
                    {
                        lblerror.Text = "";
                    }
                }
                else
                {
                    lblerror.Text = "";
                }
            }
            if (lblerror.Text == "")
            {
                //code for save
                //String details = "";
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
                        String str = "insert into payment values(@payid,@bookid,@finalamount,@paydate,@mode,@cardno,@month,@year,@loginid)";

                        cmd = new SqlCommand(str, cn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@payid", Session["count"]);
                        cmd.Parameters.AddWithValue("@bookid", Session["bookid"]);
                        cmd.Parameters.AddWithValue("@finalamount", Session["finalamount"]);
                        cmd.Parameters.AddWithValue("@paydate", DateTime.Today.Date.ToShortDateString());
                        cmd.Parameters.AddWithValue("@mode", DdlMode.Text);
                        cmd.Parameters.AddWithValue("@cardno", txtn1.Text + txtn2.Text + txtn3.Text + txtn4.Text);
                        cmd.Parameters.AddWithValue("@month", Ddlmonth.Text);
                        cmd.Parameters.AddWithValue("@year", Ddlyear.Text);
                        cmd.Parameters.AddWithValue("@loginid", Session["LoginId"]);
                        try
                        {
                            cn.Close();
                            cn.Open();
                        }
                        catch (Exception) { }
                        cmd.Connection = cn;
                        cmd.ExecuteNonQuery();

                        str = "update Booking set status='true' where bookid = @bookid";

                        cmd = new SqlCommand(str, cn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@bookid", Session["bookid"]);
                        try
                        {
                            cn.Close();
                            cn.Open();
                        }
                        catch (Exception) { }
                        cmd.Connection = cn;
                        cmd.ExecuteNonQuery();

                        Response.Redirect("PaymentSuccessful.aspx");
                    }
                }
                catch (Exception)
                { }
            }
        }
    }
    }