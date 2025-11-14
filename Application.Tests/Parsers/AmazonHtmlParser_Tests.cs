using Application.Parsers;
using Domain.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests
{
    [TestClass]
    public class AmazonHtmlParser_Tests
    {
        [TestMethod]
        public void GetPlaylist_WithAmazonHtmlDoc_ReturnsInfo()
        {
            var doc = new HtmlDocument();

            doc.Load("Parsers/TestFiles/Playlist/amazon.html");

            var parser = new AmazonHtmlParser();

            var expected = new Playlist()
            {
                Name = "All Hits"
            };

            int numOfTracksExpected = 60;

            //act
            
            var playlist = parser.GetPlaylist(doc);

            //assert
            Assert.AreEqual(expected.Name, playlist.Name);
            Assert.AreEqual(numOfTracksExpected, playlist.Tracks.Count());
        }
    }
}
