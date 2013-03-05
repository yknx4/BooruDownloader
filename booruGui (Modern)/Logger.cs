using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booruGui__Modern_
{
    class Logger
    {
        Logger() { }
        List<string> log = new List<string>();
        public void WriteLine(object input)
        {
            log.Add(input.ToString());
        }
        public string this[int sitePosition]
        {
            get
            {
                // This indexer is very simple, and just returns or sets 
                // the corresponding element from the internal array. 
                if (sitePosition > log.Count)
                {
                    throw new ArgumentOutOfRangeException("sitePosition");
                }
                return log[sitePosition];
            }
            private set
            {
                log.Add(value);
            }
        }
    }
}
