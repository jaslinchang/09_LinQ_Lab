using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHomeWork
{
    public partial class Frm作業_2 : Form
    {
        public Frm作業_2()
        {
            InitializeComponent();
            this.productPhotoTableAdapter1.Fill(this.awDataSet1.ProductPhoto);
            LoadYearToComobox();
        }

        private void LoadYearToComobox()
        {
            this.comboBox2.Text = "請選擇季節";
            this.comboBox3.Text = "請選擇年份";
            
            var q = from y in this.awDataSet1.ProductPhoto
                    orderby y.ModifiedDate
                    select y.ModifiedDate.Year;
            foreach (int y in q.Distinct())
            {
                this.comboBox3.Items.Add(y);
            }

        }
                  
        private void button11_Click(object sender, EventArgs e)
        {  //all腳踏車
            this.pictureBox1.DataBindings.Clear();
            this.bindingSource1.Clear();
            IEnumerable<LinqLabs.AWDataSet.ProductPhotoRow> q =  from p in this.awDataSet1.ProductPhoto
                                                                                                                                   select p;

            this.bindingSource1.DataSource = q.ToList();
            this.dataGridView1.DataSource = bindingSource1;
            this.pictureBox1.DataBindings.Add("Image", bindingSource1, "LargePhoto", true);
            this.label2.Text = dataGridView1.Rows.Count.ToString();
        }
        private void button3_Click(object sender, EventArgs e)
        {   //區間腳踏車
            this.pictureBox1.DataBindings.Clear();
            this.bindingSource1.Clear();
            var q = this.awDataSet1.ProductPhoto.Where(y => y.ModifiedDate > dateTimePicker1.Value && y.ModifiedDate < dateTimePicker2.Value);

            this.bindingSource1.DataSource = q.ToList();
            this.dataGridView1.DataSource = bindingSource1;
            this.pictureBox1.DataBindings.Add("Image", bindingSource1, "LargePhoto", true);
            this.label2.Text = dataGridView1.Rows.Count.ToString();
        }
        private void button5_Click(object sender, EventArgs e)
        {  //某年腳踏車
            if (comboBox3.Text == "請選擇年份")
            {
                MessageBox.Show("請先選擇年份");
            }
            else 
            {
                this.pictureBox1.DataBindings.Clear();
                this.bindingSource1.Clear();
                int years = int.Parse(this.comboBox3.Text);
                IEnumerable<LinqLabs.AWDataSet.ProductPhotoRow> q = this.awDataSet1.ProductPhoto.Where(y => y.ModifiedDate.Year == years);

                this.bindingSource1.DataSource = q.ToList();
                this.dataGridView1.DataSource = bindingSource1;
                this.pictureBox1.DataBindings.Add("Image", bindingSource1, "LargePhoto", true);
                this.label2.Text = dataGridView1.Rows.Count.ToString();
            }
         
        }
        private void button10_Click(object sender, EventArgs e)
        {  //某季腳踏車
            if (comboBox2.Text == "請選擇季節")
            {
                MessageBox.Show("請先選擇季節");
            }
            else
            {
                this.pictureBox1.DataBindings.Clear();
                this.bindingSource1.Clear();
                if (comboBox2.Text == "第一季(1.2.3月)")
                {
                    var q1 = this.awDataSet1.ProductPhoto.Where(y => y.ModifiedDate.Month < 4);

                    this.bindingSource1.DataSource = q1.ToList();
                    this.dataGridView1.DataSource = bindingSource1;
                    this.pictureBox1.DataBindings.Add("Image", bindingSource1, "LargePhoto", true);
                    this.label2.Text = dataGridView1.Rows.Count.ToString();
                }
                else if (comboBox2.Text == "第二季(4.5.6月)")
                {
                    var q1 = this.awDataSet1.ProductPhoto.Where(y => y.ModifiedDate.Month > 3 && y.ModifiedDate.Month < 7);

                    this.bindingSource1.DataSource = q1.ToList();
                    this.dataGridView1.DataSource = bindingSource1;
                    this.pictureBox1.DataBindings.Add("Image", bindingSource1, "LargePhoto", true);
                    this.label2.Text = dataGridView1.Rows.Count.ToString();
                }
                else if (comboBox2.Text == "第三季(7.8.9.月)")
                {
                    var q1 = this.awDataSet1.ProductPhoto.Where(y => y.ModifiedDate.Month > 6 && y.ModifiedDate.Month < 10);

                    this.bindingSource1.DataSource = q1.ToList();
                    this.dataGridView1.DataSource = bindingSource1;
                    this.pictureBox1.DataBindings.Add("Image", bindingSource1, "LargePhoto", true);
                    this.label2.Text = dataGridView1.Rows.Count.ToString();
                }
                else if (comboBox2.Text == "第四季(10.11.12月)")
                {
                    var q1 = this.awDataSet1.ProductPhoto.Where(y => y.ModifiedDate.Month > 9);

                    this.bindingSource1.DataSource = q1.ToList();
                    this.dataGridView1.DataSource = bindingSource1;
                    this.pictureBox1.DataBindings.Add("Image", bindingSource1, "LargePhoto", true);
                    this.label2.Text = dataGridView1.Rows.Count.ToString();
                }
            }          
           
        }
      
    }
}


