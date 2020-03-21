using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace M3uParser
{
    public class M3uParser
    {
        private static Regex _mediaRegex = new Regex(@"#(?<key>.*?):(?<duration>-?[0-9]\d*(\.\d+)?)\s(?<attributes>.*),(?<title>.*)\r?\n(?<url>.*)");
        private static Regex _attributesRegex = new Regex("(?<key>[a-zA-Z0-9-]*)=\"(?<value>.*?)\"");

        public M3uFile Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new Exception();

            if (data.IndexOf("#EXTM3U") != 0)
                throw new Exception();

            var file = new M3uFile
            {
                Medias = new List<M3uMedia>()
            };

            var matches = Regex.Matches(data, "#[^#]*");
            for (var i = 1; i < matches.Count; i++)
                TryAddMedia(file, matches[i].Value);

            return file;
        }

        public M3uFile ParseFrom(string path) => Parse(File.ReadAllText(path));

        public async Task<M3uFile> ParseFrom(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            using (var httpClient = new HttpClient())
            {
                var resp = await httpClient.GetAsync(uri).ConfigureAwait(false);
                var data = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

                return Parse(data);
            }
        }

        private void TryAddMedia(M3uFile file, string data)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            if (string.IsNullOrEmpty(data))
                throw new ArgumentException();

            var match = _mediaRegex.Match(data);
            if (match.Success)
            {
                var media = new M3uMedia
                {
                    Title = match.Groups["title"].Value.Trim(),
                    Duration = decimal.Parse(match.Groups["duration"].Value.Trim()),
                    Url = match.Groups["url"].Value.Trim(),
                    Attributes = new Dictionary<string, string>()
                };

                var attributes = _attributesRegex.Matches(match.Groups["attributes"].Value);
                foreach (Match attr in attributes)
                    media.Attributes.Add(attr.Groups["key"].Value, attr.Groups["value"].Value);

                file.Medias.Add(media);
            }
            else
            {
                // TODO: Add warning.
            }
        }
    }
}
