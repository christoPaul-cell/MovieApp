using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChristoMoviesProject.Models;
using ChristoMoviesProject.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChristoMoviesProject.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly TmdbService _tmdbService;

        public HomeViewModel(TmdbService tmdbService)
        {
            _tmdbService = tmdbService;
            SearchMovieCommand = new AsyncRelayCommand<Media>(SearchMovieAsync);
            PlayMovieCommand = new RelayCommand<Media>(PlayMovie);
        }

        public ICommand SearchMovieCommand { get; }
        public ICommand PlayMovieCommand { get; }

        private async Task SearchMovieAsync(Media media)
        {
            // Logique de recherche de film ici
            // Par exemple, ouvrir une nouvelle vue avec les détails du film
            await Task.Delay(100); // Exemple d'une opération asynchrone
        }

        private void PlayMovie(Media media)
        {
            try
            {

                SelectedMedia = TrendingMovie;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la lecture du film : {ex.Message}");
                // Gérer l'exception ici, par exemple : afficher un message d'erreur à l'utilisateur
            }
        }

        [ObservableProperty]
        private Media _trendingMovie;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShowMovieInfoBox))]
        private Media? _selectedMedia;

        public bool ShowMovieInfoBox => SelectedMedia is not null;

        public ObservableCollection<Media> Trending { get; set; } = new();
        public ObservableCollection<Media> TopRated { get; set; } = new();
        public ObservableCollection<Media> NetflixOriginals { get; set; } = new();
        public ObservableCollection<Media> ActionMovies { get; set; } = new();

        public async Task InitializeAsync()
        {
            try
            {
                var trendingListTask = _tmdbService.GetTrendingAsync();
                var netflixOriginalsListTask = _tmdbService.GetNetflixOriginalAsync();
                var topRatedListTask = _tmdbService.GetTopRatedAsync();
                var actionListTask = _tmdbService.GetActionAsync();

                var medias = await Task.WhenAll(trendingListTask, netflixOriginalsListTask, topRatedListTask, actionListTask);

                var trendingList = medias[0];
                var netflixOriginalsList = medias[1];
                var topRatedList = medias[2];
                var actionList = medias[3];

                // Seting random trending movie from Trending List to the Trending Movie
                TrendingMovie = trendingList.OrderBy(t => Guid.NewGuid()).FirstOrDefault(t => !string.IsNullOrWhiteSpace(t.DisplayTitle) && !string.IsNullOrWhiteSpace(t.Thumbnail));

                SetMediaCollection(trendingList, Trending);
                SetMediaCollection(netflixOriginalsList, NetflixOriginals);
                SetMediaCollection(topRatedList, TopRated);
                SetMediaCollection(actionList, ActionMovies);

                SelectedMedia = TrendingMovie;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de l'initialisation : {ex.Message}");
                // Gérer l'exception ici, par exemple : afficher un message d'erreur à l'utilisateur
            }
        }

        private static void SetMediaCollection(IEnumerable<Media> medias, ObservableCollection<Media> collection)
        {
            collection.Clear();
            foreach (var media in medias)
            {
                collection.Add(media);
            }
        }

        [RelayCommand]
        private void SelectMedia(Media? media = null)
        {
            try
            {
                if (media is not null)
                {
                    if (media.Id == SelectedMedia?.Id)
                    {
                        media = null;
                    }
                }
                SelectedMedia = media;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la sélection du média : {ex.Message}");
                // Gérer l'exception ici, par exemple : afficher un message d'erreur à l'utilisateur
            }
        }
    }
}