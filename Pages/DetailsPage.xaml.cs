using ChristoMoviesProject.ViewModels;

namespace ChristoMoviesProject.Pages;

public partial class DetailsPage : ContentPage
{
    private readonly DetailsViewModel _viewModel;
    
    public DetailsPage(DetailsViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        if(width > 0)
        {
            _viewModel.SimilarItemWidth = Convert.ToInt32(width / 3) - 3;
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }

    private void TrailersTab_Tapped(object sender, TappedEventArgs e)
    {
        similarTabIndicator.Color = Colors.Black;
        similarTabContent.IsVisible = false;

        trailersTabIndicator.Color = Colors.Red;
        trailersTabContent.IsVisible = true;
    }

    private void SimilarTab_Tapped(object sender, TappedEventArgs e)
    {
        trailersTabIndicator.Color = Colors.Black;
        trailersTabContent.IsVisible = false;

        similarTabIndicator.Color = Colors.Red;
        similarTabContent.IsVisible = true;
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await pageScrollView.ScrollToAsync(0, 0, animated: true);
    }
    private async void PlayButtonTapped(object sender, EventArgs e)
    {
        // Ajoutez ici la logique pour lancer le film
        await _viewModel.StartMoviePlaybackAsync(); // Assurez-vous que cette méthode est définie dans votre DetailsViewModel
    }
    private void PlayButton_Click(object sender, EventArgs e)
    {
        // Ajoutez ici votre logique pour jouer le film
        // Par exemple, vous pouvez démarrer la lecture d'une vidéo
        // ou naviguer vers une autre page pour afficher le film
        // Voici un exemple simple :

        DisplayAlert("Action", "Le film va démarrer!", "OK");
        // Remplacez cet exemple par votre propre logique
    }
    //private async void OnDownloadClicked(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (SelectedFilm != null)
    //        {
    //            using (var httpClient = new HttpClient())
    //            {
    //                var filmUrl = SelectedFilm.DownloadUrl; // Assurez-vous que DownloadUrl est une propriété de votre objet Media contenant l'URL de téléchargement du film

    //                var response = await httpClient.GetAsync(filmUrl);

    //                if (response.IsSuccessStatusCode)
    //                {
    //                    var content = await response.Content.ReadAsByteArrayAsync();

    //                    // Enregistrez le contenu téléchargé dans un fichier sur le système de fichiers local
    //                }
    //                else
    //                {
    //                    Console.WriteLine($"Le téléchargement du film a échoué avec le code d'état : {response.StatusCode}");
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Une erreur s'est produite lors du téléchargement du film : {ex.Message}");
    //    }
    //}

}