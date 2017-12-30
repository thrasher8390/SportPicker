using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FootballPicker.InternetConnection
{
    public class InternetConnector
    {
        private HtmlDocument HtmlReader;

        public InternetConnector(string webURL)
        {
            string html = new WebClient().DownloadString(webURL);
            HtmlReader = new HtmlDocument();
            HtmlReader.LoadHtml(html);
        }

        public HtmlDocument GetHtml()
        {
            return HtmlReader;
        }

    }
}
