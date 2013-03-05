using BooruDownloader;
using booruGui__Modern_;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
//using serverLoader;
namespace booruGui__Modern_.Pages
    {
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
        {
        // TextWriter _writer;
        //private CustomConfigurationFileReader serverConfigFile = new CustomConfigurationFileReader("");
        //private ConfigurationManager 
        public serverLoader Loader = new serverLoader(ConfigurationManager.AppSettings);
        public delegate void voidDelegate();
        public Home()
            {

            InitializeComponent();
            foreach (SiteData site in Loader.Sites)
                {
                ComboBoxItem item_to_add = new ComboBoxItem();
                item_to_add.Name = site.SITE_NAME;
                item_to_add.ToolTip = site.SITE_NAME;
                // item.
                item_to_add.Content = site.SITE_NAME;
                cmbSourceServer.Items.Add(item_to_add);
                }
            }

        private void Button_Click_1(object sender, RoutedEventArgs e)
            {
            if (txtTags.Text == "")
                {
                txtTags.Focus();
                }
            else
                {
                String[] matches = Regex.Matches(txtTags.Text, @""".*?""|[^\s]+").Cast<Match>().Select(m => m.Value).ToArray();
                BooruDownloader.danbooruDownloader Downloader = new BooruDownloader.danbooruDownloader(Loader[cmbSourceServer.SelectedIndex], matches);
                //booruDelegate
                voidDelegate startDownloader = new voidDelegate(Downloader.startDownloader);
                //Downloader.startDownloader();
              //  System.Threading.ThreadStart ts = new System.Threading.ThreadStart(startDownloader);
               // System.Threading.Thread t = new System.Threading.Thread(ts);
                //t.Start();
                startDownloader();
                }

            }

        private void cmbSourceServer_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
            {
            //ComboBox origin = (ComboBox)sender;
            //origin.Items.
            }
        }
    }
