using System;
using System.Collections.Generic;

namespace SosowaReader.Models
{
    /// <summary>
    /// 作品
    /// </summary>
    public class Work
    {
        public String Title { get; set; }

        public String Author { get; set; }

        /// <summary>
        /// 登校日
        /// </summary>
        public DateTime UploadDate { get; set; }

        public List<String> Tags { get; set; }

        public int Size { get; set; }

        public String Summary { get; set; }
    }
}
