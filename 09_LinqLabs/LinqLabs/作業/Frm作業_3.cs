using LinqLabs;
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
    public partial class Frm作業_3 : Form
    {
        public Frm作業_3()
        {
            InitializeComponent();
        }       

        private void button4_Click(object sender, EventArgs e)
        {
            this.treeView1.Nodes.Clear();
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            TreeNode node = null;
            node = this.treeView1.Nodes.Add("Small");
            foreach (int n in nums)
            {
                if (n <= 5)
                {
                    node.Nodes.Add(n.ToString());
                }
            }
            node = this.treeView1.Nodes.Add("Medium");
            foreach (int n in nums)
            {
                if (n > 5 && n <= 10)
                {
                    node.Nodes.Add(n.ToString());
                }
            }
            node = this.treeView1.Nodes.Add("Laege");
            foreach (int n in nums)
            {
                if (n > 10)
                {
                    node.Nodes.Add(n.ToString());
                }
            }
        }
        private void button38_Click(object sender, EventArgs e)
        {//DataGridView
            this.dataGridView1.DataSource = null;
            this.dataGridView2.DataSource = null;
            this.treeView1.Nodes.Clear();
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();
            var q = from f in files
                    orderby f.Length descending
                    select f;
            dataGridView1.DataSource = q.ToList();

            var q1 = from f in files
                    group f by MyLength(f) into g
                    select new
                    {
                        Name = g.Key,
                        Count = g.Count(),
                        Group=g
                    };
            dataGridView2.DataSource = q1.ToList();

            //TreeView
            foreach( var group in q1)
            {
                string s = $"{group.Name}({group.Count})";
                TreeNode node = treeView1.Nodes.Add(group.Name.ToString(), s);
                foreach(var item in group.Group)
                {
                    node.Nodes.Add(item.ToString());
                }
            }
        }
        private object MyLength(FileInfo f)
        {
            if (f.Length < 10000) return "small";
            else if (f.Length < 1000000) return "Medium";
            else return "Large";
        }        
        private void button6_Click(object sender, EventArgs e)
        {//DataGridView
            this.dataGridView1.DataSource = null;
            this.dataGridView2.DataSource = null;
            this.treeView1.Nodes.Clear();
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();
            var q = from f in files
                    orderby f.Length descending
                    select f;
            dataGridView1.DataSource = q.ToList();
            var q1 = from f in files
                     orderby f.CreationTime descending
                     group f by f.CreationTime.Year into g                     
                     select new
                     {
                         Name = g.Key,
                         Count = g.Count(),
                         Group = g
                     };          
            dataGridView2.DataSource = q1.ToList();

            //TreeView
           foreach(var group in q1)
            {
                string s = $"{group.Name}({group.Count})";
                TreeNode node = this.treeView1.Nodes.Add(group.Name.ToString(), s);
                foreach(var item in group.Group)
                {
                    node.Nodes.Add(item.ToString());
                }
            }

        }

        //Orders
        NorthwindEntities dbContext = new NorthwindEntities();
        private void button8_Click(object sender, EventArgs e)
        {
            this.treeView1.Nodes.Clear();
            this.dataGridView1.DataSource = dbContext.Products.ToList();
            var q = from p in this.dbContext.Products.AsEnumerable()    //因為跳出了notsupport狀況，故要加入轉型"AsEnumerable()"
                    group p by MyPrice(p.UnitPrice) into g
                    select new
                    {
                        Name = g.Key,
                        Count = g.Count(),
                        Group =g
                    };
                        
            this.dataGridView2.DataSource = q.ToList();
            //Treeview
            foreach (var group in q)
            {
                string s = $"{group.Name}({group.Count})";
                TreeNode node = this.treeView1.Nodes.Add(group.Name.ToString());
                foreach(var item in group.Group)
                {
                    node.Nodes.Add(item.ProductName.ToString()+"--"+$"{item.UnitPrice:c2}");
                }
            }
        }
        private object MyPrice(decimal? unitPrice)
        {
            if (unitPrice < 30) return "Low Price";
            else if (unitPrice < 100) return "Medium Price";
            else return "High Price";
        }
        private void button15_Click(object sender, EventArgs e)
        {
            this.treeView1.Nodes.Clear();
            this.dataGridView1.DataSource = this.dbContext.Orders.ToList();
            var q = from o in this.dbContext.Orders
                    group o by o.OrderDate.Value.Year into g
                    select new
                    {
                        Year = g.Key,
                        Count = g.Count() ,
                        Group = g
                    };
            this.dataGridView2.DataSource = q.ToList();
            //TreeView
            foreach (var group in q)
            {
                string s = $"{group.Year}({group.Count})";
                TreeNode node = this.treeView1.Nodes.Add(group.Year.ToString(), s);
                foreach (var item in group.Group)
                {
                    node.Nodes.Add(item.OrderID.ToString());
                }

            }

        }
        private void button10_Click(object sender, EventArgs e)
        {
            this.treeView1.Nodes.Clear();
            this.dataGridView1.DataSource = this.dbContext.Orders.ToList();
            var q = from o in this.dbContext.Orders
                    orderby o.OrderDate.Value
                    group o by new { o.OrderDate.Value.Year, o.OrderDate.Value.Month } into g
                    select new
                    {
                        Year = g.Key.Year,
                        Month=g.Key.Month,
                        Count = g.Count(),
                        Group =g
                    };
            this.dataGridView2.DataSource = q.ToList();
            //TreeView
            foreach(var group in q)
            {
                string s = $"{group.Year}({group.Count})";
                TreeNode node = this.treeView1.Nodes.Add(group.Year.ToString(), s);
                foreach(var item in group.Group)
                {
                    node.Nodes.Add(item.OrderID.ToString());
                }

            }
        }
        
        //OrderDetails
        private void button2_Click(object sender, EventArgs e)
        {   //總銷售金額      
            var q1 = (from d in this.dbContext.Order_Details.AsEnumerable()
                      select d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount)).Sum();

            MessageBox.Show($"總銷售金額為 : {q1:c2} 元");

            var q2 = from d in this.dbContext.Order_Details.AsEnumerable()
                     group d by d.Order.OrderDate.Value.Year into g
                     select new
                     {
                         g.Key,
                         TotalPrice = $"{g.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount)):c2}"
                     };
            this.dataGridView2.DataSource = q2.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {   //銷售最好的top 5業務員    
            var q1 = (from d in this.dbContext.Order_Details.AsEnumerable()
                      group d by new { d.Order.EmployeeID, d.Order.Employee.FirstName, d.Order.Employee.LastName } into g
                      orderby g.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount)) descending
                      select new
                      {
                          g.Key.EmployeeID,
                          g.Key.FirstName,
                          g.Key.LastName,
                          TotalPrice = $"{g.Sum(n => n.UnitPrice * n.Quantity * (decimal)(1 - n.Discount)):c2}"
                      }).Take(5);

            dataGridView1.DataSource = q1.ToList();
        }
        //Product
        private void button9_Click(object sender, EventArgs e)
        {//     NW 產品最高單價前 5 筆 (包括類別名稱)
            var q = (from p in this.dbContext.Products
                    orderby p.UnitPrice descending
                    select new
                    {
                        p.ProductID,
                        p.ProductName,
                        p.Category.CategoryName,
                        p.UnitPrice
                    }).Take(5);
            this.dataGridView1.DataSource = q.ToList();
        }
        private void button7_Click(object sender, EventArgs e)
        {   //     NW 產品有任何一筆單價大於300 ?
            //var q = (from p in this.dbContext.Products
            //        orderby p.UnitPrice descending
            //        select p.UnitPrice).Any(n=>n>300);
            var q = from p in this.dbContext.Products
                    orderby p.UnitPrice descending
                    select p.UnitPrice;
            bool result;
            result = q.Any(n => n > 300);

            MessageBox.Show(result.ToString());

            var q1 = from p in this.dbContext.Products
                     orderby p.UnitPrice descending
                     select p.UnitPrice;
            dataGridView1.DataSource = q1.ToList();
        }
    }
}
