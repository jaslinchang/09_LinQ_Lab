using LinqLabs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


//entity data model 特色
//1. App.config 連接字串
//2. Package 套件下載, 參考 EntityFramework.dll, EntityFramework.SqlServer.dll
//3. 導覽屬性 關聯
//物件關聯對映（英語：Object Relational Mapping，簡稱ORM，或O / RM，或O / R mapping），


//4. DataSet model 需要處理 DBNull; Entity Model  不需要處理 DBNull (DBNull 會被 ignore)
//5. IQuerable<T> query 執行時會轉成 => T-SQL

namespace Starter
{
    public partial class FrmLinq_To_Entity : Form
    {
        public FrmLinq_To_Entity()
        {
            InitializeComponent();
            dbContext.Database.Log = Console.Write;   //因為此為視窗程式， Console.Write會在下面的輸出視窗顯示，呈現C#轉T-SQL的
        }

        NorthwindEntities dbContext = new NorthwindEntities();  //可以不用再Fill資料、且會自動跳過空值

        private void button1_Click(object sender, EventArgs e)
        {
            var q = from p in dbContext.Products
                    where p.UnitPrice > 30
                    select p;
            dataGridView1.DataSource = q.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource= dbContext.Categories.First().Products.ToList();   //找出類別的第一筆的所有產品
            MessageBox.Show(dbContext.Products.First().Category.CategoryName);   //找出產品的第一筆的類別名稱
        }

        private void button3_Click(object sender, EventArgs e)
        {   //用預存程序，抓指定日期到現在的
            dataGridView1.DataSource = dbContext.Sales_by_Year(new DateTime(1996, 1, 1), DateTime.Now).ToList();

        }

        private void button22_Click(object sender, EventArgs e)
        {
            //System.NotSupportedException: 'LINQ to Entities 無法辨識方法 'System.String Format(System.String, System.Object)' 方法，
            //而且這個方法無法轉譯成存放區運算式。'

            var q = from p in this.dbContext.Products.AsEnumerable()     //如遇到上面的notsupport狀況，就加入轉型"AsEnumerable()"
                    orderby p.UnitsInStock descending, p.ProductID  //先以庫存排序，若一樣再以ID排序
                    select new
                    {
                        p.ProductID,
                        p.ProductName,
                        p.UnitPrice,
                        p.UnitsInStock,
                        TotalPrice = $"{ p.UnitPrice * p.UnitsInStock:c2}"
                    };
            dataGridView1.DataSource = q.ToList();

            var q1 = dbContext.Products.OrderByDescending(p => p.UnitsInStock).ThenBy(w => w.ProductID);
            dataGridView2.DataSource = q1.ToList();
        }
        private void button23_Click(object sender, EventArgs e)
        { //自訂 compare logic
            var q3 = dbContext.Products.AsEnumerable().OrderBy(p => p, new MyComparer()).ToList();
            this.dataGridView2.DataSource = q3.ToList();
        }

        class MyComparer : IComparer<Product>
        {

            public int Compare(Product x, Product y)
            {
                if (x.UnitPrice < y.UnitPrice)
                    return -1;
                else if (x.UnitPrice > y.UnitPrice)
                    return 1;
                else
                    return string.Compare(x.ProductName[0].ToString(), y.ProductName[0].ToString(), true);

            }
        }

        private void button20_Click(object sender, EventArgs e)
        {       //太T-SQL了
            var q = from c in dbContext.Categories
                    join p in dbContext.Products
                    on c.CategoryID equals p.CategoryID
                    select new { c.CategoryID, c.CategoryName, p.ProductName, p.UnitPrice };

            dataGridView1.DataSource = q.ToList();
        }     

        private void button21_Click(object sender, EventArgs e)
        {   //select many   ==inner join
            var q = from c in dbContext.Categories
                    from p in c.Products
                    select new
                    {
                        c.CategoryID,
                        c.CategoryName,
                        p.ProductName,
                        p.UnitPrice
                    };
            dataGridView2.DataSource = q.ToList();
            //=========================================
            //this.dbContext.Categories.SelectMany(c => c.Products, (c, p) => new { c.CategoryID, c.CategoryName, p.ProductID, p.UnitPrice});

            //cross join
            //他會變成每抓一筆類別的就會抓所有產品一次
            var q2 = from c in this.dbContext.Categories
                     from p in this.dbContext.Products
                     select new { c.CategoryID, c.CategoryName, p.ProductID, p.UnitPrice, p.UnitsInStock };
            MessageBox.Show("q2.count() =" + q2.Count());
            this.dataGridView3.DataSource = q2.ToList();
        }
        private void button16_Click(object sender, EventArgs e)
        {   //物件化查詢
            var q = from p in dbContext.Products
                    select new
                    {
                        p.CategoryID,
                        p.Category.CategoryName,
                        p.ProductName,
                        p.UnitPrice
                    };
           dataGridView3.DataSource = q.ToList();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var q = from p in dbContext.Products
                    group p by p.Category.CategoryName into g
                    select new
                    {
                        CategoryName = g.Key,
                        AvgUnitprice = g.Average(p => p.UnitPrice)
                    };
            dataGridView1.DataSource = q.ToList();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //bool? b;      
            //b = true;
            //b = false;
            //b = null;

            var q = from o in dbContext.Orders
                    group o by o.OrderDate.Value.Year into g
                    select new
                    {
                        Year = g.Key,
                        Count = g.Count()
                    };
            dataGridView1.DataSource = q.ToList();
            //====================
            var q1 = from o in dbContext.Orders
                     group o by new { o.OrderDate.Value.Year, o.OrderDate.Value.Month } into g
                     select new { Year = g.Key, Count = g.Count() };
            dataGridView2.DataSource = q1.ToList();
        }

        private void button55_Click(object sender, EventArgs e)
        {  //Insert
            Product prod = new Product { ProductName = "Test"+DateTime.Now.ToString(), Discontinued = true };
            this.dbContext.Products.Add(prod);  //此時還只在記憶體內做
            this.dbContext.SaveChanges();  //這邊才真正加入資料庫 
            Read_RefreshDataGridView();  //呼叫更新資料庫的方法
        }

        private void button56_Click(object sender, EventArgs e)
        {   //update
            var q = (from p in dbContext.Products
                     where p.ProductName.Contains("Test")
                     select p).FirstOrDefault();

            if (q == null) return;   //離開方法

            q.ProductName = "Test" + q.ProductName;
            this.dbContext.SaveChanges();

            Read_RefreshDataGridView();
        }
        private void button53_Click(object sender, EventArgs e)
        {   //delete
            var q = (from p in dbContext.Products
                     where p.ProductName.Contains("Test")
                     select p).FirstOrDefault();

            if (q == null) return;

            this.dbContext.Products.Remove(q);
            this.dbContext.SaveChanges();

            Read_RefreshDataGridView();
        }
        void Read_RefreshDataGridView()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = this.dbContext.Products.ToList();
        }

      
    }
}
