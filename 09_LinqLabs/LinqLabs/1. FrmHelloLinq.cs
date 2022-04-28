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
    public partial class FrmHelloLinq : Form
    {
        public FrmHelloLinq()
        {
            InitializeComponent();
            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);
            this.ordersTableAdapter1.Fill(this.nwDataSet1.Orders);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //    public interface IEnumerable<T>
            //    System.Collections.Generic 的成員
            //摘要:
            //公開支援指定類型集合上簡單反覆運算的列舉值。

            //使用foreach 語法糖
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            foreach(int n in nums)
            {
                this.listBox1.Items.Add(n);
            }
            //=============================  
            //使用c#內部編譯
            this.listBox1.Items.Add("======================"); 
            System.Collections.IEnumerator en = nums.GetEnumerator();
            while (en.MoveNext()){
                this.listBox1.Items.Add(en.Current);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<int> list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9,10 };
            foreach(int n in list)
            {
                this.listBox1.Items.Add(n);
            }
            this.listBox1.Items.Add("======================");

            List<int>.Enumerator en = list.GetEnumerator();
            //var en = list.GetEnumerator();  //u,3
            while (en.MoveNext())  //當他前進時
            {
                this.listBox1.Items.Add(en.Current);  //他就加入目前的值
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //step 1 : Define Data Source  定義資料來源
            //step 2 : Define Query  定義規則
            //step 3 : Execute Query  執行上面的規則

            //step1
            int [] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //step2
            IEnumerable<int> q = from n in nums
                                                         where n > 5
                                                         select n;
            //step3
            foreach (int n in q)
            {
                this.listBox1.Items.Add(n);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {  //使用C#的語法
            //step1
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,11,12 };
            //step2
            IEnumerable<int> q = from n in nums
                                     //where (n >= 5 && n <= 8) && (n%2==0)
                                 where n < 3 || n > 8
                                 select n;
            //step3
            foreach (int n in q)
            {
                this.listBox1.Items.Add(n);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            IEnumerable<int> q = from n in nums
                                 where IsEven(n)
                                 select n;
            foreach (int n in q)
            {
                this.listBox1.Items.Add(n);
            }
        }

        bool IsEven (int n)
        {
            //if (n % 2==0)
            //{
            //    return true;
            //}
            //else { return false; };

            return n % 2 == 0;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
          
            IEnumerable<Point> q = from n in nums   //命名時上下的命名物件要一樣
                                 where n > 5
                                 select new Point (n , n*n);
           
            foreach (Point pt in q)
            {
                this.listBox1.Items.Add(pt.X+" , "+pt.Y);
            }
            //======================================
            //使用<Point> 也等於上面的結果，
            List<Point> list = q.ToList();  //=>foreach(...item ....in q .... ) List.Add(item)  return list;
            dataGridView1.DataSource = list;

            //======================================
            //使用Chart 圖表顯示結果
            this.chart1.DataSource = list;
            this.chart1.Series[0].XValueMember = "X";
            this.chart1.Series[0].YValueMembers = "Y";
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
        }

        private void button1_Click(object sender, EventArgs e)
        {  //找出含有apple的字樣的
            string[] words = { "aaa", "Apple", "pineApple", "xxxapple" };
            IEnumerable<string> q = from w in words
                                                            where w.ToLower().Contains("apple")&&w.Length>5  //ToLower()--把字串全部轉成小寫再去比
                                                            select w;
            foreach (string s in q)
            {
                this.listBox1.Items.Add(s);
            }
            this.dataGridView1.DataSource = q.ToList();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = this.nwDataSet1.Products;
            IEnumerable<global::LinqLabs.NWDataSet.ProductsRow> q = from p in this.nwDataSet1.Products
                                                                    where !p.IsUnitPriceNull() && p.UnitPrice > 30 && p.UnitPrice < 50 
                                                                                                                      && p.ProductName.StartsWith("M")
                                                                    select p;

            dataGridView1.DataSource = q.ToList();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = this.nwDataSet1.Orders;
            var q = from p in this.nwDataSet1.Orders
                    where p.OrderDate.Year == 1997 && p.OrderDate.Month<4    //抓"特定年分"+"特定月份"
                    orderby p.OrderDate descending
                    select p;

            dataGridView1.DataSource = q.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //IEnumerable<int >  q=  nums.Where<....> 
        }
    }
}
