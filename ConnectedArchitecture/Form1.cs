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

namespace ConnectedArchitecture
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-C5I21SI;Initial Catalog=NORTHWND;Integrated Security=True");

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Select * from Categories",con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            comboBox1.DisplayMember = "CategoryName";
            comboBox1.ValueMember = "CategoryID";

            List<Category> list = new List<Category>();
            while (reader.Read())
            {
                Category cat = new Category();
                cat.CategoryID = (int)reader["CategoryID"];
                cat.CategoryName = reader["CategoryName"].ToString();
                list.Add(cat);
            }
            con.Close();
            comboBox1.DataSource= list;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            var SelectedID=comboBox1.SelectedValue;
            con.Open();
            SqlCommand sqlCommand = new SqlCommand("Select * from Products where CategoryID="+SelectedID,con);
            SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Button button = new Button();
                button.Name = "b" + reader["ProductID"];
                button.Height = 50;
                button.AutoSize = true;
                button.Text = reader["ProductName"].ToString();
                flowLayoutPanel1.Controls.Add(button);
                button.Click += GetProductDetail;
            }
            con.Close();
        }

        private void GetProductDetail(object clicked,EventArgs clickdetail)
        {
            Button btn=(Button)clicked;
            var IDtxt = btn.Name.Remove(0,1);

            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from Products where ProductID="+IDtxt,con);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            ProductDetails pd=new ProductDetails();
            pd.lbl_productname.Text = reader["ProductName"].ToString();
            pd.lbl_qpu.Text = reader["QuantityPerUnit"].ToString();
            pd.Visible = true;
            con.Close();
        }
    }
}
