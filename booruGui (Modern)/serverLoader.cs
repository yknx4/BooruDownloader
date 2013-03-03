using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooruDownloader;
using System.Collections.Specialized;
using System.Configuration;

namespace booruGui__Modern_
    {
   public class serverLoader
        {
        public SiteData[] Sites;
        public int sitesCount { get; private set; }
        System.Configuration.Configuration config;
        static private bool stringToBool(string text)
            {
            if (text == "true") { 
                return true;}
            else{
            return false;
                }
            }
        public SiteData this[int sitePosition]
            {
            get
                {
                // This indexer is very simple, and just returns or sets 
                // the corresponding element from the internal array. 
                if (sitePosition>sitesCount-1)
                {
                throw new ArgumentOutOfRangeException("sitePosition");
                }
                return Sites[sitePosition];
                }
            private set
                {
                Sites[sitePosition] = value;
                }
            }

        public serverLoader(NameValueCollection appSettings)
            {
            NameValueCollection sitesList = appSettings;
            int siteCount = appSettings.Count;
            sitesCount = siteCount;
            Sites = new SiteData[siteCount];
            for (int i = 0; i < siteCount; i++ )
                {
               
                ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
                configMap.ExeConfigFilename = @"Hosts\" + sitesList[i];
                config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                KeyValueConfigurationCollection siteOptions = config.AppSettings.Settings;
                Sites[i]=new SiteData();
                Sites[i].SITE_DOMAIN = siteOptions["domain"].Value;
                Sites[i].SITE_NAME = siteOptions["name"].Value;
                Sites[i].START_PAGE_INDEX = Convert.ToUInt32(siteOptions["startPageIndex"].Value);
                Sites[i].NUMBER_OF_THREADS = Convert.ToInt32(siteOptions["numberOfThreads"].Value);
                Sites[i].SEGMENTDEPTH_FOR_ID = Convert.ToInt32(siteOptions["segmentDepthForId"].Value);
                Sites[i].USER_AGENT_STRING = siteOptions["user-agent"].Value;
                Sites[i].CHECKTAGS_STRING = Uri.UnescapeDataString(siteOptions["checkTags"].Value);
                Sites[i].ACCESSPAGE_STRING = siteOptions["accessPage"].Value;
                Sites[i].POSTTAGS_STRING = Uri.UnescapeDataString(siteOptions["postTags"].Value);
                Sites[i].PAGENUMBER_XPATH = Uri.UnescapeDataString(siteOptions["pageNumber_xpath"].Value);
                Sites[i].POSTLINKS_XPATH = Uri.UnescapeDataString(siteOptions["postLinks_xpath"].Value);
                Sites[i].IMAGECONTAINER_XPATH = Uri.UnescapeDataString(siteOptions["imageContainer_xpath"].Value);
                Sites[i].FILEPATH_JOINER = siteOptions["filepathJoiner"].Value;
                Sites[i].DelayInConnections = stringToBool(siteOptions["delayInConnections"].Value);
                Sites[i].type = siteOptions["type"].Value;
                //Console.WriteLine(siteOptions["domain"].Value);
#if DEBUG
                Sites[i].printValues();
#endif
                }
            }

        }
    }
