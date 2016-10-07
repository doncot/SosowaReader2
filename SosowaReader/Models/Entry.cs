using System;
using System.Collections.Generic;

namespace SosowaReader.Models
{
    /// <summary>
    /// 作品
    /// </summary>
    public class Entry
    {
        public String Title { get; set; }

        public String Author { get; set; }

        /// <summary>
        /// 投稿日
        /// </summary>
        public DateTime UploadDate { get; set; }

        public int Size { get; set; }

        public int Points { get; set; }

        public String Summary { get; set; }
        
        public String Url { get; set; }

        public List<Tag> Tags { get; set; }

        /// <summary>
        /// 本文
        /// </summary>
        public String Content { get; set; }
    }
}
