using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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

namespace booruGui__WFA_
    {
    public class TextBoxStreamWriter : TextWriter
        {
        RichTextBox _output = null;

        public TextBoxStreamWriter(RichTextBox output)
            {
            _output = output;
            }

        public override void Write(char value)
            {
            base.Write(value);
            _output.AppendText(value.ToString()); // When character data is written, append it to the text box.
            }

        public override Encoding Encoding
            {
            get { return System.Text.Encoding.UTF8; }
            }
        }
    }
