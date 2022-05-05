using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinqLabs
{
    public partial class Frm考試 : Form
    {
        public Frm考試()
        {
            InitializeComponent();

            students_scores = new List<Student>()
            {
            new Student{ Name = "aaa", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Male" },
            new Student{ Name = "bbb", Class = "CS_102", Chi = 80, Eng = 80, Math = 100, Gender = "Male" },
            new Student{ Name = "ccc", Class = "CS_101", Chi = 60, Eng = 50, Math = 75, Gender = "Female" },
            new Student{ Name = "ddd", Class = "CS_102", Chi = 80, Eng = 70, Math = 85, Gender = "Female" },
            new Student{ Name = "eee", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Female" },
            new Student{ Name = "fff", Class = "CS_102", Chi = 80, Eng = 80, Math = 80, Gender = "Female" },

            };
        }

        NorthwindEntities dbContext = new NorthwindEntities();
        List<Student> students_scores;

        public class Student
        {
            public string Name { get; set; }
            public string Class { get;  set; }
            public int Chi { get; set; }
            public int Eng { get; internal set; }
            public int Math { get;  set; }
            public string Gender { get; set; }
        }

        Student student = new Student();
        int countt = 1;
               
        private void button36_Click(object sender, EventArgs e)
        {
            //搜尋 班級學生成績          
            if (countt == 1)
            {
                int count = students_scores.Count();
                // 共幾個 學員成績 ?	

                // 找出 前面三個 的學員所有科目成績			
                var q2 = (from s in students_scores
                          select new { s.Name,s.Chi,s.Eng,s.Math}).Take(3);
                dataGridView1.DataSource = q2.ToList();
                // 找出 後面兩個 的學員所有科目成績					
                var q3 = (from s in students_scores
                          select new { s.Name, s.Chi, s.Eng, s.Math }).Skip(count-2).Take(2);
                dataGridView2.DataSource = q3.ToList();
                //找出 Name 'aaa','bbb','ccc' 的學員國文英文科目成績
                var q4 = from s in students_scores
                         where s.Name == "aaa" || s.Name == "bbb" || s.Name == "ccc"
                         select new { s.Name,s.Chi, s.Eng };
                dataGridView3.DataSource = q4.ToList();
                countt++;
            }
            else if (countt == 2)
            {
                this.dataGridView1.DataSource = null;
                this.dataGridView2.DataSource = null;
                this.dataGridView3.DataSource = null;
                // 找出學員 'bbb' 的成績	                          
                var q5 = from s in students_scores
                         where s.Name == "bbb"
                         select new { s.Name, s.Chi, s.Eng, s.Math };
                dataGridView1.DataSource = q5.ToList();
                // 找出除了 'bbb' 學員的學員的所有成績 ('bbb' 退學)	
                var q6 = from s in students_scores
                         where s.Name != "bbb"
                         select new { s.Name, s.Chi, s.Eng, s.Math };
                dataGridView2.DataSource = q6.ToList();
                // 找出 'aaa', 'bbb' 'ccc' 學員 國文數學兩科 科目成績  |		
                var q7 = from s in students_scores
                         where s.Name == "aaa" || s.Name == "bbb" || s.Name == "ccc"
                         select new { s.Name, s.Chi, s.Math };
                dataGridView3.DataSource = q7.ToList();
                //數學不及格 ... 是誰  
                var q8 = from s in students_scores
                         where s.Math < 60
                         select s.Name;

                List<string> Name = new List<string>();
                foreach (string n in q8)
                {
                    Name.Add(n);
                }                
                MessageBox.Show($"數學不及格 : {String.Join(", ",Name)}");


                countt = 1;
            }



        }
  
        private void button37_Click(object sender, EventArgs e)
        {
            //個人 sum, min, max, avg
            var q = from s in students_scores
                    select s;
            dataGridView1.DataSource = q.ToList();
            //MessageBox.Show(students_scores.Max().ToString());
            //var q = from s in students_scores
            //        select new
            //        {
            //            Name = s.Name,
            //            Sum = s.Chi + s.Eng + s.Math,
            //            Avg = (s.Chi + s.Eng + s.Math) / 3,
            //            //Max = students_scores.Sum()
            //        };

            //var max = students_scores.Select(n => n.Name).Max();

            //MessageBox.Show(max);
            //========================================
            var q1 = from s in students_scores
                     group s by s.Name into g
                     select new
                     {
                         Name = g.Key,
                         //Max=

                         //Max = from s2 in g
                         //      group s2 by s2.Name into g2
                         //      //where (s2.Chi + s2.Eng + s2.Math) == g.Max(n => (s2.Chi + s2.Eng + s2.Math))
                         //      select g2.Key                               
                     };

            dataGridView2.DataSource = q1.ToList();


            //各科 sum, min, max, avg
            var q2 = from s in students_scores
                     group s by s.Name into g
                     select new
                     {
                         g.Key,
                         //Max = g.Max(g.Key)
                     };
            dataGridView3.DataSource = q2.ToList();
        }
        private void button33_Click(object sender, EventArgs e)
        {
            // split=> 分成 三群 '待加強'(60~69) '佳'(70~89) '優良'(90~100) 
            // print 每一群是哪幾個 ? (每一群 sort by 分數 descending)

        }

        private void button35_Click(object sender, EventArgs e)
        {
            // 統計 :　所有隨機分數出現的次數/比率; sort ascending or descending
            // 63     7.00%
            // 100    6.00%
            // 78     6.00%
            // 89     5.00%
            // 83     5.00%
            // 61     4.00%
            // 64     4.00%
            // 91     4.00%
            // 79     4.00%
            // 84     3.00%
            // 62     3.00%
            // 73     3.00%
            // 74     3.00%
            // 75     3.00%
        }

        private void button34_Click(object sender, EventArgs e)
        {           
            // 年度最高銷售金額 年度最低銷售金額
            var q1 =from d in this.dbContext.Order_Details.AsEnumerable()                     
                    group d by d.Order.OrderDate.Value.Year into g
                    //orderby g.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount))  descending
                    select new
                     {
                         Year = g.Key,
                        Max =$"{ g.Max(q => q.Order.Order_Details.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount))):c2}" ,
                        Min =$"{g.Min(q => q.Order.Order_Details.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount))):c2}" ,
                    };
            this.dataGridView1.DataSource = q1.ToList();
            //this.label1.Text = $"年度最高銷售金額 : {} \n年度最低銷售金額 : {}";

            // 那一年總銷售最好 ? 那一年總銷售最不好 ?  
            var q2 = from d in this.dbContext.Order_Details.AsEnumerable()
                     group d by d.Order.OrderDate.Value.Year into g
                     orderby g.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount)) descending
                     select new
                     {
                         Year = g.Key,
                         TotalPrice = $"{g.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount)):c2}"
                     };
            //this.dataGridView2.DataSource = q2.ToList();
            this.label2.Text = $"年總銷售最優 : {q2.First()} \n年總銷售最差 : {q2.Last()}";

            // 那一個月總銷售最好 ? 那一個月總銷售最不好 ?
            var q3 = from d in this.dbContext.Order_Details.AsEnumerable()                    
                     group d by d.Order.OrderDate.Value.Month into g
                     orderby g.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount)) descending
                     select new
                     {
                         Month = g.Key,
                         TotalPrice = $"{g.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount)):c2}"
                     };
            this.dataGridView3.DataSource = q3.ToList();
            this.label3.Text = $"月總銷售最優 : {q3.First()} \n月總銷售最差 : {q3.Last()}";

            // 每年 總銷售分析圖            
            var q4 = from d in this.dbContext.Order_Details.AsEnumerable()
                     group d by d.Order.OrderDate.Value.Year into g
                     select new
                     {
                         Mykey=g.Key,
                         TotalPrice = $"{g.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount)):c2}"
                     };
            this.dataGridView2.DataSource = q4.ToList();
            this.chart1.DataSource = q4.ToList();
            this.chart1.Series[0].XValueMember = "Mykey";
            this.chart1.Series[0].YValueMembers = "TotalPrice";
            this.chart1.Series[0].IsValueShownAsLabel = true;
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            // 每月 總銷售分析圖
            var q5 = from d in this.dbContext.Order_Details.AsEnumerable()
                     group d by d.Order.OrderDate.Value.Month into g
                     select new
                     {
                         Mykey = g.Key,
                         TotalPrice = $"{g.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount)):c2}"
                     };
            this.chart2.DataSource = q5.ToList();
            this.chart2.Series[0].XValueMember = "Mykey";
            this.chart2.Series[0].YValueMembers = "TotalPrice";
            this.chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var q1 = from d in this.dbContext.Order_Details.AsEnumerable()
                     group d by d.Order.OrderDate.Value.Year into g
                     select new
                     {
                         Mykey = g.Key,
                         TotalPrice = $"{g.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount)):c2}"

                     };

            this.chart1.DataSource = q1.ToList();
            chart1.Series[0].XValueMember = "MyKey";    //分成小中大，抓key的值
            chart1.Series[0].YValueMembers = "TotalPrice";
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            var q2 = from d in this.dbContext.Order_Details.AsEnumerable()
                     group d by d.Order.OrderDate.Value.Year into g
                     select new
                     {
                         Mykey = g.Key,                         
                         TotalPrice = g.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount))
                     };
            //List<int>
            
            dataGridView1.DataSource = q2.ToList();

            //this.chart1.DataSource = q1.ToList();
            //chart1.Series[1].XValueMember = "MyKey";    //分成小中大，抓key的值
            //chart1.Series[1].YValueMembers = "TotalPrice";
            //chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
        }
        
    }
}
