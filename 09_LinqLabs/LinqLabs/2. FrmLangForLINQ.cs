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
    public partial class FrmLangForLINQ : Form
    {
        public FrmLangForLINQ()
        {
            InitializeComponent();
            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);
        }
        //SWAP
        private void button4_Click(object sender, EventArgs e)
        {   //SWAP(int)
            int a, b;
            a = 100;
            b = 200;
            MessageBox.Show("a=" + a.ToString() + ", b=" + b.ToString());
            Swap(ref a, ref b);
            MessageBox.Show("a=" + a.ToString() + ", b=" + b.ToString());
            //==========================
            //SWAP( string )
            string n1 = "aaa";
            string n2 = "bbb";
            MessageBox.Show(n1 + ", " + n2);
            Swap(ref n1, ref n2);
            MessageBox.Show(n1 + ", " + n2);
        }
        void Swap(ref int a, ref int b)
        {
            int T = a;
            a = b;
            b = T;
        }
        void Swap(ref string a, ref string b)
        {
            string T = a;
            a = b;
            b = T;
        }
        //SWAP  object
        private void button5_Click(object sender, EventArgs e)
        { 
            object n1, n2;
            n1 = 100;
            n2 = 200;
            MessageBox.Show(n1 + ", " + n2);
            Swap2(ref n1, ref n2);
            MessageBox.Show(n1 + ", " + n2);
        }
        void Swap2(ref Object a, ref Object b)
        {
            Object T = a;
            a = b;
            b = T;
        }
        //SWAP<T> 
        private void button7_Click(object sender, EventArgs e)
        {
            //SWAP<T> int
            int n1, n2;
            n1 = 100;
            n2 = 200;
            MessageBox.Show(n1 + ", " + n2);
            SwapAnyType<int>(ref n1, ref n2);
            MessageBox.Show(n1 + ", " + n2);
            //==========================
            //SWAP<T> string
            string s1 = "aaa";
            string s2 = "bbb";
            MessageBox.Show(s1 + ", " + s2);
            SwapAnyType(ref s1, ref s2);
            MessageBox.Show(s1 + ", " + s2);
        }

        void SwapAnyType<T>(ref T n1, ref T n2)
        {
            T temp = n1;
            n1 = n2;
            n2 = temp;
        }

        //=========================================
        //Delegate
        private void button2_Click(object sender, EventArgs e)
        {
            //            嚴重性 程式碼 說明 專案  檔案 行   隱藏項目狀態
            //錯誤  CS0123  'ButtonX_Click' 沒有任何多載符合委派 'EventHandler'

            //            this.buttonX.Click += ButtonX_Click;

            //C#1.0 具名方法
            buttonX.Click += new EventHandler(aaa);
            buttonX.Click += bbb;

            //=========================
            //C# 2.0 匿名方法
            buttonX.Click += delegate (object sender1, EventArgs e1) //加1是因為跟btn2的變數名稱一樣
              {
                  MessageBox.Show("C# 2.0 匿名方法");
              };
            //=========================
            //C# 3.0 匿名方法  Lambda 運算式  =>  goes to
            buttonX.Click += (object sender2, EventArgs e2) =>
              {
                  MessageBox.Show("C# 3.0 匿名方法  Lambda 運算式");
              };

        }

        private void ButtonX_Click()
        {
            MessageBox.Show("Button X");
        }
        private void aaa(object sender, EventArgs e)
        {
            MessageBox.Show("aaa");
        }
        private void bbb(object sender, EventArgs e)
        {
            MessageBox.Show("bbb");
        }
        bool Test(int n)
        {
            return n > 5;  //直接會回傳true /false
        }
        bool Test1(int n)  
        {
            return n % 2 == 0;
        }

        //step 1 : create delegate  型別
        //step 2 : create delegate Object(new ...)
        //step 3 : invoke /call method

        delegate bool MyDelegate(int n);

        private void button9_Click(object sender, EventArgs e)
        {
            bool result = Test(4);
            MessageBox.Show("result=" + result);
            //==============================
            MyDelegate delegateObj = new MyDelegate(Test);  //再帶到Test 的方法
            bool result1 = delegateObj(7);
            MessageBox.Show("result=" + result1);
            //==============================
            delegateObj = new MyDelegate(Test1);  //委派帶入的只會對應一個參數
            result1 = delegateObj(9);
            MessageBox.Show("result=" + result1);

            //=========================
            //C# 2.0 匿名方法
            delegateObj = delegate (int n) { return n > 5; };
            result = delegateObj(6);
            MessageBox.Show("result=" + result);

            //=========================
            //C# 3.0 匿名方法  Lambda 運算式  =>  goes to
            delegateObj = n => n > 5;
            result = delegateObj(1);
            MessageBox.Show("result=" + result);


        }
        List<int> MyWhere(int[] nums, MyDelegate delegateObj)
        {
            List<int> list = new List<int>();
            foreach (int n in nums)
            {
                if (delegateObj(n))
                {
                    list.Add(n);
                }                
            }
            return list;
        }
        private void button10_Click(object sender, EventArgs e)
        {
            //委派具名方法
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            List<int> Large_list = MyWhere(nums, Test);
            foreach(int n in Large_list)
            {
                listBox1.Items.Add(n);
            }
            //委派匿名方法3.0
            List<int> oddlist = MyWhere(nums, n =>n % 2 == 1);
            List<int> evenlist = MyWhere(nums, n => n % 2 == 0);
            foreach (int n in oddlist)
            {
                listBox1.Items.Add(n);
            }
            foreach (int n in evenlist)
            {
                listBox2.Items.Add(n);
            }
        }

        IEnumerable<int> MyIterator(int [] nums,MyDelegate delegateObj)
        {
            foreach(int n in nums)
            {
                if (delegateObj(n))
                {
                    yield return n;
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            IEnumerable<int> q = MyIterator(nums, n => n >6);
            foreach (int n in q)
            {
                listBox1.Items.Add(n);
            }            
        }

        private void button3_Click(object sender, EventArgs e)
        {   //用定義的方法去做查詢
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //var q = from n in nums
            //        where n > 5
            //        select n;
            IEnumerable<int> q = nums.Where(n => n > 5);  //.where 等於上面寫的 篩選條件
            foreach (int n in q)
            {
                listBox1.Items.Add(n);
            }
            //=================================
            //抓字串
            string[] words = { "aaa", "bbbbbb", "ccccccccccccc" };
            //IEnumerable<string>
                var q1 = words.Where(w => w.Length > 5);
            foreach(string w in q1)
            {
                listBox2.Items.Add(w);
            }
            dataGridView1.DataSource = q1.ToList();  //這邊會變抓屬性，秀出來是字串長度
            //=================================
            //抓北峰資料庫產品單價>30
            var q3 = nwDataSet1.Products.Where(p => p.UnitPrice > 30);

            dataGridView2.DataSource = q3.ToList();
      
        }

        private void button45_Click(object sender, EventArgs e)
        {  //初始化時要有值，系統才能自動判定型別
            var n = 100;
            var s = "abc";
            var p = new Point(100, 100);
        }

        private void button41_Click(object sender, EventArgs e)
        { //物件初始化
            MyPoint pt1 =new MyPoint();
            pt1.P1 = 100;    //set
            int w = pt1.P1;   //get

            //MessageBox.Show(pt1.p1.ToString());  //get
            List<MyPoint> list = new List<MyPoint>();
            list.Add(new MyPoint());
            list.Add(new MyPoint(100));
            list.Add(new MyPoint(100,300));
            list.Add(new MyPoint("aaa"));
            // 物件初始化-大括號 {   } 
            list.Add(new MyPoint { P1 = 1, P2 = 5, Field1 = "ccc", Field2 = "ddd" });
            list.Add(new MyPoint { P1 = 64});
            list.Add(new MyPoint { P1 = 7, P2 = 55, Field1 = "ccc", Field2 = "ddd" });
            dataGridView1.DataSource = list;
            //============================
            //物件初始化-集合初始
            List<MyPoint> list2 = new List<MyPoint>
            {
                new MyPoint { P1 = 1, P2 = 1, Field1 = "ccc", Field2 = "ddd" },
                new MyPoint { P1 = 22, P2 = 22, Field1 = "ccc", Field2 = "ddd" },
                new MyPoint { P1 = 333, P2 = 333, Field1 = "ccc", Field2 = "ddd" }
            };
            dataGridView2.DataSource = list2;
        }

        public class MyPoint
        {         
            private int m_p1;
            public int P1
            {
                get
                {
                    return m_p1;  //把值取出來
                }
                set
                {
                    m_p1 = value;  //把值存進去
                }
            }
            public int P2 { get; set; }
            public string Field1 = "xxx", Field2 = "yyy";

            //建構子方法
            public MyPoint()
            {

            }
            public MyPoint(int p1)
            {
                this.P1 = p1;  //+this可以更確定是指為MyPoint的那個物件，P1為屬性
            }
            public MyPoint(int p1, int p2)
            {
                this.P1 = p1;
                this.P2 = p2;
            }
            public MyPoint(string Field1)
            {

            }

        }

        private void button43_Click(object sender, EventArgs e)
        {     //匿名型別，var 會自動幫忙建立適合的型別
            var x = new { P1 = 99, P2 = 88, P3 = 77 };
            var y = new { P1 = 99, P2 = 88, P3 = 77 };
            var z = new {username="aaa",password="bbb "};

            listBox1.Items.Add(x.GetType());  //秀出他取名ㄉ型別
            listBox1.Items.Add(y.GetType());
            listBox1.Items.Add(z.GetType());
            //=========================
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //1.0
            //var q = from n in nums
            //        where n > 5
            //        select new { N = n, Squar = n * n, Cube = n * n * n };
            //============================
            //用方法寫
            var q = nums.Where(n => n > 5).Select(n => new { N = n, Squar = n * n, Cube = n * n * n });
            //先篩出>5的數字，然後再從裡面建立新的方法
            dataGridView1.DataSource = q.ToList();

            //===================================
            //抓北峰的資料，單價>30 ，且秀出$*數量
            var q1 = from p in nwDataSet1.Products
                            where p.UnitPrice > 30
                            select new {
                                ID = p.ProductID, 
                                產品名稱 = p.ProductName, 
                                p.UnitPrice,
                                p.UnitsInStock, 
                                TotalPrice = $"{p.UnitPrice * p.UnitsInStock:c2}"
                            };
            dataGridView2.DataSource = q1.ToList();
        }

        private void button32_Click(object sender, EventArgs e)
        {
            string s1 = "abcd";
            int n = s1.WordCount();
            MessageBox.Show("WordCount= " + n);
            //=======================
            string s2 = "123456789";
            n = s2.WordCount();
            //n = MystringExtend.WordCount(s2);    //等於上片的
            MessageBox.Show("WordCount= " + n);
            //=======================
            char ch = s2.Chars(3);
            MessageBox.Show("Chars= " + ch);
        }

       
    }
    public static class MystringExtend   //擴充方法為靜態類別 
    {
        public static int WordCount(this string s)  //靜態類別==靜態方法
        {
            return s.Length;
        }
        public static char Chars (this string s ,int index)
        {
            return s[index];
        }
    }
}
