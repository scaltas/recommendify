using Microsoft.AspNetCore.Mvc;
using Recommendify.Web.ViewModels;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;

namespace Recommendify.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly TMDbClient _client;

        public MoviesController(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new TMDbClient(_configuration["TMDbApiKey"]);
        }

        public async Task<IActionResult> Index()
        {
            var random = new Random();
            var randomMovieIds = new List<int>();

            // Generate 30 random movie IDs
            while (randomMovieIds.Count < 30)
            {
                int randomMovieId = random.Next(1, 100000); // Adjust the range as needed

                // Check if the randomMovieId is not already in the list
                if (!randomMovieIds.Contains(randomMovieId))
                {
                    randomMovieIds.Add(randomMovieId);
                }
            }

            // Fetch movie details for the random IDs asynchronously
            var randomMovies = new List<Movie>();

            // Fetch movie details in parallel
            var tasks = randomMovieIds.Select(async movieId =>
            {
                var movie = await _client.GetMovieAsync(movieId);
                if (movie != null)
                {
                    randomMovies.Add(movie);
                }
            });

            // Wait for all API requests to complete
            await Task.WhenAll(tasks);

            // Sort the randomMovies list by rating in descending order
            randomMovies = randomMovies.OrderByDescending(m => m.VoteAverage).ToList();

            // Select the top 3 movies with the highest ratings
            var topRatedMovies = randomMovies.Take(3).ToList();

            var viewModel = new MovieViewModel
            {
                Movies = topRatedMovies
            };

            return View(viewModel);
        }
    }
}
