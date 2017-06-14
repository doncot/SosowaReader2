using SosowaReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;

namespace SosowaReader.Services
{
    public class SosowaBrowseService
    {
        private static readonly String BaseUrl = "http://coolier.dip.jp/sosowa/ssw_l/";
        private static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public async Task<int> GetActiveCollectionNo()
        {
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            string htmlString = await (new HttpClient()).GetStringAsync(BaseUrl);
            htmlDoc.LoadHtml(htmlString);

            var idNode = htmlDoc.DocumentNode.Descendants("div")
                .Where(x => x.GetAttributeValue("class", "") == "pagerContainer subjectPager").Single()
                .Descendants("a")
                .Where(x => x.GetAttributeValue("class", "") == "active");

            return Int32.Parse(idNode.Single().InnerText);
        }

        public async Task<List<Entry>> LoadCollectionAsync(int collectionNo = -1)
        {
            Uri targetUrl;
            if (collectionNo == -1)
            {
                targetUrl = new Uri(BaseUrl);
            }
            else
            {
                targetUrl = new Uri(new Uri(BaseUrl), collectionNo.ToString());
            }

            List<Entry> results = new List<Entry>();

            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            string htmlString = await (new HttpClient()).GetStringAsync(targetUrl);
            htmlDoc.LoadHtml(htmlString);

            return RetrieveEntriesNodesFromRootNode(htmlDoc.DocumentNode);
        }

        /// <summary>
        /// 作品本体をロードする
        /// </summary>
        /// <param name="url">ロード対象のURL</param>
        /// <returns>作品本体</returns>
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

        public async Task<List<Entry>> SearchEntriesAsync(EntryQuery query)
        {
            //Build query
            string queryString = "search?";
            //検索文字列
            queryString += "query=" + query.SearchString;
            //タグ
            queryString += "&tags=" + query.Tags;

            var contentUri = new Uri(new Uri(BaseUrl), queryString);

            var htmlDoc = new HtmlDocument();
            string htmlString = await (new HttpClient()).GetStringAsync(contentUri);
            htmlDoc.LoadHtml(htmlString);

            return RetrieveEntriesNodesFromRootNode(htmlDoc.DocumentNode);
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

        private List<Entry> RetrieveEntriesNodesFromRootNode(HtmlNode rootNode)
        {
            var entryNodes = rootNode.Descendants("section")
                .Where(x => x.GetAttributeValue("class", "") == "entries").Single()
                .Descendants("tbody").Single()
                .Descendants("tr")
                .Where(x => x.GetAttributeValue("class", "").StartsWith("article"));

            return RetrieveEntriesFromNodes(entryNodes);
        }

        private List<Entry> RetrieveEntriesFromNodes(IEnumerable<HtmlNode> entryNodes)
        {
            var results = new List<Entry>();

            foreach (var entry in entryNodes)
            {
                //タイトル
                var title = System.Net.WebUtility.HtmlDecode(entry.Descendants("a").First().InnerText);
                //URL
                var url = entry.Descendants("a").First().GetAttributeValue("href", "");

                //作者
                var name = entry.Descendants("td").Where(x => x.GetAttributeValue("class", "") == "name").Single()
                    .Descendants("a").Single().InnerText;

                //日付
                long unixTime = entry.Descendants("td").Where(x => x.GetAttributeValue("class", "") == "dateTime")
                    .Single().GetAttributeValue("data-unixtime", 0);

                //ポイント
                var points = entry.Descendants("td").Where(x => x.GetAttributeValue("class", "") == "info points").Single().InnerText;

                //タグ
                var tags = new List<Tag>();
                var tagListNodes = entry.NextSibling.Descendants("li");
                foreach (var tagListNode in tagListNodes)
                {
                    var tagNode = tagListNode.Descendants("a").Single();
                    var newTag = new Tag { Name = tagNode.InnerText, Url = tagNode.GetAttributeValue("href", "") };
                    tags.Add(newTag);
                }

                //セット
                results.Add(new Entry
                {
                    Title = title,
                    Author = name,
                    Url = url,
                    UploadDate = UNIX_EPOCH.AddSeconds(unixTime).ToLocalTime(),
                    Points = Int32.Parse(points),
                    Tags = tags,
                });
            }

            return results;
        }
    }
}
