using Application.Interfaces.Managers;
using Domain.Models;
using ReactiveUI;
using System.Reactive;

namespace MusConv.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private string _targetUrl = string.Empty;
        private Playlist? _playlist;
        private bool _isLoading = false;
        private string _errorMessage = string.Empty;
        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }
        public Playlist Playlist
        {
            get => _playlist;
            set => this.RaiseAndSetIfChanged(ref _playlist, value);
        }
        public string TargetUrl
        {
            get => _targetUrl;
            set => this.RaiseAndSetIfChanged(ref _targetUrl, value);
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }
        public ReactiveCommand<Unit, Unit> LoadCommand { get; }

        public MainWindowViewModel()
        {
            LoadCommand = ReactiveCommand.Create(() => { });
        }
    }
}
