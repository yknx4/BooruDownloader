using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BooruDownloader;

namespace booruGui__WFA_
    {
    public partial class Form1 : Form
        {
        danbooruDownloader Downloader = new danbooruDownloader();
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
            //Console.WriteLine("Now redirecting output to the text box");
            
            
            }

        private void button1_Click(object sender, EventArgs e)
            {
            string[] args = { "transparent_background" };
            labelMessage.Text = args[0];
            SiteData SiteData = new SiteData();
            SiteData.SITE_DOMAIN = "http://ichijou.org";
            SiteData.SITE_NAME = "Vectorbooru";
            SiteData.START_PAGE_INDEX = 1;
            SiteData.NUMBER_OF_THREADS = 3;
            SiteData.SEGMENTDEPTH_FOR_ID = 4;
            SiteData.USER_AGENT_STRING = "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/536.5 (KHTML, like Gecko) Chrome/19.0.1084.56 Safari/536.5";
            SiteData.CHECKTAGS_STRING = "<img  class=\"preview    \"";
            SiteData.ACCESSPAGE_STRING = "/post?page=";
            SiteData.POSTTAGS_STRING = "&commit=Search&tags=";
            SiteData.PAGENUMBER_XPATH = "//div[@class='pagination']/a";
            SiteData.POSTLINKS_XPATH = "//span[@class='thumb blacklisted']/a";
            SiteData.IMAGECONTAINER_XPATH = "//div[@class='content']/div/img[@id='image']";
            SiteData.FILEPATH_JOINER = "";
            SiteData.DelayInConnections = false;
            Downloader.startDownloader(args, SiteData);
            }

        
        }
    }
