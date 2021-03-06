using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace M3uParser.Tests
{
    public class M3uParserTests
    {
        [Fact]
        public void Test1()
        {
            var parser = new M3uParser();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Data\\example.m3u");
            var m3u = parser.ParseFrom(path);

            Assert.Equal(5906, m3u.Medias.Count);
        }


        [Fact]
        public async Task Test2()
        {
            var parser = new M3uParser();
            var m3u = await parser.ParseFrom(new Uri("http://meuip.tv/iptv78017675"));

            Assert.Equal(5906, m3u.Medias.Count);
        }
    }
}
