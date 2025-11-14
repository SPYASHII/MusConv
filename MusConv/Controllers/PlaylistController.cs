using Application.Interfaces.Managers;
using MusConv.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusConv.Controllers
{
    public class PlaylistController
    {
        IWebPlaylistManager _webPlaylistManager;
        MainWindowViewModel _vm;
        public PlaylistController(IWebPlaylistManager webPlaylistManager, MainWindowViewModel vm)
        {
            _webPlaylistManager = webPlaylistManager;
            _vm = vm;

            vm.LoadCommand.Subscribe(_ => LoadPlaylistAsync());
        }

        private async void LoadPlaylistAsync()
        {
            _vm.IsLoading = true;

            try
            {
                var playlist = await _webPlaylistManager.GetPlaylistAsync(_vm.TargetUrl);
                _vm.Playlist = playlist;
            }
            catch (Exception ex)
            {
                _vm.ErrorMessage = ex.Message;
            }
            finally
            {
                _vm.IsLoading = false;
            }
        }


    }
}
