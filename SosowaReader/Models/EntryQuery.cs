using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SosowaReader.Models
{
    public class EntryQuery
    {
        /// <summary>
        /// 検索文字列
        /// </summary>
        public string SearchString { get; set; }

        /// <summary>
        /// タグ
        /// </summary>
        private string tags = "";
        public string Tags
        {
            get { return tags; }
            set
            {
                //空白文字を+に
                tags = Regex.Replace(value, @"\s", "+");
            }
        }
    }
}
