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
using System.Configuration;
using System.Collections.Specialized;
using booruGui__Modern_;
using booruGui__TextBoxStreamWriter_;

namespace booruGui__Modern_.Pages
    {
    /// <summary>
    /// Interaction logic for log.xaml
    /// </summary>
    public partial class log : UserControl
        {
        public static TextBoxStreamWriter _writer;
        TextRange _content;
        public log()
            {
           
            InitializeComponent();
            _writer = new TextBoxStreamWriter(txtLog);
            Console.SetOut(_writer);
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            Console.WriteLine();
            //logPage.txtLog.WriteLine("Using AppSettings property.");
            Console.WriteLine("Application settings:");

            if (appSettings.Count == 0)
                {
                Console.WriteLine("[ReadAppSettings: {0}]",
                "AppSettings is empty Use GetSection command first.");
                }
            for (int i = 0; i < appSettings.Count; i++)
                {
                Console.WriteLine("#{0} Key: {1} Value: {2}",
                  i, appSettings.GetKey(i), appSettings[i]);
                }
            }

        private void clickbtn(object sender, RoutedEventArgs e)
            {
            Console.WriteLine("test");
            try
            {
              string filename;
             string n = string.Format("-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
              filename = "log" + n + "log";
               _content = new TextRange(txtLog.Document.ContentStart, txtLog.Document.ContentEnd);
              FileStream outFile = new FileStream(filename, FileMode.Create);
              _content.Save(outFile, System.Windows.DataFormats.Text);
              lblMessage.Content = filename + " saved.";
              lblMessage.Visibility = Visibility.Visible;
            }
            catch (System.Exception ex)
            {
            Console.WriteLine("Error Saving file {0}",ex);
            }
            
            }

        private void Button_Click_1(object sender, RoutedEventArgs e)
            {
            txtLog.Document.Blocks.Clear();
            //serverLoader Loader2 = new serverLoader(ConfigurationManager.AppSettings);
            }
        }
    }
