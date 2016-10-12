using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoogleMapCrawler;

namespace CombineSqlLite
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select file";
            dialog.InitialDirectory = Application.StartupPath;
            dialog.Filter = "Data Base File (*.db)|*.db";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Add(dialog.FileName);          
            }
        }
              

        private void button3_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Data Base File (*.db)|*.db";
            saveFileDialog1.Title = "Save an File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                SQLliteHelp.Instance.Init(saveFileDialog1.FileName);
                SQLliteHelp.Instance.CreateTable(saveFileDialog1.FileName);

                listBox2.Items.Add(String.Format("Save to {0}", saveFileDialog1.FileName));

                SQLliteHelp.Instance.Open(saveFileDialog1.FileName);
                

                foreach (String path in listBox1.Items)
                {
                    listBox2.Items.Add(String.Format("Begin load {0}", path));
                    List<MapInfo> list = SQLliteHelp.Instance.GetAllData(path);
                    listBox2.Items.Add(String.Format("After load {0}", path));
                    Application.DoEvents();
                    SQLliteHelp.Instance.BeginTransaction();
                    foreach (MapInfo info in list)
                    {
                        SQLliteHelp.Instance.AddAndUpdate(info);
                        Application.DoEvents();
                    }
                    SQLliteHelp.Instance.Commit();
                    Application.DoEvents();
                    listBox2.Items.Add(String.Format("Combine done {0}", path));
                }
                SQLliteHelp.Instance.Close();

            }

           
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }
}
