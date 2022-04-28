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
        }
    }
}
