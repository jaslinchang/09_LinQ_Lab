using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHomeWork
{
    public partial class Frm作業_1 : Form
    {
        public Frm作業_1()
        {
            InitializeComponent();            
            this.ordersTableAdapter1.Fill(this.nwDataSet1.Orders);
            LoadToComboBox();
        }

        
        private void button14_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files =  dir.GetFiles();
            IEnumerable<System.IO.FileInfo> q = from n in files
                    where n.Extension == ".log"
                    select n;

              this.dataGridView1.DataSource = q.ToList();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();
            IEnumerable<System.IO.FileInfo> q = from n in files
                                                where n.CreationTime.Year==2022
                                                select n;

            this.dataGridView1.DataSource = q.ToList();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();
            IEnumerable<System.IO.FileInfo> q = from n in files
                                                where n.Length>99999
                                                select n;
            this.dataGridView1.DataSource = q.ToList();
        }

        //==================================================

        private void button6_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = this.nwDataSet1.Orders;

            //var q = from n in this.nwDataSet1.Orders
            //        where !n.IsShipRegionNull()&&!n.IsShipPostalCodeNull()&&!n.IsShippedDateNull()
            //        select n;
            //this.dataGridView1.DataSource = q.ToList();
        }

        private void LoadToComboBox()
        {
            this.comboBox1.Text = "請選擇年份";
            var q = from n in this.nwDataSet1.Orders
                          select  n.OrderDate.Year ;            
            foreach(int a in q.Distinct())
            {
                comboBox1.Items.Add(a);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (comboBox1.Text == "請選擇年份")
            //{
            //    MessageBox.Show("請先選擇年份");
            //}
            var q = from o in this.nwDataSet1.Orders
                    where o.OrderDate.Year == int.Parse(comboBox1.Text)
                    select o;
            this.dataGridView1.DataSource = q.ToList();


            var q1 = from d in this.nwDataSet1.Order_Details
                     join o in nwDataSet1.Orders
                     on d.OrderID equals o.OrderID
                     where o.OrderDate.Year == int.Parse(comboBox1.Text)
                     select d;
            this.dataGridView2.DataSource = q1.ToList();

            //var qr1 = from o in this.nwDataSet1.Orders
            //        join d in nwDataSet1.Order_Details
            //        on o.OrderID equals d.OrderID
            //        where o.OrderDate.Year == int.Parse(comboBox1.Text)
            //        select o;
            //this.dataGridView2.DataSource = q1.ToList();
        }



        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            //this.nwDataSet1.Products.Take(10);//Top 10 Skip(10)

            //Distinct()
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var q = from n in this.nwDataSet1.Orders
            //        //where n.OrderDate.Year
            //        select n;

            //this.comboBox1.Items.Add(q.ToList());
            ////this.dataGridView1.DataSource = q.ToList();

        }

        
    }
}
