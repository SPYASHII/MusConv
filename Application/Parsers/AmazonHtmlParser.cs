using Application.Interfaces.Parsers;
using Domain.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parsers
{
    enum XPath
    {
        PlaylistDetail,
        MusicContainer,
        MusicLink
    }
    internal class AmazonHtmlParser : IHtmlParser
    {
        private readonly Dictionary<XPath, string> _xPathes = new()
        {
            {
                XPath.MusicContainer,
                "//music-container//music-image-row"
            },
            {
                XPath.MusicLink,
                ".//music-link"
            },
            {
                XPath.PlaylistDetail,
                "//music-detail-header"
            }
        };


        public Playlist GetPlaylist(HtmlDocument doc)
        {
            return BuildPlaylist(doc);
        }
        private Playlist BuildPlaylist(HtmlDocument doc)
        {
            var playlist = GetPlaylistInfo(doc);

            playlist.Tracks = GetTracks(doc);

            return playlist;
        }
        private Playlist GetPlaylistInfo(HtmlDocument doc)
        {
            var playlistDetail = doc.DocumentNode.SelectSingleNode(_xPathes[XPath.PlaylistDetail]);

            var attributes = playlistDetail.Attributes;

            string name = attributes
                .Single(a => a.Name.Equals("headline"))
                .Value;

            string description = attributes
                .Single(a => a.Name.Equals("secondary-text"))
                .Value;

            var playlist = new Playlist()
            {
                Name = name,
                Description = description
            };

            return playlist;
        }
        private IEnumerable<Track> GetTracks(HtmlDocument doc)
        {
            var musicContainers = doc.DocumentNode.SelectNodes(_xPathes[XPath.MusicContainer]);

            var tracks = new List<Track>();

            foreach (var musicContainer in musicContainers)
            {
                var track = GetTrackFromNode(musicContainer);

                tracks.Add(track);
            }

            return tracks;
        }
        private Track GetTrackFromNode(HtmlNode musicContainer)
        {
            var musicLinks = musicContainer.SelectNodes(_xPathes[XPath.MusicLink]);

            string name = musicLinks[0].InnerText.Trim();
            string artistName = musicLinks[1].InnerText.Trim();
            string albumName = musicLinks[2].InnerText.Trim();
            string duration = musicLinks[3].InnerText.Trim();

            var track = new Track()
            {
                Name = name,
                ArtistName = artistName,
                AlbumName = albumName,
                Duration = duration,
            };

            return track;
        }
    }
}
