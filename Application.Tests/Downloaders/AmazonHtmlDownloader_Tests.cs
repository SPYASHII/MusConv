using Application.Downloaders;
using Application.Parsers;
using Domain.Models;
using HtmlAgilityPack;

namespace Application.IntegrationTests;

[TestClass]
public class AmazonHtmlDownloader_Tests
{
    [TestMethod]
    public void DownloadHtmlAsync_WithUrl_ReturnsCorrectHtml()
    {
        var downloader = new AmazonHtmlDownloader();

        string url = "https://music.amazon.com/playlists/B01M11SBC8";

        //act

        string html = downloader.DownloadHtmlAsync(url).Result;

        //assert
        bool isEmpty = string.IsNullOrEmpty(html);

        Assert.IsFalse(isEmpty);

        var doc = new HtmlDocument();

        doc.LoadHtml(html);

        var actual = new AmazonHtmlParser().GetPlaylist(doc);

        Assert.AreEqual("All Hits", actual.Name);
        Assert.AreEqual(60, actual.Tracks.Count());
    }
}
