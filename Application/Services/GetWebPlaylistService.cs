using Application.Interfaces.Downloaders;
using Application.Interfaces.Factories;
using Application.Interfaces.Services;
using Domain.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Services
{
    internal class GetWebPlaylistService : IGetWebPlaylistService
    {
        private IHtmlInstrumentsFactory _htmlInstrumentsFactory;
        public GetWebPlaylistService(IHtmlInstrumentsFactory factory)
        {
            _htmlInstrumentsFactory = factory;
        }

        public async Task<Playlist> GetPlaylistAsync(string url)
        {
            var web = _htmlInstrumentsFactory.CreateDownloader(url);

            string html = await web.DownloadHtmlAsync(url);


            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(html);

            var parser = _htmlInstrumentsFactory.CreateParser(url);

            var playlist = parser.GetPlaylist(htmlDoc);

            return playlist;
        }
    }
}
