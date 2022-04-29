using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Starter
{
    public partial class FrmLINQ架構介紹_InsideLINQ : Form
    {
        public FrmLINQ架構介紹_InsideLINQ()
        {
            InitializeComponent();
            productsTableAdapter1.Fill(this.nwDataSet1.Products);
        }

        private void button30_Click(object sender, EventArgs e)
        {
            //一般非泛用集合沒有.where的方法可以用
            System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
            arrayList.Add(3);
            arrayList.Add(4);
            arrayList.Add(1);
            //其實一般<>裡面可以不用加型別，
            //<>裡面加值的原因為，因為Arraylist為非泛型的集合，所以要跟據他加進去的東西進行轉型
            var q = from n in arrayList.Cast<int>()   
                    where n > 2
                    select new { N = n };  //因為Cast型態為<int>沒有屬性，datagrid秀不出來，所以需要給他一個屬性
            dataGridView1.DataSource = q.ToList();
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var q = (from p in this.nwDataSet1.Products
                     orderby p.UnitPrice descending
                     select p).Take(5);   //只拿前五個的意思
            dataGridView1.DataSource = q.ToList();
        }
    }
}