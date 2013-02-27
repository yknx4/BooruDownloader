using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

public class CombinedWriter : StreamWriter
{
    TextWriter console;
    public CombinedWriter(string path, bool append, Encoding encoding, int bufferSize, TextWriter console)
        : base(path, append, encoding, bufferSize)
    {
        this.console = console;
    }
    public override void Write(string value)
    {
        console.Write(value);
        base.Write(value);
    }
    public override void WriteLine(string value)
    {
        Console.WriteLine(value);
        base.WriteLine(value);
    }
}
namespace BooruDownloader
{
    public class PostData
    {
        public PostData(){}
        public string link;
        public string tags;
        public int id;
    }
  public class SiteData
    {
        public SiteData(){}
        public uint START_PAGE_INDEX;
        public int NUMBER_OF_THREADS;
        public int SEGMENTDEPTH_FOR_ID;
        public string SITE_DOMAIN;
        public string SITE_NAME;
        public string USER_AGENT_STRING;
        public string CHECKTAGS_STRING;
        public string ACCESSPAGE_STRING;
        public string POSTTAGS_STRING;
        public string PAGENUMBER_XPATH;
        public string POSTLINKS_XPATH;
        public string IMAGECONTAINER_XPATH;
        public string FILEPATH_JOINER;
        public bool DelayInConnections = false;
        public bool tagEvaluation;
        public int DelayTime = 500;
        public int PID_MULT = 1;
    }
  //  class ThreadGroup { }
   public class danbooruDownloader
    {
       public danbooruDownloader() { }
        protected CombinedWriter OutputLog;
        protected string[] tags;
        protected SiteData site;
        protected HtmlDocument HTMLDoc = new HtmlDocument();
        public string Error;
        protected static Regex digitsOnly = new Regex("[^0-9]");
        public static WebClient Reader = new WebClient();
        protected string getRawHtml(Uri url)
        {
            string OutputHTML;

#if DEBUG
            OutputLog.WriteLine("getRawHtml called with "+url);
#endif
 
        
            try
            {
                OutputHTML = Reader.DownloadString(url);
            }
            catch (WebException ex)
            {
                Error = "GetRawHtml failed because " + ex;
                OutputLog.WriteLine(Error);
                return null;
            }
            catch (ArgumentNullException ex)
            {
                Error = "You have to have an input " + ex;
                OutputLog.WriteLine(Error);
                throw ex;
            }
            return OutputHTML;
        }
        protected string readSite(uint page)
        {
            /* if (page == null)
             {
                 throw new ArgumentOutOfRangeException();
             }*/
            Uri URL;
            string Page_Data = null;//RAW Html string initialization
            String TmpUrl = site.SITE_DOMAIN + site.ACCESSPAGE_STRING + (page * site.PID_MULT).ToString() + site.POSTTAGS_STRING;//URL String initialization
            foreach (string tag in tags)//Cycle to add each tag to URL
            {
                TmpUrl += tag + "+";//each tags must be separated by '+' so each tag is added to URL after '+'
            }
            TmpUrl = TmpUrl.Remove(TmpUrl.Length - 1);//removes the extra '+' from URL output
            URL = new Uri(TmpUrl);
            try
            {
                Page_Data = getRawHtml(URL);
            }
            catch (ArgumentNullException e)
            {

                OutputLog.WriteLine("URL Must have a value");
                OutputLog.WriteLine("Error: " + e);
            }
            if (Page_Data == null) //In case of Webexception the following code is run
            {
                OutputLog.WriteLine("Cannot connect to Host");
                OutputLog.WriteLine("Is Internet connection alive?");
            }
            return Page_Data; //Returns RAW HTLM string    
        }
        protected HtmlNodeCollection getHtmlNodes(string RawHtml, string Xpath)
        {
            /*if (RawHtml == null || Xpath == null)
            {
                throw new ArgumentNullException();
            }*/
            try
            {
                //initialize HTML Documents
                HTMLDoc.LoadHtml(RawHtml);//load Html from RAW Html string
                HtmlNode nodo_p = HTMLDoc.DocumentNode; //Get Nodes from HTML Doc
                return nodo_p.SelectNodes(Xpath); //Select all nodes with direct image link
            }
            catch (ArgumentNullException e)
            {

                OutputLog.WriteLine("GetHtmlNodes failed because " + e);

                return null;
            }
        }
        public virtual PostData[] getDownloadData(uint page)
        {
            string RawHtml = null;
            HtmlNodeCollection HtmlNodes;
            try
            {
                RawHtml = readSite(page);
            }
            catch (ArgumentOutOfRangeException e)
            {

                OutputLog.WriteLine("Page must be positive");
                OutputLog.WriteLine("Error: " + e);
            }

            if (RawHtml == null)
            {
                throw new ArgumentNullException();
            }

            HtmlNodes = getHtmlNodes(RawHtml, site.POSTLINKS_XPATH);
            if (HtmlNodes == null || HtmlNodes.Count == 0)
            {
                OutputLog.WriteLine("No nodes were found with the specified XPath");
                return null;
            }
#if DEBUG
            OutputLog.WriteLine("Total nodes in GetDownloadLink are: " + HtmlNodes.Count);
#endif // DEBUGGING
            PostData[] LinkData = new PostData[HtmlNodes.Count];
            int nodecount = 0;
            foreach (HtmlNode a_refs in HtmlNodes)
            {
                if (site.DelayInConnections) Thread.Sleep(site.DelayTime);
                PostData a_r_data = null;
                try
                {
                    a_r_data = GetPostData(site.SITE_DOMAIN + site.FILEPATH_JOINER + a_refs.GetAttributeValue("href", ""));
                }
                catch (ArgumentNullException e)
                {
                    OutputLog.WriteLine("Gotta give a valid url");
                    OutputLog.WriteLine("Error: " + e);
                }
                LinkData[nodecount] = a_r_data;
                //OutputLog.WriteLine(Links[nodecount]);
                nodecount++;
            }
#if DEBUG
            OutputLog.WriteLine("The total nodes processed are " + nodecount);
#endif // DEBUGGING
            return LinkData;
        }
        public virtual void DownloadFiles(object data)
        {
            if (data != null)
            {
                uint Page = (uint)data; //Convert input object point to a ReadYandereParameters Struct pointer and direct it to object
                PostData[] Links = getDownloadData(Page);
                if (Links != null)
                {
                    int LinksCount = Links.Length;
                    OutputLog.WriteLine("Downloading " + LinksCount + " files."); //Write the number of downloadable files
                    int TryCount = 0;
                    for (int i = 0; i < LinksCount; i++)
                    {
                        if (Links[i] == null)
                        {

                            OutputLog.WriteLine("No data in position " + i);
                            continue;
                        }
                        //OutputLog.WriteLine(Links[i]);
                        Uri Link = new Uri(Links[i].link);

                        string FilePath = ParseFilePath(Links[i].id + " " + Links[i].tags);
                        string FileExtension = ParseFileExtension(Links[i].link, Convert.ToInt32(Links[i].id));
                        FilePath = site.SITE_NAME + site.FILEPATH_JOINER + FilePath + FileExtension;
                        if (!(File.Exists(FilePath)))
                        {
                            try
                            {
                                OutputLog.WriteLine("Downloading " + FilePath);
#if !DEBUG
                                Reader.DownloadFile(Link, FilePath);
#endif // !DEBUGGING
                            }
                            catch (WebException e)
                            {
                                if (TryCount < 3)
                                {
                                    OutputLog.WriteLine("Failed to fetch position " + i + "... Attempt " + (TryCount + 1));
                                    OutputLog.WriteLine("Because: " + e);
                                    i -= 1;
                                    continue;
                                }
                                OutputLog.WriteLine("Cannot Download " + Links[i].link);
                                OutputLog.WriteLine("Because " + e);
                            }
                            catch (ArgumentNullException e)
                            {
                                OutputLog.WriteLine("You have to have an input " + e);
                                throw e;
                            }

                        }
                        else
                        {
                            OutputLog.WriteLine(Links[i].id + " Already Exists");
                        }


                    }
                    Thread.Yield();
                }

            }
        }
        public virtual string ParseFilePath(string FilePath)
        {
            #region CORREGIR
            //  FilePath = FilePath.Join("", FilePath.Split(Path.GetInvalidFileNameChars()));
            //FilePath=FilePath.Join("",FilePath.Split(=Path.GetInvalidFileNameChars()));
            #endregion
            FilePath = FilePath.Replace("show ", "");
            FilePath = Uri.UnescapeDataString(FilePath);
            if (FilePath.Length > 140)
            {
                FilePath = FilePath.Substring(0, 140);
            }
            return FilePath;
        }
        public virtual PostData GetPostData(string PostUrl)
        {
            if (PostUrl == null)
            {
                OutputLog.WriteLine("URL Cannot be Null");
                throw new ArgumentNullException();
            }
            Uri Link = new Uri(PostUrl);
            string RawHtml = null;
            try
            {
                RawHtml = getRawHtml(Link);
            }
            catch (ArgumentNullException e)
            {
                OutputLog.WriteLine("Gotta specify an URL");
                OutputLog.WriteLine("Error " + e);
            }

            if (RawHtml == null)
            {
                OutputLog.WriteLine("No HTML Found, giving null");
                return null;
            }
            PostData PostData = new PostData();
            //OutputLog.WriteLine(RawHtml);
            HtmlNodeCollection HtmlNodes = getHtmlNodes(RawHtml, site.IMAGECONTAINER_XPATH);
            //OutputLog.WriteLine("Total nodes in GetPostDirectLink are: "+HtmlNodes.Count);
            if (HtmlNodes != null)
            {
                foreach (HtmlNode img in HtmlNodes)
                {
                    PostData.link = img.GetAttributeValue("src", "false");
                    PostData.tags = img.GetAttributeValue("alt", "false");
#if DEBUG
                    OutputLog.WriteLine("Direct link of post " + PostUrl + " is " + PostData.link);
                    OutputLog.WriteLine("Tags of post " + PostUrl + " are " + PostData.tags);
#endif // DEBUGGING
                }
                string IDasString = Link.Segments[site.SEGMENTDEPTH_FOR_ID];
                PostData.id = Convert.ToInt32(digitsOnly.Replace(IDasString, ""));
#if DEBUG
                OutputLog.WriteLine("ID of post " + PostUrl + " is " + PostData.id);
#endif // DEBUGGING
                return PostData;
            }
            return null;
        }
        public virtual string ParseFileExtension(string FileExtension, int ID = 0)
        {
            FileExtension = FileExtension.Replace("jpeg", "jpg");
            FileExtension = FileExtension.Substring(FileExtension.Length - 4, 4);
            return FileExtension;
        }
        public virtual int GetPagesNumber() //Function to get the number of pages the tags have
        {
           string texto = readSite((uint)site.START_PAGE_INDEX);
           OutputLog.WriteLine(texto);//Put RAW HTML in the String Texto
           // string texto = readSite((uint)(site.START_PAGE_INDEX+1));//Put RAW HTML in the String Texto
            if (!(texto == null)) //If the RAW HTML exist, do following code
            {
                string PageTemp = null; // Create string pointer to get inner text from Html Node
                HtmlNodeCollection nodos_a = getHtmlNodes(texto, site.PAGENUMBER_XPATH); //Select 'a' nodes from 'div' with class='pagination'
                try //Try to read nodes
                {
                    nodos_a.RemoveAt(nodos_a.Count - 1); //Remove an extra node that doesn't fullfill our needance
                    foreach (HtmlNode var in nodos_a) // Cycle foreach node
                    {
                        PageTemp = var.InnerText; //Put the data in PageTemp variable
                    }
                    return Convert.ToInt32(PageTemp, 10); //Return the last PageTemp, and so the biggest number, meanning the last page
                }
                catch (NullReferenceException e) //If no div availavable, means it just have 1 page
                {
                    OutputLog.WriteLine("Return 1 page because of " + e);
                    return 1;	//return 1 page
                }
            }
            return 1;//in any case return 1 page, to avoid program crash, further errors are handled after
        }
        public void startDownloader(string[] args, SiteData SiteData)
        {
            Console.Title = SiteData.SITE_NAME + " Batch image downloader";
            bool CheckTags = false;
            if (args == null || args.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                OutputLog.WriteLine("Specify the tags to crawl");
                OutputLog.WriteLine("Usage: " + SiteData.SITE_NAME + " moe suzumiya_haruhi");
                OutputLog.WriteLine("To download pictures with tags moe and suzumiya_haruhi");
                OutputLog.WriteLine("Remember Danbooru-like sites threats spaces as underline score");
            }
            else
            {
                Reader.Headers.Add("user-agent", SiteData.USER_AGENT_STRING);
                OutputLog = new CombinedWriter((SiteData.SITE_NAME + "_log.txt"), false, Encoding.UTF8, 512, Console.Out);
                site = SiteData;
                //tags = tags_content;
                /*Array.Reverse(args);
                Array.Resize(args,args.Length -1);*/
                tags = args;
#if !Moebooru
                CheckTags = readSite(site.START_PAGE_INDEX).Contains(site.CHECKTAGS_STRING);
#endif
#if Moebooru
			CheckTags=readSite(site.START_PAGE_INDEX).Contains("<a class=\"directlink largeimg\"")||readSite(site.START_PAGE_INDEX).Contains("<a class=\"directlink smallimg\"");
#endif
#if DEBUG
                OutputLog.WriteLine("Checking tags.... Returned " + CheckTags);
#endif // DEBUGGING
                if (CheckTags)
                {
                    Thread.Sleep(site.DelayTime);
                    int TotalPages = GetPagesNumber();
                    OutputLog.WriteLine("Will download " + TotalPages + " pages.");
                }
                else
                {
                    OutputLog.WriteLine("Nothing here but us Chickens!");
                }


            }
        }
    }
  public class gelbooruDownloader : danbooruDownloader
    {
        public override PostData GetPostData(string PostUrl)
        {
            if (PostUrl == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                OutputLog.WriteLine("URL Cannot be Null");
                Console.ResetColor();
                throw new ArgumentNullException();
            }
            PostUrl = Uri.UnescapeDataString(PostUrl);
            PostUrl = PostUrl.Replace("&amp;", "&");
#if DEBUG
            OutputLog.WriteLine("GetPostData function called with: " + PostUrl);
#endif
            Uri Link = new Uri(PostUrl);
            string RawHtml = null;
            try
            {
                RawHtml = getRawHtml(Link);
            }
            catch (ArgumentNullException e)
            {
                OutputLog.WriteLine("Gotta specify an URL");
                OutputLog.WriteLine("Error " + e);
            }
            if (RawHtml == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                OutputLog.WriteLine("No HTML Found, giving null");
                Console.ResetColor();
                return null;
            }
            PostData PostData = new PostData();
            //OutputLog.WriteLine(RawHtml);
            HtmlNodeCollection HtmlNodes = getHtmlNodes(RawHtml, site.IMAGECONTAINER_XPATH);
            //OutputLog.WriteLine("Total nodes in GetPostDirectLink are: "+HtmlNodes.Count);
            if (HtmlNodes != null)
            {
                foreach (HtmlNode img in HtmlNodes)
                {
                    PostData.link = img.GetAttributeValue("src", "false");
                    PostData.tags = img.GetAttributeValue("alt", "false");
#if DEBUG
                    OutputLog.WriteLine("Direct link of post " + PostUrl + " is " + PostData.link);
                    OutputLog.WriteLine("Tags of post " + PostUrl + " are " + PostData.tags);
#endif // DEBUGGING
                }
                PostData.id = Convert.ToInt32(PostUrl.Substring((PostUrl.IndexOf("id=") + 3)));
#if DEBUG
                OutputLog.WriteLine("ID of post " + PostUrl + " is " + PostData.id);
#endif // DEBUGGING
                return PostData;
            }
            return null;
        }
        public override string ParseFileExtension(string FileExtension, int ID)
        {
            FileExtension = FileExtension.Replace("jpeg", "jpg");
            FileExtension = FileExtension.Replace("?" + ID, "");
            FileExtension = FileExtension.Substring(FileExtension.Length - 4, 4);
            return FileExtension;
        }
        public override int GetPagesNumber()
        {
            string texto = readSite(site.START_PAGE_INDEX);//Put RAW HTML in the String Texto
            OutputLog.WriteLine(site.START_PAGE_INDEX);
            if (!(texto == null)) //If the RAW HTML exist, do following code
            {
                string PageTemp = null; // Create string pointer to get inner text from Html Node
                HtmlNodeCollection nodos_a = getHtmlNodes(texto, site.PAGENUMBER_XPATH); //Select 'a' nodes from 'div' with class='pagination'
                try //Try to read nodes
                {
                    foreach (HtmlNode var in nodos_a) // Cycle foreach node
                    {
                        PageTemp = var.GetAttributeValue("href", "false"); //Put the data in PageTemp variable
                        //OutputLog.WriteLine(PageTemp);
                        PageTemp = PageTemp.Substring((PageTemp.IndexOf("pid=") + 4));
                        //OutputLog.WriteLine(PageTemp);
                    }
                    return ((Convert.ToInt32(PageTemp, 10) / site.PID_MULT) + 1);
                }
                catch (NullReferenceException e) //If no div availavable, means it just have 1 page
                {
                    OutputLog.WriteLine("Return 1 page because of " + e);
                    return 1;	//return 1 page
                }
            }
            return 1;
        }
    }

