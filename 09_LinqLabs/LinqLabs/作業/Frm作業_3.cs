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
            else return "Laege";
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
        
    }
}
