using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace IDELANG
{
    public partial class Form1 : Form
    {
        string inicial=Application.StartupPath+@"\dist";
        string direction = "";
        bool guardado = false;
        bool abierto = false;
        bool exists = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            direction = "";
            textBox1.Text = "";
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                StreamReader sr = new StreamReader(openFileDialog1.FileName);
                direction = openFileDialog1.FileName;
                textBox1.Text= sr.ReadToEnd();
                abierto = true;
                if (File.Exists(direction + @"\dist\Basic.exe"))
                {
                    exists = true;
                }
                else {
                    exists = false;
                }
                direction = openFileDialog1.FileName;
                sr.Close();
            }
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (direction != "")
                {
                    StreamWriter sw = new StreamWriter(direction + @"\a.jrnm");
                    sw.Write(textBox1.Text);
                    guardado = true;
                    sw.Close();
                }
                else
                {
                    guardarComoToolStripMenuItem_Click(sender, e);
                }
            }
            else {
                MessageBox.Show("Ningún código", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            if (textBox1.Text != "")
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    guardado = true;
                    direction = saveFileDialog1.FileName + @"\dist";
                    Directory.CreateDirectory(direction);
                    DirectoryCopy(inicial, direction, true);
                    StreamWriter sw = new StreamWriter(direction + @"\a.jrnm");
                    direction = saveFileDialog1.FileName;
                    sw.Write(textBox1.Text);
                    sw.Close();
                }
            }
            else
            {
                MessageBox.Show("Ningún código", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private static void DirectoryCopy(string de, string para, bool csd) {
            DirectoryInfo di = new DirectoryInfo(de);
            if (!di.Exists) {
                MessageBox.Show("Error");
            }
            DirectoryInfo[] dirs = di.GetDirectories();
            FileInfo[] files = di.GetFiles();
            foreach (FileInfo file in files) {
                string temppath = Path.Combine(para, file.Name);
                file.CopyTo(temppath, false);
            }
            if (csd)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(para, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, csd);
                }
            }
        }

        private void correrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (guardado || abierto || exists)
                {
                    if (File.Exists(direction + @"\dist\Basic.exe"))
                    {
                        ProcessStartInfo info = new ProcessStartInfo();

                        info.UseShellExecute = true;
                        info.FileName = "Basic.exe";
                        info.WorkingDirectory = direction + @"\dist\";
                        Process.Start(info);
                    }
                }
                else
                {
                    guardarComoToolStripMenuItem_Click(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Ningún código","Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
