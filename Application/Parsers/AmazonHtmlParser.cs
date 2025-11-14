using Application.Interfaces.Parsers;
using Domain.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
    enum PlaylistType
    {
        None = 0,
        Playlist,
        Album
    }
    internal class AmazonHtmlParser : IHtmlParser
    {
        private readonly Dictionary<XPath, string> _playlistXPathes = new()
        {
            {
                XPath.MusicContainer,
                ".//music-container//music-image-row"
            },
            {
                XPath.MusicLink,
                ".//music-link"
            },
            {
                XPath.PlaylistDetail,
                ".//music-detail-header"
            }
        };
        private readonly Dictionary<XPath, string> _albumXPathes = new()
        {
            {
                XPath.MusicContainer,
                ".//music-container//music-text-row"
            },
            {
                XPath.MusicLink,
                ".//music-link"
            },
            {
                XPath.PlaylistDetail,
                ".//music-detail-header"
            }
        };


        public Playlist GetPlaylist(HtmlDocument doc)
        {
            return SelectScenarioAndGetPlaylist(doc);
        }
        private Playlist SelectScenarioAndGetPlaylist(HtmlDocument doc)
        {
            var playlistType = GetPlaylistType(doc);

            switch(playlistType)
            {
                case PlaylistType.Playlist:
                    return BuildPlaylist(doc);
                case PlaylistType.Album:
                    return BuildAlbum(doc);
                default:
                    throw new ArgumentException("Invalid Url");
            }
        }
        private PlaylistType GetPlaylistType(HtmlDocument doc)
        {
            var playlistContainers = GetMusicContainer(doc, _playlistXPathes[XPath.MusicContainer]);
            var albumContainers = GetMusicContainer(doc, _albumXPathes[XPath.MusicContainer]);

            if (playlistContainers != null)
                return PlaylistType.Playlist;
            else
                return PlaylistType.Album;

        }
        private HtmlNodeCollection GetMusicContainer(HtmlDocument doc, string xPath)
        {
            return doc.DocumentNode.SelectNodes(xPath);
        }
        #region Playlist
        private Playlist BuildPlaylist(HtmlDocument doc)
        {
            var playlist = GetPlaylistInfo(doc);

            playlist.Tracks = GetPlaylistTracks(doc);

            return playlist;
        }
        private Playlist GetPlaylistInfo(HtmlDocument doc)
        {
            var playlistDetail = doc.DocumentNode.SelectNodes(_playlistXPathes[XPath.PlaylistDetail]);

            var playNode = playlistDetail.Last();

            var attributes = playNode.Attributes;

            string name = attributes
                .Single(a => a.Name.Equals("headline"))
                .Value;

            string description = attributes
                .SingleOrDefault(a => a.Name.Equals("secondary-text"))
                ?.Value ?? string.Empty;

            string imgSrc = attributes
                .SingleOrDefault(a => a.Name.Equals("image-src"))
                ?.Value ?? string.Empty;

            var playlist = new Playlist()
            {
                Name = name,
                Description = description,
                ImageUrl = imgSrc
            };

            return playlist;
        }
        private IEnumerable<Track> GetPlaylistTracks(HtmlDocument doc)
        {
            var musicContainers = GetMusicContainer(doc, _playlistXPathes[XPath.MusicContainer]);

            var tracks = new List<Track>();

            foreach (var musicContainer in musicContainers)
            {
                var track = GetPLaylistTrackFromNode(musicContainer);

                tracks.Add(track);
            }

            return tracks;
        }
        private Track GetPLaylistTrackFromNode(HtmlNode musicContainer)
        {
            var musicLinks = musicContainer.SelectNodes(_playlistXPathes[XPath.MusicLink]);

            string name = GetTextFromLinkAndFormat(musicLinks[0]);
            string artistName = GetTextFromLinkAndFormat(musicLinks[1]);
            string albumName = GetTextFromLinkAndFormat(musicLinks[2]);
            string duration = FormatText(musicLinks[3].InnerText.Trim());

            var track = new Track()
            {
                Name = name,
                ArtistName = artistName,
                AlbumName = albumName,
                Duration = duration,
            };

            return track;
        }
        #endregion
        private string GetTextFromLinkAndFormat(HtmlNode node)
        {
            var text = node.SelectSingleNode("a").InnerText.Trim();

            return FormatText(text);
        }
        private string FormatText(string text )
        {
            text = text.Replace("amp;", "");

            return text;
        }
        #region Album
        private Playlist BuildAlbum(HtmlDocument doc)
        {
            var album = GetAlbumInfo(doc, out string author);

            album.Tracks = GetAlbumTracks(doc);

            SetAuthorAndAlbum(album, author);

            return album;
        }
        private void SetAuthorAndAlbum(Playlist album, string author)
        {
            foreach(var track in album.Tracks)
            {
                track.ArtistName = author;
                track.AlbumName = album.Name;
            }
        }
        private Playlist GetAlbumInfo(HtmlDocument doc, out string author)
        {
            var playlistDetail = doc.DocumentNode.SelectNodes(_albumXPathes[XPath.PlaylistDetail]);

            var albumNode = playlistDetail.Last();

            var attributes = albumNode.Attributes;

            string name = attributes
                .Single(a => a.Name.Equals("headline"))
                .Value;

            author = attributes
                .Single(a => a.Name.Equals("primary-text"))
                .Value;

            string imgSrc = attributes
                .SingleOrDefault(a => a.Name.Equals("image-src"))
                ?.Value ?? string.Empty;

            var playlist = new Playlist()
            {
                Name = name,
                ImageUrl = imgSrc
            };

            return playlist;
        }
        private IEnumerable<Track> GetAlbumTracks(HtmlDocument doc)
        {
            var musicContainers = GetMusicContainer(doc, _albumXPathes[XPath.MusicContainer]);

            var tracks = new List<Track>();

            foreach (var musicContainer in musicContainers)
            {
                var track = GetAlbumTrackFromNode(musicContainer);

                tracks.Add(track);
            }

            return tracks;
        }
        private Track GetAlbumTrackFromNode(HtmlNode musicContainer)
        {
            var musicLinks = musicContainer.SelectNodes(_albumXPathes[XPath.MusicLink]);

            string name = GetTextFromLinkAndFormat(musicLinks[0]);

            string duration = FormatText(musicLinks[1].InnerText.Trim());

            var track = new Track()
            {
                Name = name,
                Duration = duration,
            };

            return track;
        }
        #endregion
    }
}