  public class moebooruDownloader : danbooruDownloader
    {
        public override void DownloadFiles(Object data)
        {
            if (data != null)
            {
                int Page = (int)data; //Convert input object point to a ReadYandereParameters Struct pointer and direct it to object
                string HtmlContent = readSite((uint)Page); //Get Raw HTML with needed parameters
                if (HtmlContent != null)
                {
                    string TempUrl; //Initialize string to store URL
                    HtmlNodeCollection nodos_a = getHtmlNodes(HtmlContent, "//a[@class='directlink largeimg'] | //a[@class='directlink smallimg']"); ; //Select all nodes with direct image link
                    OutputLog.WriteLine("Downloading " + nodos_a.Count + " files."); //Write the number of downloadable files
                    foreach (HtmlNode var in nodos_a)
                    {
                        if (var == null)
                        {
                            continue;
                        }
                        //OutputLog.WriteLine(Links[i]);
                        TempUrl = var.GetAttributeValue("href", "");
                        Uri Link = new Uri(TempUrl);
                        string FilePath = ParseFilePath(Link.Segments[site.SEGMENTDEPTH_FOR_ID]);
                        string FileExtension = ParseFileExtension(Link.Segments[site.SEGMENTDEPTH_FOR_ID]);
                        FilePath = site.SITE_NAME + site.FILEPATH_JOINER + FilePath + FileExtension;
                        if (!(File.Exists(FilePath)))
                        {
                            try
                            {
                                OutputLog.WriteLine("Downloading " + FilePath);
#if !DEBUG
                                Reader.DownloadFile(Link, FilePath);
#endif // !DEBUGGING
                            }
                            catch (WebException e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
#if DEBUG
                                OutputLog.WriteLine("Cannot connect to Host " + e);
#endif
#if !DEBUG
                                OutputLog.WriteLine("Cannot Download " + Link);
                                OutputLog.WriteLine("Because " + e);
#endif
                                Console.ResetColor();
                            }
                            catch (ArgumentNullException e)
                            {
                                OutputLog.WriteLine("You have to have an input " + e);
                                throw e;
                            }
                        }
                        else
                        {
                            OutputLog.WriteLine(FilePath + " Already Exists");
                        }


                    }
                }

                Thread.Yield();
            }
        }
        public override string ParseFilePath(string FilePath)
        {
            FilePath = FilePath.Replace(site.SITE_NAME + "%20", "");
            FilePath = FilePath.Replace("Konachan.com%20-%20", "");
            FilePath = Uri.UnescapeDataString(FilePath);
            //FilePath=FilePath.Join("",FilePath.Split(IO.Path.GetInvalidFileNameChars()));
            if (FilePath.Length > 140)
            {
                FilePath = FilePath.Substring(0, 140);
            }
            return FilePath;
        }
    }
}
