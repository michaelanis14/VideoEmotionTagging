using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proj2
{

    public partial class Form1 : Form
    {
        private static string filename;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string strfilename = openFileDialog1.FileName;
                   // MessageBox.Show(strfilename);
                    filename = strfilename;
                    //textBox1.Text = strfilename;
                }
                this.Close();
            }
            catch (Exception e1)
            { Console.Write("error baby"); }
        }
        public static string getFilename()
        {
            return filename;
        }
        //public string TheValue
        //{
        //   // get { return textBox1.Text; }
        //}
    }
}
