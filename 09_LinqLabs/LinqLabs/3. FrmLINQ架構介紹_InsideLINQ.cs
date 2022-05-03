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

        //when execture Query q...
        //1. foreach
        //2.ToXXX()     (ex) ToList()
        //3.Aggregaion(彙總函數)  Sum().Min()...

        private void button1_Click(object sender, EventArgs e)
        {
            //抓陣列彙總函數
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            listBox1.Items.Add("Sum=" + nums.Sum());
            listBox1.Items.Add("Min=" + nums.Min());
            listBox1.Items.Add("Max=" + nums.Max());
            listBox1.Items.Add("Avg=" + nums.Average());
            listBox1.Items.Add("Count=" + nums.Count());
            //用彙總函數抓北風庫存量============================
            listBox1.Items.Add("Products UnitStock Sum = " + $"{nwDataSet1.Products.Sum(p => p.UnitsInStock):f2}");
            listBox1.Items.Add("Products UnitStock Max = " + $"{nwDataSet1.Products.Max(p => p.UnitsInStock):f2}");
            listBox1.Items.Add("Products UnitStock Min = " + nwDataSet1.Products.Min(p => p.UnitsInStock));
            listBox1.Items.Add("Products UnitStock Avg = " + nwDataSet1.Products.Average(p => p.UnitsInStock));


        }

        private void button3_Click(object sender, EventArgs e)
        {
            //I. 延遲查詢(deferred execution)
            //定義時不會估算
            //使用時才估算
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int i = 0;
            var q = from n in nums
                    select ++i;
            //foreach  執行  Query
            foreach(var v in q)
            {
                listBox1.Items.Add(string.Format("v={0}, i={1} ", v, i));
            }
            listBox1.Items.Add("========================");

            i = 0;
            var q1 = (from n in nums
                      select ++i).ToList();
            //foreach  執行  Query
            foreach (var v in q1)
            {
                listBox1.Items.Add(string.Format("v={0}, i={1} ", v, i));
            }
        }
    }
}