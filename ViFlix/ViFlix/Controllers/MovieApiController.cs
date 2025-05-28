using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViFlix.Core.Services.Movies.Ganereservices;
using ViFlix.Core.Services.Movies.LanguageServices;
using ViFlix.Core.Services.Movies.MovieServices;
using ViFlix.Core.Tools;
using ViFlix.Core.ViewModels.MoviesViewModel;

namespace ViFlix.Controllers
{
    #region Movie
    [Route("api/[controller]")]
    [ApiController]
    public class MovieApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMoviesServices _movieServices;
        public MovieApiController(IMapper mapper , IMoviesServices movieServices)
        {
            _mapper = mapper;
            _movieServices = movieServices;
        }
        
        public List<MovieViewModel> movieList { get; set; }

        [HttpGet]
        public List<MovieViewModel> GetAllMovie()
        {
            movieList = _movieServices.GetAllMovies().ToList();
            return movieList;
        }

        [HttpGet("{id}")]
        public ActionResult<MovieViewModel> GetMovieById(long id)
        {
            var movie = _movieServices.GetEntityById(id);
            if (movie == null)
                return NotFound();
            else
                return Ok(movie);
        }

        [HttpPost]
        public long AddMovieFromApi(long Id)
        {
            return Id;
        }

        [HttpPost("AddMovieFromApiBody")]
        public async Task<long> AddMovieFromApiBody([FromForm]MovieViewModel movie, IFormFile MImg)
        {
            string fileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(MImg.FileName);
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Posts", fileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await MImg.CopyToAsync(stream);
            }

            return await _movieServices.AddMovies(movie , MImg);
        }

        [HttpPut("EditMovie")]
        public async Task<IActionResult> EditMovie([FromForm] MovieViewModel movie, IFormFile MImg, long Id)
        {
            var existMovie = _movieServices.GetMovieById(Id);
            if(existMovie == null)
            {
                return NotFound("movie is not found !");
            }

            existMovie.Title = movie.Title;
            existMovie.Description = movie.Description;
            existMovie.Rating = movie.Rating;
            existMovie.GanreId = movie.GanreId;
            existMovie.ReleaseDate = movie.ReleaseDate;
            existMovie.Trailer = movie.Trailer;
            existMovie.Duration = movie.Duration;
            existMovie.DownloadLink = movie.DownloadLink;
            existMovie.LanguageId = movie.LanguageId;


            string fileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(MImg.FileName);
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Avatar", fileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await MImg.CopyToAsync(stream);
            }


            existMovie.Poster = "/Images/Avatar" + fileName;


            await _movieServices.EditMovies(existMovie, MImg);

            return Ok();
        }

        [HttpDelete("DeleteMovie")]
        public async Task<IActionResult> DeleteMovie(long id)
        {
            var mo = _movieServices.GetMovieById(id);

            if (mo == null)
            {
                return NotFound(new { message = "Not found !" });
            }

            await _movieServices.DeleteMovies(id);

            return Ok();
        }

        [HttpPost("GetMovieByTitle")]
        public async Task<IActionResult> GetMovieByTitle([FromQuery]string title)
        {
            var movies = await _movieServices.GetMoviesByTitleAsync(title);

            if (movies == null || !movies.Any())
            {
                return NotFound("Cant find any movies!");
            }

            return Ok(movies);

        }

        [HttpGet("GetMovieByGenreId")]
        public async Task<IActionResult> GetMovieByGenreId(long Id)
        {
            var movie = _movieServices.GetMovieByGenreId(Id);
            return Ok(movie);
        }

        [HttpGet("GetMovieByLanguageId")]
        public async Task<IActionResult> GetMovieByLanguageId(long Id)
        {
            var movie = _movieServices.GetMovieByLanguageId(Id);
            return Ok(movie);
        }

        [HttpGet("GetNewMovies")]
        public List<MovieViewModel> GetNewMovies()
        {
            movieList = _movieServices.GetAllMovies().ToList();
            return movieList;
        }

        [HttpGet("GetOldMovies")]
        public List<MovieViewModel> GetOldMovies()
        {
            movieList = _movieServices.GetAllMovies().ToList();
            return movieList;
        }
    }
    #endregion

    #region Genre
    [Route("api/[controller]")]
    [ApiController]
    public class GanreController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGanreServices _genreServices;

        public GanreController(IMapper mapper , IGanreServices genreServices)
        {
            _mapper = mapper;
            _genreServices = genreServices;
        }

        public List<GanresViewModel> GanreList { get; set; }

        [HttpGet]
        public List<GanresViewModel> GetAllGenres()
        {
            GanreList = _genreServices.GetAllGanres().ToList();
            return GanreList;
        }

        [HttpGet("{id}")]
        public ActionResult<GanresViewModel> GetGenreById(long id)
        {
            var genre = _genreServices.GetGanresById(id);
            if (genre == null)
                return NotFound(genre);
            else
                return Ok(genre);
        }

        [HttpPost]
        public long AddGenreFromApi(long id)
        {
            return id;
        }

        [HttpPost("AddGenre")]
        public async Task<long> AddGenre(GanresViewModel genre)
        {
            return await _genreServices.AddGanres(genre);
        }

        [HttpPut("EditGenres")]
        public async Task<IActionResult> EditGenres(GanresViewModel genre, long id)
        {
            var existGenre = _genreServices.GetGanresById(id);
            if (existGenre == null)
            {
                return NotFound("Brand not found!");
            }

            existGenre.Title = genre.Title;

            await _genreServices.EditGanres(existGenre);

            return Ok();
        }

        [HttpDelete("DeleteGenre")]
        public async Task<IActionResult> DeleteGenre(long id)
        {
            var gen = _genreServices.GetGanresById(id);

            if (gen == null)
            {
                return NotFound(new { message = "Not found !" });
            }

            await _genreServices.DeleteGanres(id);

            return Ok();
        }
    }
    #endregion

    #region Language
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILanguagesServices _languageServices;

        public LanguageController(IMapper mapper, ILanguagesServices languageServices)
        {
            _mapper = mapper;
            _languageServices = languageServices;
        }

        public List<LanguageViewModel> languageList { get; set; }

        [HttpGet]
        public List<LanguageViewModel> GetAllGenres()
        {
            languageList = _languageServices.GetAllLanguages().ToList();
            return languageList;
        }

        [HttpGet("{id}")]
        public ActionResult<LanguageViewModel> GetLanguageById(long id)
        {
            var lang = _languageServices.GetLanguageById(id);
            if (lang == null)
                return NotFound(lang);
            else
                return Ok(lang);
        }

        [HttpPost]
        public long AddLanguageFromApi(long id)
        {
            return id;
        }

        [HttpPost("AddLanguage")]
        public async Task<long> AddLanguage(LanguageViewModel language)
        {
            return await _languageServices.AddLanguage(language);
        }

        [HttpPut("EditLanguage")]
        public async Task<IActionResult> EditLanguage(LanguageViewModel language, long id)
        {
            var existLanguage = _languageServices.GetLanguageById(id);
            if (existLanguage == null)
            {
                return NotFound("Brand not found!");
            }

            existLanguage.Title = language.Title;

            await _languageServices.EditLanguages(existLanguage);

            return Ok();
        }

        [HttpDelete("DeleteLanguage")]
        public async Task<IActionResult> DeleteLanguage(long id)
        {
            var lan = _languageServices.GetLanguageById(id);

            if (lan == null)
            {
                return NotFound(new { message = "Not found !" });
            }

            await _languageServices.DeleteLanguage(id);

            return Ok();
        }
    }
    #endregion
}
