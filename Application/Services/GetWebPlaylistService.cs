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
        private IHtmlParserFactory _htmlParserFactory;
        public GetWebPlaylistService(IHtmlParserFactory factory)
        {
            _htmlParserFactory = factory;
        }

        public async Task<Playlist> GetPlaylistAsync(string url)
        {
            HtmlWeb web = new HtmlWeb();

            var htmlDoc = await web.LoadFromWebAsync(url);

            var parser = _htmlParserFactory.Create(url);

            var playlist = parser.GetPlaylist(htmlDoc);

            return playlist;
        }
    }
}
