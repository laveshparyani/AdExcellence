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
using System.Drawing;
using System.IO;

using System.Data.Sql;
using System.Data.SqlClient;

namespace AdExcellence
{
    public partial class WebForm15 : System.Web.UI.Page
    {
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        SqlCommand cmd = new SqlCommand();
        String i;

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
                fillLocation();
                clearAll();

                disable();

                btnNew.Enabled = true;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                btnRemove.Enabled = false;

                getdata();
                if (dt.Rows.Count > 0)
                {
                    gridfill();
                }

                

            }
            else
            {
            }
        }

        protected void getdata()
        {
            da = new SqlDataAdapter("select * from Hoarding", cn);
            dt = new DataTable();
            da.Fill(dt);
        }
        public void fillLocation()
        {
            try
            {

                connection();
                da = new SqlDataAdapter("select distinct pincode from Location",cn);
                dt = new DataTable();
                da.Fill(dt);
                if(dt.Rows.Count > 0)
                {
                    DdlLocation.Items.Clear();
                    for (int i = 0;i < dt.Rows.Count; i++)
                    {
                        DdlLocation.Items.Add(dt.Rows[i][0].ToString());

                    }

                }

            }
            catch { }
        }

        private void autogenerate()
        {
            try
            {
                cn.Open();
                SqlCommand sq = new SqlCommand("select MAX(hid) from Hoarding", cn);
                SqlDataReader sd = sq.ExecuteReader();
                sd.Read();
                Session["count"] = sd[0];
                Session["count"] = (int)Session["count"] + 1;
            }
            catch { cn.Close(); cn.Open(); Session["count"] = "1"; }


            //int a = 0;
            //getdata();
            //Session["count"] = dt.Rows.Count;
            //if (a.Equals(Session["count"]))
            //{
            //    Session["count"] = 1;
            //}
            //else
            //{
            //    Session["count"] = (int)Session["count"] + 1;
            //}

            //da = null;
            //dt = null;
        }

        public void clearAll()
        {
            lblid.Text = "";
            txtcost.Text = "";
            DdlLocation.SelectedIndex = 0;
            txtsize.Text = "";
            txtsummary.Text = "";
            Image1.ImageUrl = "~/images/hoarding/NAvail.jpg";
        }
        public void disable()
        {
            txtsize.Enabled = false;
            txtcost.Enabled = false;
            DdlLocation.Enabled = false;
            txtsummary.Enabled = false;
            
            
        }
        public void enable()
        {
            txtsize.Enabled = true;
            txtcost.Enabled = true;
            DdlLocation.Enabled = true;
            txtsummary.Enabled = true;

            txtsummary.Enabled = true;
        }
        protected void display()
        {
            lblid.Text = dt.Rows[0]["hid"].ToString();
            txtcost.Text = dt.Rows[0]["cost"].ToString();
            DdlLocation.Text = dt.Rows[0]["location"].ToString();
            txtsummary.Text = dt.Rows[0]["description"].ToString();
            txtsize.Text = dt.Rows[0]["size"].ToString();
           

            //image
            string image;
            image = FileUpload1.FileName;
            Image1.ImageUrl = "~/images/hoarding/" + lblid.Text + ".jpg";
        }

        public void gridfill()
        {
            SqlCommand gridCmd = new SqlCommand("select hid[Id],size[Size],location[Location],cost[Price] from Hoarding where emailid=@email order by hid desc", cn);
            gridCmd.Parameters.AddWithValue("@email", Session["LoginId"].ToString());
            da = new SqlDataAdapter(gridCmd);
            dt = new DataTable();
            da.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridView1.SelectedRow;
            i = GridView1.SelectedRow.Cells[1].Text;

            try
            {
                SqlCommand selCmd = new SqlCommand("select * from Hoarding where hid=@hid ", cn);
                selCmd.Parameters.AddWithValue("@hid", i);
                da = new SqlDataAdapter(selCmd);
                dt = new DataTable();
                da.Fill(dt);
                display();
                btnNew.Enabled = false;
                
                btnSave.Enabled = false;
                btnRemove.Enabled = true;
                btnCancel.Enabled = true;
            }
            catch (Exception)
            {

            }
        }

        protected void btnNew_Click1(object sender, EventArgs e)
        {
            enable();
            RequiredFieldValidator3.Enabled = true;
            Session["save_code"] = "true";
            autogenerate();
            lblid.Text = Session["count"].ToString();
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;

            btnRemove.Enabled = false;

        }

        protected void btnSave_Click1(object sender, EventArgs e)
        {
            if (Page.IsValid == true)
            {
                String t = "true";
                //image


                if (t.Equals(Session["save_code"].ToString()))
                {
                    if (FileUpload1.HasFile)
                    {
                        try
                        {
                            string dir = Server.MapPath("~/images/hoarding/");
                            if (!System.IO.Directory.Exists(dir))
                                System.IO.Directory.CreateDirectory(dir);
                            FileUpload1.SaveAs(dir + lblid.Text + ".jpg");
                            Image1.ImageUrl = "~/images/hoarding/" + lblid.Text + ".jpg";
                        }
                        catch (Exception)
                        {
                        }
                    }
                    //code for new record
                    cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandText = "insert into Hoarding values(@hid,@size,@location,@cost,@description,@emailid)";
                    cmd.Parameters.AddWithValue("@hid", Convert.ToInt32(Session["count"].ToString()));
                    cmd.Parameters.AddWithValue("@size", txtsize.Text);
                    cmd.Parameters.AddWithValue("@location", DdlLocation.Text);
                    cmd.Parameters.AddWithValue("@cost", txtcost.Text);
                    cmd.Parameters.AddWithValue("@description", txtsummary.Text);
                    cmd.Parameters.AddWithValue("@emailid", Session["loginId"].ToString());
                    cmd.ExecuteNonQuery();
                    Response.Write("<script>alert('Saved Successfully') </script>");
                    cmd = null;

                    clearAll();
                    disable();
                    gridfill();
                    btnNew.Enabled = true;

                    btnSave.Enabled = false;
                    btnRemove.Enabled = false;
                    btnCancel.Enabled = false;
                }
                else
                {

                }
            }

        }

        protected void btnRemove_Click1(object sender, EventArgs e)
        {
            if (lblid.Text != "")
            {
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "Delete from Hoarding where hid=@hid ";
                cmd.Parameters.AddWithValue("@hid", lblid.Text);
                cmd.ExecuteNonQuery();
                Response.Write("<script>alert('Removed Successfully') </script>");
                cmd = null;

                clearAll();
                disable();
                gridfill();
                btnNew.Enabled = true;

                btnSave.Enabled = false;
                btnRemove.Enabled = false;
                btnCancel.Enabled = false;
            }

        }

        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            disable();
            clearAll();
            Session["save_code"] = "";
            btnNew.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnRemove.Enabled = false;
        }
    }

}