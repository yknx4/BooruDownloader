using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using booruGui__WFA_;

namespace booruGui__Modern_.Pages
    {
    /// <summary>
    /// Interaction logic for log.xaml
    /// </summary>
    public partial class log : UserControl
        {
        public TextWriter _writer;
        public log()
            {
            _writer = new TextBoxStreamWriter(txtLog);
            Console.SetOut(_writer);
            InitializeComponent();
            }
        }
    }
