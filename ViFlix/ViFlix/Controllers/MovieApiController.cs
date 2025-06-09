using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ViFlix.Core.Services.Movies.ActorServices;
using ViFlix.Core.Services.Movies.DirectorServices;
using ViFlix.Core.Services.Movies.DownloadLinks;
using ViFlix.Core.Services.Movies.Ganereservices;
using ViFlix.Core.Services.Movies.LanguageServices;
using ViFlix.Core.Services.Movies.MovieServices;
using ViFlix.Core.Services.Movies.ReviewServices;
using ViFlix.Core.Services.Movies.SeassonServices;
using ViFlix.Core.Services.Movies.SerieServices;
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
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Posters", fileName);

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
            existMovie.LanguageId = movie.LanguageId;

           
            existMovie.DownloadLinks.Clear();


            if (movie.DownloadLinks != null && movie.DownloadLinks.Any())
            {
                foreach (var linkViewModel in movie.DownloadLinks)
                {
                    existMovie.DownloadLinks.Add(new DownloadLinksViewModel
                    {
                        Url = linkViewModel.Url,
                        Quality = linkViewModel.Quality,
                        
                    });
                }
            }

            string fileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(MImg.FileName);
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Posters", fileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await MImg.CopyToAsync(stream);
            }


            existMovie.Poster = "/Images/Posters" + fileName;


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

        [HttpGet("GetMovieByActorsId")]
        public async Task<IActionResult> GetMovieByActorsId(long Id)
        {
            var movie = _movieServices.GetMovieByActorsId(Id);
            return Ok(movie);
        }

        [HttpGet("GetMovieByDirectorId")]
        public async Task<IActionResult> GetMovieByDirectorId(long Id)
        {
            var movie = _movieServices.GetMovieByDirectorId(Id);
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

    #region downloadLinks
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadLinkApiController : ControllerBase
    {
        private readonly IDownloadLinksServices _LinkServices;

        public DownloadLinkApiController(IDownloadLinksServices LinkServices)
        {
            _LinkServices = LinkServices;
        }

        public List<DownloadLinksViewModel> LinkList { get; set; }

        [HttpGet]
        public List<DownloadLinksViewModel> GetAllLinks()
        {
            LinkList = _LinkServices.GetAllDownloadLinks().ToList();
            return LinkList;
        }

        [HttpGet("{id}")]
        public ActionResult<DownloadLinksViewModel> GetLinkById(long id)
        {
            var link = _LinkServices.GetLinksById(id);
            if (link == null)
                return NotFound(link);
            else
                return Ok(link);
        }

        [HttpPut("EditLinks")]
        public async Task<IActionResult> EditLinks([FromForm] DownloadLinksViewModel link, long id)
        {
            var existLink = _LinkServices.GetLinksById(id);
            if (existLink == null)
            {
                return NotFound("Links not found!");
            }

            existLink.Quality = link.Quality;
            existLink.Url = link.Url;
            

            await _LinkServices.EditDownloadLinkAsync(existLink);

            return Ok();
        }

        [HttpDelete("DeleteLink")]
        public async Task<IActionResult> DeleteLink(long id)
        {
            var link = _LinkServices.GetLinksById(id);

            if (link == null)
            {
                return NotFound(new { message = "Not found !" });
            }

            await _LinkServices.DeleteDownloadLinkAsync(id);

            return Ok();
        }

        [HttpGet("GetLinksBymovieId")]
        public async Task<IActionResult> GetLinksBymovieId(long MovieId)
        {
            var movie = _LinkServices.GetLinksByMovieId(MovieId);
            if(movie == null)
            {
                return NotFound("Movie id dosnt exist");
            }
            return Ok(movie);
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
        public async Task<long> AddGenre([FromForm]GanresViewModel genre)
        {
            return await _genreServices.AddGanres(genre);
        }

        [HttpPut("EditGenres")]
        public async Task<IActionResult> EditGenres([FromForm]GanresViewModel genre, long id)
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
        public async Task<long> AddLanguage([FromForm]LanguageViewModel language)
        {
            return await _languageServices.AddLanguage(language);
        }

        [HttpPut("EditLanguage")]
        public async Task<IActionResult> EditLanguage([FromForm] LanguageViewModel language, long id)
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

    #region Review
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IReviewsServices _reviewServices;

        public ReviewApiController(IMapper mapper, IReviewsServices reviewServices)
        {
            _mapper = mapper;
            _reviewServices = reviewServices;
        }


        [HttpGet("GetMovieReviews")]
        public async Task<ActionResult<IEnumerable<DisplayReviewViewModel>>> GetMovieReviews(long movieId)
        {
            var reviews = await _reviewServices.GetReviewsForMovieAsync(movieId);
            return Ok(reviews);
        }

        [HttpGet("GetSeriesReviews")]
        public async Task<ActionResult<IEnumerable<DisplayReviewViewModel>>> GetSeriesReviews(long seriesId)
        {
            var reviews = await _reviewServices.GetReviewsForSeriesAsync(seriesId);
            return Ok(reviews);
        }

        [HttpPost("AddReview")]
        [Authorize]
        public async Task<IActionResult> AddReview([FromForm] CreateReviewViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!long.TryParse(userIdString, out long userIdAsLong))
            {
                return Unauthorized("User not found!");
            }

            
                var createdReview = await _reviewServices.AddReviewAsync(viewModel, userIdAsLong);
                return Ok(createdReview);
            
           
        }

        [HttpPost("ApproveReview")]
        public async Task<IActionResult> ApproveReview(long reviewId)
        {
            var success = await _reviewServices.ApproveReviewAsync(reviewId);
            if (!success)
            {
                return NotFound("Dont find Any review");
            }
            return NoContent(); 
        }

        [HttpDelete("DeleteReview")]
        public async Task<IActionResult> DeleteReview(long reviewId)
        {
            var success = await _reviewServices.DeleteReviewAsync(reviewId);
            if (!success)
            {
                return NotFound("Dont find any reviews!");
            }
            return NoContent(); 
        }
    }
    #endregion

    #region Series
    [ApiController]
    [Route("api/[controller]")]
    public class SeriesApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISeriesServices _seriesServices;

        public SeriesApiController(IMapper mapper , ISeriesServices seriesServices)
        {
            _mapper = mapper;
            _seriesServices = seriesServices;
        }

        public List<SeriesViewModel> seriesList { get; set; }

        [HttpGet]
        public List<SeriesViewModel> GetAllSeries()
        {
            seriesList = _seriesServices.GetAllSeries().ToList();
            return seriesList;
        }

        [HttpGet("{id}")]
        public ActionResult<SeriesViewModel> GetSerieById(long id)
        {
            var serie = _seriesServices.GetSeriesById(id);
            if (serie == null)
                return NotFound();
            else
                return Ok(serie);
        }

        [HttpPost]
        public long AddMovieFromApi(long Id)
        {
            return Id;
        }

        [HttpPost("AddSerieFromApiBody")]
        public async Task<long> AddSerieFromApiBody([FromForm] SeriesViewModel serie, IFormFile SImg)
        {
            string fileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(SImg.FileName);
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Posters", fileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await SImg.CopyToAsync(stream);
            }

            return await _seriesServices.AddSeries(serie, SImg);
        }


        [HttpPut("EditSerie")]
        public async Task<IActionResult> EditSerie([FromForm] SeriesViewModel serie, IFormFile SImg, long Id)
        {
            var existSerie = _seriesServices.GetSeriesById(Id);
            if (existSerie == null)
            {
                return NotFound("movie is not found !");
            }

            existSerie.Country = serie.Country;
            existSerie.ActorsId = serie.ActorsId;
            existSerie.GanreId = serie.GanreId;
            existSerie.LanguageId = serie.LanguageId;
            existSerie.Description = serie.Description;
            existSerie.DirectorId = serie.DirectorId;
            existSerie.ReleaseDate = serie.ReleaseDate;
            existSerie.Trailer = serie.Trailer;
            existSerie.Title = serie.Title;
            existSerie.Link = serie.Link;
            existSerie.IsDubed = serie.IsDubed;

            existSerie.Seasons.Clear();


            if (serie.Seasons != null && serie.Seasons.Any())
            {
                foreach (var season in serie.Seasons)
                {
                    existSerie.Seasons.Add(new SeasonsViewModel
                    {
                        SeasonNumber = season.SeasonNumber,
                        HostUrl = season.HostUrl,
                        SeriesId = season.SeriesId
                    });
                }
            }


            string fileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(SImg.FileName);
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Posters", fileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await SImg.CopyToAsync(stream);
            }


            existSerie.Poster = "/Images/Posters" + fileName;


            await _seriesServices.EditSeries(existSerie, SImg);

            return Ok();
        }

        [HttpDelete("DeleteSeries")]
        public async Task<IActionResult> DeleteSeries(long id)
        {
            var se = _seriesServices.GetSeriesById(id);

            if (se == null)
            {
                return NotFound(new { message = "Not found !" });
            }

            await _seriesServices.DeleteSeries(id);

            return Ok();
        }


        [HttpPost("GetSeriesByTitle")]
        public async Task<IActionResult> GetSeriesByTitle([FromQuery] string title)
        {
            var serie = await _seriesServices.GetSeriesByTitleAsync(title);

            if (serie == null || !serie.Any())
            {
                return NotFound("Cant find any movies!");
            }

            return Ok(serie);

        }


        [HttpGet("GetSeriesByGenreId")]
        public async Task<IActionResult> GetSeriesByGenreId(long Id)
        {
            var movie = _seriesServices.GetSeriesByGenreId(Id);
            return Ok(movie);
        }


        [HttpGet("GetSeriesByLanguageId")]
        public async Task<IActionResult> GetSeriesByLanguageId(long Id)
        {
            var movie = _seriesServices.GetSeriesByLanguageId(Id);
            return Ok(movie);
        }


        [HttpGet("GetSeasonsBySeriesId")]
        public async Task<IActionResult> GetSeasonsBySeriesId(long Id)
        {
            var movie = _seriesServices.GetSeasonsBySeriesId(Id);
            return Ok(movie);
        }

        [HttpGet("GetNewMovies")]
        public List<SeriesViewModel> GetNewSeries()
        {
            seriesList = _seriesServices.GetNewSeries().ToList();
            return seriesList;
        }

        [HttpGet("GetOldMovies")]
        public List<SeriesViewModel> GetOldSeries()
        {
            seriesList = _seriesServices.GetOldSeries().ToList();
            return seriesList;
        }
    }
    #endregion

    #region Seasons
    [Route("api/[controller]")]
    [ApiController]
    public class SeasonsApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISeasonServices _seasonServices;

        public SeasonsApiController(IMapper mapper , ISeasonServices seasonServices)
        {
            _mapper = mapper;
            _seasonServices = seasonServices;
        }

        public List<SeasonsViewModel> seasonList { get; set; }

        [HttpGet]
        public List<SeasonsViewModel> GetAllSeasons()
        {
            seasonList = _seasonServices.GetAllSeasons().ToList();
            return seasonList;
        }

        [HttpGet("{id}")]
        public ActionResult<SeasonsViewModel> GetSeasonById(long Id)
        {
            var season = _seasonServices.GetSeasonById(Id);
            if (season == null)
                return NotFound();
            else
                return Ok(season);
        }

        [HttpPost]
        public long AddSeasonFromApi(long id)
        {
            return id;
        }

        //[HttpPut("EditSeason")]
        //public async Task<IActionResult> EditSeason(SeasonsViewModel season, long id)
        //{
        //    var existSeason = _seasonServices.GetSeasonById(id);
        //    if (existSeason == null)
        //    {
        //        return NotFound("Brand not found!");
        //    }

        //    existSeason.SeriesId = season.Id;
        //    existSeason.SeasonNumber = season.SeasonNumber;
        //    existSeason.SeriesId = existSeason.Id;


        //    await _seasonServices.EditSeason(existSeason);

        //    return Ok();
        //}

        [HttpDelete("DeleteSeasons")]
        public async Task<IActionResult> DeleteSeasons(long id)
        {
            var sea = _seasonServices.GetSeasonById(id);

            if (sea == null)
            {
                return NotFound(new { message = "Not found !" });
            }

            await _seasonServices.DeleteSeason(id);

            return Ok();
        }

        [HttpGet("GetSeasonsBySeriesId")]
        public async Task<IActionResult> GetSeasonsBySeriesId(long Id)
        {
            var sea = _seasonServices.GetSeasonsBySeriesId(Id);

            return Ok(sea);
        }
    }
    #endregion

    #region Actors
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsApiController : ControllerBase
    {
        public readonly IActorsServices _actorServices;

        public ActorsApiController(IActorsServices actorServices)
        {
            _actorServices = actorServices;
        }

        public List<ActorsViewModel> ActorList { get; set; }

        [HttpGet]
        public List<ActorsViewModel> GetAllActors()
        {
            ActorList = _actorServices.GetAllActors().ToList();
            return ActorList;
        }

        [HttpGet("{id}")]
        public ActionResult<ActorsViewModel> GetActorsByid(long id)
        {
            var actors = _actorServices.GetActorsById(id);
            if (actors == null)
                return NotFound();
            else
                return Ok(actors);
        }

        [HttpPost]
        public long AddActorsFromApi(long id)
        {
            return id;
        }

        [HttpPost("AddActors")]
        public async Task<long> AddActors([FromForm]ActorsViewModel actor)
        {
            return await _actorServices.AddActors(actor);
        }

        [HttpPut("EditActors")]
        public async Task<IActionResult> EditActors([FromForm] ActorsViewModel actor, long id)
        {
            var existActor = _actorServices.GetActorsById(id);
            if (existActor == null)
            {
                return NotFound("actors not found!");
            }

            existActor.ActorFullName = actor.ActorFullName;


            await _actorServices.EditActors(existActor);

            return Ok();
        }

        [HttpDelete("DeleteDirector")]
        public async Task<IActionResult> DeleteDirector(long id)
        {
            var actor = _actorServices.GetActorsById(id);

            if (actor == null)
            {
                return NotFound(new { message = "Not found !" });
            }

            await _actorServices.DeleteActors(id);

            return Ok();
        }
    }
    #endregion

    #region Director
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorApiController : ControllerBase
    {
        public readonly IDirectorsServices _directorServices;

        public DirectorApiController(IDirectorsServices directorServices)
        {
            _directorServices = directorServices;
        }

        public List<DirectorViewModel> directorList { get; set; }

        [HttpGet]
        public List<DirectorViewModel> GetAllDirectors()
        {
            directorList = _directorServices.GetAllDirector().ToList();
            return directorList;
        }

        [HttpGet("{id}")]
        public ActionResult<DirectorViewModel> GetDirectorsByid(long id)
        {
            var directors = _directorServices.GetDirectorById(id);
            if (directors == null)
                return NotFound();
            else
                return Ok(directors);
        }

        [HttpPost]
        public long AddDirectorsFromApi(long id)
        {
            return id;
        }

        [HttpPost("AddDirectors")]
        public async Task<long> AddDirectors([FromForm] DirectorViewModel director)
        {
            return await _directorServices.AddDirector(director);
        }

        [HttpPut("EditDirector")]
        public async Task<IActionResult> EditDirector([FromForm] DirectorViewModel director, long id)
        {
            var existDirector = _directorServices.GetDirectorById(id);
            if (existDirector == null)
            {
                return NotFound("actors not found!");
            }

            existDirector.DirectorName = director.DirectorName;


            await _directorServices.EditDirector(existDirector);

            return Ok();
        }

        [HttpDelete("DeleteDirector")]
        public async Task<IActionResult> DeleteDirector(long id)
        {
            var director = _directorServices.GetDirectorById(id);

            if (director == null)
            {
                return NotFound(new { message = "Not found !" });
            }

            await _directorServices.DeleteDirector(id);

            return Ok();
        }
    }
    #endregion
}
