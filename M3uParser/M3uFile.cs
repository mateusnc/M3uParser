using System;
using System.Collections.Generic;
using System.Text;

namespace M3uParser
{
    public class M3uFile
    {
        public List<M3uMedia> Medias { get; set; }
    }

    public class M3uMedia
    {
        public string Title { get; set; }

        public decimal Duration { get; set; }

        public Dictionary<string, string> Attributes { get; set; }

        public string Url { get; set; }
    }
}
