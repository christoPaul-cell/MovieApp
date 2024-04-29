using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChristoMoviesProject.Models;
using ChristoMoviesProject.Pages;
using ChristoMoviesProject.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChristoMoviesProject.ViewModels
{
    [QueryProperty(nameof(Media), nameof(Media))]
    public partial class DetailsViewModel : ObservableObject
    {
        private readonly TmdbService _tmdbService;

        public DetailsViewModel(TmdbService tmdbService)
        {
            _tmdbService = tmdbService;

        }

        [ObservableProperty]
        private Media _media;

        //[ObservableProperty]
        //private Media _selectedFilm;
        //public Media SelectedFilm
        //{
        //    get { return _selectedFilm; }
        //    set { SetProperty(ref _selectedFilm, value); }
        //}

        [ObservableProperty]
        private string _mainTrailerUrl;

        [ObservableProperty]
        private int _runtime;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private int _similarItemWidth = 125;

        public ICommand DownloadCommand { get; set; }
        public ObservableCollection<Video> Videos { get; set; } = new();
        public ObservableCollection<Media> Similar { get; set; } = new();

        public async Task InitializeAsync()
        {    

            var similarMediasTask = _tmdbService.GetSimilarAsync(Media.Id, Media.MediaType);
            IsBusy = true;
            try
            {

                var trailerTeasersTask = _tmdbService.GetTrailersAsync(Media.Id, Media.MediaType);
                var detailsTask = _tmdbService.GetMediaDetailsAsync(Media.Id, Media.MediaType);

                var trailerTeasers = await trailerTeasersTask;
                var details = await detailsTask;

                if (trailerTeasers?.Any() == true)
                {
                    var trailer = trailerTeasers.FirstOrDefault(t => t.type == "Trailer");
                    trailer ??= trailerTeasers.First();
                    MainTrailerUrl = GenerateYoutubeUrl(trailer.key);
                    DownloadCommand = new Command(OnDownloadClicked);


                    foreach (var video in trailerTeasers)
                    {
                        Videos.Add(video);
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Not found", "No videos found", "Ok");
                }
                if (details is not null)
                {
                    Runtime = details.runtime;
                }
            }
            finally
            {
                IsBusy = false; 
            }

            var similarMedias = await similarMediasTask;
            if(similarMedias?.Any() == true)
            {
                foreach (var media in similarMedias)
                {
                    Similar.Add(media);
                }
            }
        }

        private async void OnDownloadClicked()
        {
            if (!string.IsNullOrEmpty(_mainTrailerUrl))
            {
                using (HttpClient client = new HttpClient())
                {
                    // Téléchargement du film à partir de l'URL associée au bouton
                    byte[] movieData = await client.GetByteArrayAsync(_mainTrailerUrl);

                    // Enregistrement du film localement (exemple)
                    string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "downloaded_movie.mp4");
                    File.WriteAllBytes(savePath, movieData);

                    // Afficher un message de confirmation ou effectuer d'autres actions après le téléchargement
                }
            }
        }
            [RelayCommand]
        private async Task ChangeToThisMedia(Media media)
        {
            var parameters = new Dictionary<string, object>
            {
                [nameof(DetailsViewModel.Media)] = media
            };
            await Shell.Current.GoToAsync(nameof(DetailsPage), true, parameters);
        }

        [RelayCommand]
        private void SetMainTrailer(string videoKey) => 
            MainTrailerUrl = GenerateYoutubeUrl(videoKey);

        private static string GenerateYoutubeUrl(string videoKey) =>
            $"https://www.youtube.com/embed/{videoKey}";

        public async Task StartMoviePlaybackAsync()
        {
            // Assurez-vous d'avoir une URL valide pour le lecteur vidéo principal
            if (!string.IsNullOrEmpty(MainTrailerUrl))
            {
                // Utilisez Xamarin.Essentials pour ouvrir l'URL dans le navigateur par défaut
                await Browser.OpenAsync(MainTrailerUrl, BrowserLaunchMode.SystemPreferred);
            }
            else
            {
                // Affichez une alerte indiquant qu'aucune vidéo n'est disponible
                await Shell.Current.DisplayAlert("Not found", "No video available", "Ok");
            }
        }

    }
}


