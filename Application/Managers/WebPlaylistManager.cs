using Application.Interfaces.Managers;
using Application.Interfaces.Services;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Managers
{
    internal class WebPlaylistManager : IWebPlaylistManager
    {
        private IGetWebPlaylistService _getWebPlaylistService;
        public WebPlaylistManager(IGetWebPlaylistService getWebPlaylistService)
        {
            _getWebPlaylistService = getWebPlaylistService;
        }
        public async Task<Playlist> GetPlaylistAsync(string url)
        {
            return await _getWebPlaylistService.GetPlaylistAsync(url);
        }
    }
}