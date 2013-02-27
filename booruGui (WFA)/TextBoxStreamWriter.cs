using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

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
