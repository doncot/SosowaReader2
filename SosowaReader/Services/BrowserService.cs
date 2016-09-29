using SosowaReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace SosowaReader.Services
{
    public class SosowaBrowseService
    {
        readonly String BaseUrl = "http://coolier.dip.jp/sosowa/ssw_l/";

        public async Task<List<Entry>> LoadCollectionAsync(int collectionNum = -1)
        {
            Uri targetUrl;
            if (collectionNum == -1)
            {
                targetUrl = new Uri(BaseUrl);
            }
            else
            {
                targetUrl = new Uri(new Uri(BaseUrl), collectionNum.ToString());
            }

            List<Entry> results = new List<Entry>();

            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            string htmlString = await (new HttpClient()).GetStringAsync(targetUrl);
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

        public async Task<Entry> LoadContentAsync(String url)
        {
            var contentUri = new Uri(new Uri(BaseUrl), url);

            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            string htmlString = await(new HttpClient()).GetStringAsync(contentUri);
            htmlDoc.LoadHtml(htmlString);

            //h1要素は1つしかない
            var titleNode = htmlDoc.DocumentNode.Descendants("h1").Single();

            //本文
            var contentNode = htmlDoc.DocumentNode.Descendants("section")
                .Where(x => x.GetAttributeValue("id", "") == "body").Single()
                .Descendants("div")
                .Where(x => x.GetAttributeValue("id", "") == "content").Single();
            var contentBodyNode = contentNode.Descendants("div")
                .Where(x => x.GetAttributeValue("id", "") == "contentBody").Single();
            var convertedContent = contentBodyNode.InnerHtml.Replace("<br>", "\n");

            return new Entry
            {
                Title = titleNode.InnerText,
                Content = convertedContent,
            };
        }

        private async Task<bool> PageExists(String url)
        {
            using (var client = new HttpClient())
            {
                var httpRequestMsg = new HttpRequestMessage(HttpMethod.Head, url);
                var response = await client.SendAsync(httpRequestMsg);
                return response.IsSuccessStatusCode;
            }
        }
    }
}
