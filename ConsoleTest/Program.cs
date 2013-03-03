using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BooruDownloader;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Console.BackgroundColor=ConsoleColor.White;
	Console.ForegroundColor=ConsoleColor.Black;*/
            foreach (string v in args)
            {
                Console.WriteLine(v);
            }
            
            Console.Title = "Batch image downloader";
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
            BooruDownloader.danbooruDownloader Downloader = new BooruDownloader.danbooruDownloader(SiteData,args);
            Downloader.startDownloader();
#if DEBUG
            Console.ReadKey();
#endif // _DEBUG
        }
    }
}
