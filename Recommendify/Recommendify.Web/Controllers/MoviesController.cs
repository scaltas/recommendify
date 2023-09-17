using Microsoft.AspNetCore.Mvc;
using Recommendify.Web.ViewModels;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;

namespace Recommendify.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly TMDbClient _client;

        public MoviesController(IConfiguration configuration)
        {
            _client = new TMDbClient(configuration["TMDbApiKey"]);
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new MovieViewModel
            {
                Movies = new List<Movie>()
                {
                    await _client.GetMovieAsync(819223),
                    await _client.GetMovieAsync(1381),
                    await _client.GetMovieAsync(567410),
                }
            };

            return View(viewModel);
        }
    }
}
