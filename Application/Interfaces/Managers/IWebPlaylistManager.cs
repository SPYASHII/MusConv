using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Managers
{
    public interface IWebPlaylistManager
    {
        Task<Playlist> GetPlaylistAsync(string url);
    }
}
