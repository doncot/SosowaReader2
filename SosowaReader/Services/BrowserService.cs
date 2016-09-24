using SosowaReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace SosowaReader.Services
{
    public class BrowserService
    {
        readonly String MainPageUrl = "http://coolier.dip.jp/sosowa/ssw_l/";

        public async Task<List<Entry>> LoadMainPageAsync()
        {
            List<Entry> results = new List<Entry>();

            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            string htmlString = await (new HttpClient()).GetStringAsync(MainPageUrl);
            htmlDoc.LoadHtml(htmlString);

            var entries = htmlDoc.DocumentNode.Descendants("section")
                .Where(x => x.GetAttributeValue("class", "") == "entries").Single()
                .Descendants("tbody").Single()
                .Descendants("tr")
                .Where(x => x.GetAttributeValue("class", "").StartsWith("article"));



            //var nodes = htmlDoc.DocumentNode.Descendants("td")// タグ
            //    .Where(x => x.GetAttributeValue("class", "") == "title"); //要素

            foreach (var entry in entries)
            {
                //タイトル
                var title = entry.Descendants("a").First().InnerText;
                //URL
                var url = entry.Descendants("a").First().GetAttributeValue("href", "");


                //作者


                //セット
                results.Add(new Entry
                {
                    Title = title,
                    Url = url,
                });
            }

            return results;
        }
    }
}
