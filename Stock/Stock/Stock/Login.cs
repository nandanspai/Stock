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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            txt_UserName.Text = "";
            txt_Password.Clear();
            txt_UserName.Focus();
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Stock;Integrated Security=True");
                SqlDataAdapter sda = new SqlDataAdapter(@"Select * from Login
                                                     where username='" + txt_UserName.Text + "' and userpassword = '" + txt_Password.Text + "'", con);

                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count == 1)
                {
                    StockMain sm = new StockMain();
                    sm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid User Name Or Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btn_Clear_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}