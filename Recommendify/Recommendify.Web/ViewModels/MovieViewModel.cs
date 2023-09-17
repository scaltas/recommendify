using TMDbLib.Objects.Movies;

namespace Recommendify.Web.ViewModels
{
    public class MovieViewModel
    {
        public required List<Movie> Movies { get; set; }
    }
}
