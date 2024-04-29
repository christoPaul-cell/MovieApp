namespace ChristoMoviesProject.Models
{
    public class Media
    {
        public int Id { get; set; }
        public string DisplayTitle { get; set; }
        public string MediaType { get; set; } // "movie" or "tv"

        public string Thumbnail { get; set; }
        public string ThumbnailSmall { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ThumbnailLarge { get; set; }
        public string DownloadUrl { get; set; }
        public string Overview { get; set; }
         
        public string ReleaseDate { get; set; }

        public string TrailerURL { get; set; }
    }
}
