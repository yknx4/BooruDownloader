using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace booruGui__WFA_
    {
    public partial class Form1 : Form
        {
        TextWriter _writer = null;
        public Form1()
            {
            InitializeComponent();
            }

        private void Form1_Load(object sender, EventArgs e)
            {
             // Instantiate the writer
             _writer = new TextBoxStreamWriter(txtConsole);
             // Redirect the out Console stream
            Console.SetOut(_writer);
            Console.WriteLine("Now redirecting output to the text box");
            }
        }
    }
