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
            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);
            this.order_DetailsTableAdapter1.Fill(this.nwDataSet1.Order_Details);
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
            //this.dataGridView1.DataSource = this.nwDataSet1.Orders;

            var q = from n in this.nwDataSet1.Orders
                    //where !n.IsShipRegionNull() && !n.IsShipPostalCodeNull() && !n.IsShippedDateNull()
                    select n;
            this.dataGridView1.DataSource = q.ToList();
            //========
            var q1 = from d in this.nwDataSet1.Order_Details
                     select d;
            this.dataGridView2.DataSource = q1.ToList();
        }

        private void LoadToComboBox()
        {
            this.comboBox1.Text = "請選擇年份";
            var q = from n in this.nwDataSet1.Orders
                    select n.OrderDate.Year;
            foreach (int a in q.Distinct())
            {
                comboBox1.Items.Add(a);
            }
            //用Group by
            //var q = from n in nwDataSet1.Orders
            //        group n by n.OrderDate.Year into years
            //        select years;

            //foreach(var years in q)
            //{
            //    comboBox1.Items.Add(years.Key);
            //}
        }              


        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            var q1 = from d in this.nwDataSet1.Order_Details
                     join o in nwDataSet1.Orders
                     on d.OrderID equals o.OrderID
                     where d.OrderID == (int)dataGridView1.CurrentRow.Cells[0].Value + 1
                     select d;
            this.dataGridView2.DataSource = q1.ToList();
        }
              

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource == null)
            {
                MessageBox.Show("請先選擇年份");
            }
            else if (comboBox1.Text != "請選擇年份")
            {
                //orders
                var q = from o in this.nwDataSet1.Orders
                        where o.OrderDate.Year == int.Parse(comboBox1.Text)
                        select o;
                this.bindingSource1.DataSource = q.ToList();
                this.dataGridView1.DataSource = bindingSource1;

                //order details
                var q1 = from d in this.nwDataSet1.Order_Details
                         join o in nwDataSet1.Orders
                         on d.OrderID equals o.OrderID
                         where o.OrderDate.Year == int.Parse(comboBox1.Text)
                         select d;
                this.dataGridView2.DataSource = q1.ToList();
            }

        }
        //上下頁
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = this.nwDataSet1.Products;
        }

        int page = 1;  //設為初始為第一頁
        int shownum = 0;
        int skipnum = 0;
        bool next = true;
        bool pre = true;

        private void button12_Click(object sender, EventArgs e)
        {
            if (pre)
            {
                page -= 1;
                shownum = (int.Parse(textBox1.Text) * page);  //抓textbox 值
                skipnum = (page - 1) * (shownum / page);
                var q = from p in this.nwDataSet1.Products.Take(shownum).Skip(skipnum)
                        select p;

                dataGridView1.DataSource = q.ToList();
                label6.Text = page.ToString();

            }
       

        }
      
        private void button13_Click(object sender, EventArgs e)
        {
            if (pre)
            {
                shownum = int.Parse(textBox1.Text) * page;  //抓textbox 值
                skipnum = (page - 1) * (shownum / page);
                var q = from p in this.nwDataSet1.Products.Take(shownum).Skip(skipnum)

                        select p;

                dataGridView1.DataSource = q.ToList();
                page += 1;
                label6.Text = page.ToString();
            }
            else
            {

            }
          
        }
    }
}


// int page = int.Parse(textBox1.Text);
// var q = from p in this.nwDataSet1.Products.Take(page)
//         select p;

//dataGridView1.DataSource= q.ToList();