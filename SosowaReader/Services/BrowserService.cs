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

        public async Task<List<Work>> LoadMainPageAsync()
        {
            List<Work> works = new List<Work>();

            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            string htmlString = await (new HttpClient()).GetStringAsync(MainPageUrl);
            htmlDoc.LoadHtml(htmlString);


            var nodes = htmlDoc.DocumentNode.Descendants("td")// タグ
                .Where(x => x.GetAttributeValue("class", "") == "title"); //要素

            foreach (var node in nodes)
            {
                var titleNode = node.LastChild;

                works.Add(new Work
                {
                    Title = titleNode.InnerText
                });
            }

            return works;
        }
    }
}
