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
            LoadComobox();

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


        private void LoadComobox()
        {
            string[] list = {"共幾個 學員成績 ?	","找出 前面三個 的學員所有科目成績","找出 後面兩個 的學員所有科目成績",
                           "找出 Name 'aaa','bbb','ccc' 的學員國文英文科目成績","找出學員 'bbb' 的成績",
                           "找出除了 'bbb' 學員的學員的所有成績 ('bbb' 退學)",
                           "找出 'aaa', 'bbb' 'ccc' 學員 國文數學兩科 科目成績","數學不及格 ... 是誰"
             };
            comboBox1.DataSource = list;
        }

        private void button36_Click(object sender, EventArgs e)
        {
            Clear();
            int index = comboBox1.SelectedIndex;

            switch (index)
            {
                case 0:
                    MessageBox.Show($"共{ students_scores.Count()}個學員");
                    break;
                case 1:  // 找出 前面三個 的學員所有科目成績
                    dataGridView1.DataSource = students_scores.Select(p => new { p.Name, p.Chi, p.Eng, p.Math }).Take(3).ToList();
                    break;
                case 2:// 找出 後面兩個 的學員所有科目成績	
                    dataGridView1.DataSource = students_scores.Skip(students_scores.Count - 2).Take(2).ToList(); ;
                    break;
                case 3:// 找出 Name 'aaa','bbb','ccc' 的學員國文英文科目成績	
                    dataGridView1.DataSource = students_scores.Where(p => p.Name == "aaa" || p.Name == "bbb" || p.Name == "ccc")
                        .Select(p => new { p.Name, p.Chi, p.Eng }).ToList();
                    break;
                case 4:// 找出學員 'bbb' 的成績	
                    dataGridView1.DataSource = students_scores.Where(p => p.Name == "bbb")
                        .Select(c=>new { c.Name,c.Chi,c.Math,c.Eng}).ToList();
                    break;
                case 5:// 找出除了 'bbb' 學員的學員的所有成績 ('bbb' 退學)	
                    dataGridView1.DataSource = students_scores.Where(p => p.Name != "bbb")
                        .Select(c => new { c.Name, c.Chi, c.Math, c.Eng }).ToList();
                    break;
                case 6:// 找出 'aaa', 'bbb' 'ccc' 學員 國文數學兩科 科目成績
                    dataGridView1.DataSource = students_scores.Where(p => p.Name == "aaa" || p.Name == "bbb" || p.Name == "ccc")
                        .Select(c => new { c.Name, c.Chi, c.Math }).ToList();
                    break;
                case 7:// 數學不及格 ... 是誰
                    dataGridView1.DataSource = students_scores.Where(p => p.Math < 60)
                         .Select(c => new { c.Name, c.Math }).ToList();
                    break;
            }
        }              
        
  
        private void button37_Click(object sender, EventArgs e)
        {
            //個人 sum, min, max, avg
            dataGridView1.DataSource = students_scores.Select(p => new
            {
                Name = p.Name,
                Sum = p.Chi + p.Eng + p.Math,
                Min=(p.Chi>p.Eng?p.Chi:p.Eng)>(p.Eng>p.Math?p.Eng:p.Math)? (p.Chi > p.Eng ? p.Chi : p.Eng): (p.Eng > p.Math ? p.Eng : p.Math),
                Max = (p.Chi < p.Eng ? p.Chi : p.Eng) < (p.Eng < p.Math ? p.Eng : p.Math) ? (p.Chi < p.Eng ? p.Chi : p.Eng) : (p.Eng < p.Math ? p.Eng : p.Math),
                Avg =( p.Chi + p.Eng + p.Math)/3
            }).ToList();

            //==========================================
            //各科 sum, min, max, avg
            var Chi = students_scores.Select(p => new { Subject = "Chinese" }).GroupBy(p => p.Subject)
                .Select(c => new
                {
                    Subject=c.Key,
                    Sum = students_scores.Sum(s => s.Chi),
                    Min = students_scores.Min(s => s.Chi),
                    Max = students_scores.Max(s => s.Chi),
                    Avg = $"{students_scores.Average(s => s.Chi):f2}",
                }).ToList();

            var Eng = students_scores.Select(p => new { Subject = "English" }).GroupBy(p => p.Subject)
                .Select(c => new
                {
                    Subject=c.Key,
                    Sum=students_scores.Sum(s=>s.Eng),
                    Min=students_scores.Min(s=>s.Eng),
                    Max = students_scores.Max(s => s.Eng),
                    Avg = $"{students_scores.Average(s => s.Eng):f2}",
                }).ToList();

            var Math = students_scores.Select(p => new { Subject = "Math" }).GroupBy(p => p.Subject)
                .Select(c => new
                {
                    Subject = c.Key,
                    Sum = students_scores.Sum(s => s.Math),
                    Min = students_scores.Min(s => s.Math),
                    Max = students_scores.Max(s => s.Math),
                    Avg = $"{students_scores.Average(s => s.Math):f2}",
                }).ToList();

            Chi.Add(Eng[0]);    //把這個集合資料行加入到第一個集合的最後面
            Chi.Add(Math[0]);
            dataGridView2.DataSource = Chi;

        }
        Random r = new Random();
        List<Student> stu;
        private void button33_Click(object sender, EventArgs e)
        {
            // split=> 分成 三群 '待加強'(60~69) '佳'(70~89) '優良'(90~100) 
            // print 每一群是哪幾個 ? (每一群 sort by 分數 descending)
            stu = new List<Student>();
            for(int i = 1; i < 101; i++)
            {
                stu.Add(new Student { Name = i.ToString(), Chi = r.Next(50, 101)});
            }
            dataGridView1.DataSource = stu.Select(p => new
            {
                Name = p.Name,
                Level = CheckScore(p.Chi),
                Chinese_Score = p.Chi
            }).OrderByDescending(p => p.Chinese_Score).ToList();

        }
        private string CheckScore(int s)  //因為回傳輸出為字串 ，所以用string
        {
            if (s >= 90) return "優良";
            else if (s >= 70 && s < 90) return "佳";
            else if (s >= 60 && s < 70) return "待加強";
            else return "不及格";
        }

        private void button35_Click(object sender, EventArgs e)
        {
            // 統計 :　所有隨機分數出現的次數/比率; sort ascending or descending
            #region
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
            #endregion
            dataGridView2.DataSource = stu.GroupBy(p => p.Chi).Select(p => new
            {
                Score = p.Key,
                Count = p.Count(),
                Percentage = $"{((double)p.Count() /stu.Count )*100:f2}%"
            }).OrderByDescending(p => p.Score).ToList();
        }

        private void button34_Click(object sender, EventArgs e)
        {
            //var Q = dbContext.Order_Details.AsEnumerable().GroupBy(p => p.Order.OrderDate.Value.Year).Select(g => new
            //{
            //    Year = g.Key,
            //    Max = $"{ g.Max(q => q.Order.Order_Details.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount))):c2}",
            //    Min = $"{g.Min(q => q.Order.Order_Details.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount))):c2}",
            //}).ToList();
            // 年度最高銷售金額 年度最低銷售金額        
            var q1 = from d in this.dbContext.Order_Details.AsEnumerable()
                     group d by d.Order.OrderDate.Value.Year into g
                     //orderby g.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount))  descending
                     select new
                     {
                         Year = g.Key,
                         Max = $"{ g.Max(q => q.Order.Order_Details.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount))):c2}",
                         Min = $"{g.Min(q => q.Order.Order_Details.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount))):c2}",
                     };
            this.dataGridView1.DataSource = q1.ToList();
            //listBox1.Items.Add($"年度最高銷售金額:{Q[0].Max} 年度最低銷售金額 :{Q[0].Min}");
      
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
            this.dataGridView2.DataSource = q3.ToList();
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
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            var q2 = from d in this.dbContext.Order_Details.AsEnumerable()
                     group d by d.Order.OrderDate.Value.Year into g
                     select new
                     {
                         Mykey = g.Key,                         
                         TotalPrice = g.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount))
                     };
            
            
            dataGridView1.DataSource = q2.ToList();

            //this.chart1.DataSource = q1.ToList();
            //chart1.Series[1].XValueMember = "MyKey";    //分成小中大，抓key的值
            //chart1.Series[1].YValueMembers = "TotalPrice";
            //chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
        }
        public void Clear()
        {
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            chart1.DataSource = null;
            chart2.DataSource = null;
        }

        
    }
}
