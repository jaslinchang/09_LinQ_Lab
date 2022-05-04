using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Starter
{
    public partial class FrmLINQ_To_XXX : Form
    {
        public FrmLINQ_To_XXX()
        {
            InitializeComponent();
            this.ordersTableAdapter1.Fill(this.nwDataSet1.Orders);
            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);
            this.categoriesTableAdapter1.Fill(this.nwDataSet1.Categories);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            IEnumerable<IGrouping<string, int>> q = from n in nums  //group 的型別為"IGrouping<x,x>"，IGrouping的屬性=key 
                                                 group n by n % 2==0?"偶數":"奇數";  // n%2 為key，用group n(key) 分出偶數跟奇數 
            dataGridView1.DataSource = q.ToList();  //僅列出型別
            //用treeview 做呈現所有值
            foreach(var group in q)  //先抓分類的群組: 1,0
            {
                TreeNode node = treeView1.Nodes.Add(group.Key.ToString());
                foreach(var item in group)   //再抓各群組內的值 : 1:1.3.5.7.9 /  0: 2.4.6.8.10
                {
                    node.Nodes.Add(item.ToString());
                }
            }
            //用Listview 做呈現所有值
            foreach(var group in q)
            {
                ListViewGroup lvg = listView1.Groups.Add(group.Key.ToString(), group.Key.ToString());  //前面的為分集合群組，後面的為設定集合名稱
                foreach(var item in group)
                {
                    listView1.Items.Add(item.ToString()).Group = lvg;  //把那些集合的內容存在 lvg 的 group 裡面
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {   //分成奇數偶數
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 13 };
            var q = from n in nums
                    group n by n % 2 == 0 ? "偶數" : "奇數" into g  //into g 把資料先暫存在g裡面
                    select new
                    {     //因為要存在datagridview ，所以要給他一個型別
                        Mykey = g.Key,
                        MyCount = g.Count(),
                        MyMin = g.Min(),
                        MyAvg = g.Average(),
                        MyGroup=g
                    };  
            dataGridView1.DataSource = q.ToList();  //僅列出型別
             //用treeview 做呈現所有值
            foreach (var group in q)  //先抓分類的群組: 1,0
            {
                string s = $"{group.Mykey}({group.MyCount})";
                TreeNode node = treeView1.Nodes.Add(group.Mykey.ToString(),s);
                foreach (var item in group.MyGroup)   //再抓各群組內的值 : 1:1.3.5.7.9 /  0: 2.4.6.8.10
                {
                    node.Nodes.Add(item.ToString());
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {  //分成大中小的方式，使用方法，讓每次抓n的值都帶入MyKey()的方法去找，再分群存在g裡面
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 13 };
            var q = from n in nums
                    group n by MyKey(n) into g  //into g 把資料先暫存在g裡面
                    select new
                    {     //因為要存在datagridview ，所以要給他一個型別
                        Mykey = g.Key,
                        MyCount = g.Count(),
                        MyMin = g.Min(),
                        MyAvg = g.Average(),
                        MyGroup = g
                    };
            dataGridView1.DataSource = q.ToList();  //僅列出型別
            
            //用treeview 做呈現所有值
            foreach (var group in q)  //先抓分類的群組: 1,0
            {
                string s = $"{group.Mykey}({group.MyCount})";
                TreeNode node = treeView1.Nodes.Add(group.Mykey.ToString(), s);
                foreach (var item in group.MyGroup)   
                {
                    node.Nodes.Add(item.ToString());
                }
            }

            //用chart呈現================================
            this.chart1.DataSource = q.ToList();
            chart1.Series[0].XValueMember = "MyKey";    //分成小中大，抓key的值
            chart1.Series[0].YValueMembers = "MyCount";
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            chart1.Series[1].XValueMember = "MyKey";
            chart1.Series[1].YValueMembers = "MyAvg";
            chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
        }

        private string MyKey(int n)
        {
            if (n < 5) return "small";
            else if (n < 10) return "Medium";
            else return "Laege";
        }

        private void button38_Click(object sender, EventArgs e)
        {  //抓C槽副檔名的統計數量
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();
            this.dataGridView1.DataSource = files;

            var q = from d in files
                    group d by d.Extension into g
                    orderby g.Count() descending
                    select new
                    {
                        Mykey = g.Key,
                        MyCount = g.Count(),
                    };
            this.dataGridView2.DataSource = q.ToList();            

        }

        private void button12_Click(object sender, EventArgs e)
        {   //抓北峰資料庫orders訂單每年有幾筆
            dataGridView1.DataSource = nwDataSet1.Orders;
            var q = from o in this.nwDataSet1.Orders
                    group o by o.OrderDate.Year into g
                    select new
                    {
                        MyYear=g.Key,   //秀分了哪幾年
                        MyCount = g.Count()  //秀每年的總數量
                    };
            this.dataGridView2.DataSource = q.ToList();
            //只抓1997年的訂單有幾筆=================
            int count = (from o in this.nwDataSet1.Orders    //因為出來的值為一整數，所以前面不用var，型別可以直接用int
                         where o.OrderDate.Year == 1997
                         select o).Count(); 
            MessageBox.Show(count.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {  //用Let查詢
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();
            int count = (from f in files
                         let s = f.Extension  //等於設變數給他存
                         where s == ".exe"   //可以直接在函式內使用
                         select f).Count();
            MessageBox.Show(".log 數量為 : "+count.ToString());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string s = "This is a book. this is a pen.    this is an apple";
            char[] chars = { ' ', ',', '?', '.'};
            //string[] words = s.Split();
            string[] words = s.Split(chars, StringSplitOptions.RemoveEmptyEntries);   //後面的為是否要移除空字串
            var q = from w in words
                    group w by w.ToUpper() into g  //by key 設為轉型成大寫字
                    select new
                    {
                        MyKey = g.Key,
                        MuCount = g.Count()
                    };
            dataGridView1.DataSource = q.ToList();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //集合運算子 : Distinct/Union/Intersect/Except
            int[] nums1 = { 1, 2, 3, 5, 11, 2 };
            int[] nums2 = { 1, 3, 66, 77, 111 };
            IEnumerable<int> q = null;
            q = nums1.Intersect(nums2);
            q = nums1.Distinct();
            q = nums1.Union(nums2);

            //切割運算子  Take/Skip
            q = nums1.Take(2);

            //數量詞作業  Any/All/Contains      => 都是回傳布林值 
            bool result;
            result = nums2.Any(n => n > 100);   //只要有一筆符合
            result = nums1.All(n => n > 20);        //全部都要符合才可以
            //單一元素運算子
            //First/Last/Single/ ElementAt
            //FirstOrDefault/LastOrDefault/SingleOrDefault/ElementAtOrDefault
            int n1;
            n1 = nums1.First();
            n1 = nums1.Last();
            n1 = nums1.ElementAtOrDefault(13);   //抓序列中的指定位置，如果找不到則會回傳int 預設值=0
            //產生作業
            //Range / Repeat / Empty / DefaultIfEmpty
            var q1 = Enumerable.Range(1, 100).Select(n=>new { N=n} );  //列出1~100的值   //select是為了製造屬性，因為datagrid只能秀屬性
            dataGridView1.DataSource = q1.ToList();
            var q2 = Enumerable.Repeat(20, 100).Select(n => new { N = n });   //列出100筆為20的值
            dataGridView2.DataSource = q2.ToList();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var q = from p in nwDataSet1.Products
                    group p by p.CategoryID into g
                    select new
                    {
                        ID = g.Key,
                        MyAvg = g.Average(p=>p.UnitPrice)
                   };
            dataGridView1.DataSource = q.ToList();

            var q1 = from c in nwDataSet1.Categories
                     join p in nwDataSet1.Products
                     on c.CategoryID equals p.CategoryID
                     group p by c.CategoryName into g
                     select new { CategoryName = g.Key, MyAvg = $"{g.Average(p => p.UnitPrice) :f2}"};  //轉成兩位小數
            dataGridView2.DataSource = q1.ToList();
        }
    }
}
