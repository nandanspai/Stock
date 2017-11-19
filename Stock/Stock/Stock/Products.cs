using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stock
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            cmb_Status.SelectedIndex = 0;
            SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Stock;Integrated Security=True");
            loadProductList(con);
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            bool cmb_status_bool = false;
            var sqlQuery = "";

            try
            {
                if (cmb_Status.SelectedIndex == 0)
                {
                    cmb_status_bool = true;
                }
                else
                {
                    cmb_status_bool = false;
                }
                SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Stock;Integrated Security=True");

                if (DoesProductExists(con, txt_ProductCode.Text) == false)
                {
                    sqlQuery = @"Insert into Products (ProductCode, ProductName, ProductStatus)
                                    values ('" + txt_ProductCode.Text + "','" + txt_ProductName.Text + "','" + cmb_status_bool + "')";
                }
                else
                {
                    sqlQuery = @"Update Products
                                set ProductName = '" + txt_ProductName.Text + "', ProductStatus = '" + cmb_status_bool + "' where ProductCode='" + Convert.ToInt32(txt_ProductCode.Text) + "'";
                }
                SqlCommand scmd = new SqlCommand(sqlQuery, con);
                con.Open();
                scmd.ExecuteNonQuery();
                con.Close();
                loadProductList(con);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadProductList(SqlConnection scon)
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select * from Products", scon);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            ProductList.Rows.Clear();

            foreach (DataRow dr in dt.Rows)
            {
                int n = ProductList.Rows.Add(); // Add rows in GridView
                ProductList.Rows[n].Cells[0].Value = dr["ProductCode"].ToString();
                ProductList.Rows[n].Cells[1].Value = dr["ProductName"].ToString();
                ProductList.Rows[n].Cells[2].Value = (((bool)dr["ProductStatus"] == true) ? "Active" : "Inactive");
            }
        }

        private void ProductList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txt_ProductCode.Text = ProductList.SelectedRows[0].Cells[0].Value.ToString();
            txt_ProductName.Text = ProductList.SelectedRows[0].Cells[1].Value.ToString();

            if (ProductList.SelectedRows[0].Cells[2].Value.ToString() == "Active")
            {
                cmb_Status.SelectedIndex = 0;
            }
            else
            {
                cmb_Status.SelectedIndex = 1;
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            string selectedProductCode = txt_ProductCode.Text.ToString();
            SqlConnection scon = new SqlConnection("Data Source=.;Initial Catalog=Stock;Integrated Security=True");
            SqlCommand scom = new SqlCommand("Delete from Products where ProductCode = '" + selectedProductCode + "'", scon);

            try
            {
                if (selectedProductCode != "")
                {
                    scon.Open();
                    scom.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                scon.Close();
                loadProductList(scon);
            }
        }

        private bool DoesProductExists(SqlConnection con, string ProductCode)
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select * from Products where ProductCode = '" + ProductCode + "'", con);

            DataTable dt = new DataTable();
            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}