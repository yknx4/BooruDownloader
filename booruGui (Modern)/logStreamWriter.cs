using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booruGui__Modern_
    {
    
        public class logStreamWriter : StreamWriter
            {
            public logStreamWriter(Stream stream)
                : base(stream)
                {

                }
            public logStreamWriter(Stream stream, Encoding encoding)
                : base(stream, encoding)
                {

                }
            public logStreamWriter(Stream stream, Encoding encoding, int bufferSize)
                : base(stream, encoding, bufferSize)
                {

                }
            public logStreamWriter(string path)
                : base(path)
                {

                }
            public logStreamWriter(string path, bool append)
                : base(path, append)
                {

                }
            public logStreamWriter(string path, bool append, Encoding encoding)
                : base(path, append, encoding)
                {

                }
            public logStreamWriter(string path, bool append, Encoding encoding, int bufferSize)
                : base(path, append, encoding, bufferSize)
                {

                }

            public override void Write(string value)
                {
                base.Write(value);
                }
            
        }
    }
