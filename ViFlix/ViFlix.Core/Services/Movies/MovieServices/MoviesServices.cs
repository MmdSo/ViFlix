using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.Services.Movies.Ganereservices;
using ViFlix.Core.Services.Movies.LanguageServices;
using ViFlix.Core.Tools;
using ViFlix.Core.ViewModels.MoviesViewModel;
using ViFlix.Data.Context;
using ViFlix.Data.Movies;
using ViFlix.Data.Repository;

namespace ViFlix.Core.Services.Movies.MovieServices
{
    public class MoviesServices : GenericRepository<Movie>, IMoviesServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILanguagesServices _language;
        private readonly IGanreServices _ganres;
        public MoviesServices(AppDbContext context , IMapper mapper , IGanreServices ganres , ILanguagesServices language) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _ganres = ganres;
            _language = language;
        }

        public async Task<long> AddMovies(MovieViewModel movie, IFormFile MImg)
        {
            if (MImg != null)
            {
                movie.Poster = NameGenerator.GenerateUniqCode() + Path.GetExtension(MImg.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Posters", movie.Poster);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    MImg.CopyTo(stream);
                }
                Tools.ImageConverter ImgResizer = new Tools.ImageConverter();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Posters", MImg.FileName);
            }
            var movieEntity = _mapper.Map<MovieViewModel, Movie>(movie);

            if (movieEntity.DownloadLinks == null)
            {
                movieEntity.DownloadLinks = new List<DownloadLink>();
            }

            if (movie.DownloadLinks != null && movie.DownloadLinks.Any())
            {
                foreach (var linkViewModel in movie.DownloadLinks)
                {
                    movieEntity.DownloadLinks.Add(new DownloadLink
                    {
                        Url = linkViewModel.Url,
                        Quality = linkViewModel.Quality,
                        IsDelete = false 
                    });
                }
            }

            _context.Movie.Add(movieEntity);

            _context.SaveChanges();
            return movieEntity.Id;
        }

        public async Task DeleteMovies(long Id)
        {
            MovieViewModel movie =  GetMovieById(Id);

            var Product = _mapper.Map<MovieViewModel, Movie>(movie);

            Product.IsDelete = true;

            EditEntity(Product);
            await SaveChanges();
        }

        public async Task EditMovies(MovieViewModel movie, IFormFile MImg)
        {
            if (MImg != null)
            {
                movie.Poster = NameGenerator.GenerateUniqCode() + Path.GetExtension(MImg.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Posters", movie.Poster);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    MImg.CopyTo(stream);
                }
                Tools.ImageConverter ImgResizer = new Tools.ImageConverter();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Posters", MImg.FileName);
            }
            
            var pO = _mapper.Map<MovieViewModel, Movie>(movie);
            EditEntity(pO);
            await SaveChanges();
        }

        public IEnumerable<MovieViewModel> GetAllMovies()
        {
            var movie = _mapper.Map<IEnumerable<Movie>, IEnumerable<MovieViewModel>>(GetAll());
            return movie;
        }

        public List<MovieViewModel> GetMovieByGenreId(long id)
        {
            var movie = _mapper.Map<IEnumerable<Movie>, IEnumerable<MovieViewModel>>(GetAll().Where(p => p.GanreId == id)).ToList();
            return movie;
        }

        public MovieViewModel GetMovieById(long id)
        {
            var movie = _mapper.Map<Movie, MovieViewModel>(GetEntityById(id));

            movie.GanreTitle = _ganres.GetGanresById(movie.GanreId).Title;
            movie.LanguageTitle = _language.GetLanguageById(movie.LanguageId).Title;

            return movie;
        }

        public List<MovieViewModel> GetMovieByLanguageId(long id)
        {
            var movie = _mapper.Map<IEnumerable<Movie>, IEnumerable<MovieViewModel>>(GetAll().Where(p => p.LanguageId == id)).ToList();
            return movie;
        }

        public async Task<IEnumerable<MovieViewModel>> GetMoviesByTitleAsync(string? title)
        {
            var mo = GetAllMovies().Where(p => p.Title.ToLower().Contains(title.ToLower()));

            foreach (var item in mo)
            {
                item.GanreTitle = _ganres.GetGanresById(item.GanreId).Title;
                item.LanguageTitle = _language.GetLanguageById(item.LanguageId).Title;
            }
            return mo;
        }

        public List<MovieViewModel> GetNewMovie()
        {
            var movie = _mapper.Map<IEnumerable<Movie>, IEnumerable<MovieViewModel>>(GetAll().OrderBy(p => p.DateCreated) ).ToList();
            return movie;
        }

        public List<MovieViewModel> GetOldMovie()
        {
            var movie = _mapper.Map<IEnumerable<Movie>, IEnumerable<MovieViewModel>>(GetAll().OrderBy(p => p.DateCreated)).ToList();
            return movie;
        }

        public MovieViewModel GetDownloadLinksById(long id)
        {
            var movieEntity = GetAll().FirstOrDefault(m => m.Id == id);
            var movieVm = _mapper.Map<Movie, MovieViewModel>(movieEntity);

            
            var downloadLinks = _context.DownloadLinks.Where(dl => dl.MovieId == id).ToList(); 
            movieVm.DownloadLinks = _mapper.Map<List<DownloadLink>, List<DownloadLinksViewModel>>(downloadLinks);

            return movieVm;
        }

        public List<MovieViewModel> GetMovieByActorsId(long id)
        {
            var movie = _mapper.Map<IEnumerable<Movie>, IEnumerable<MovieViewModel>>(GetAll().Where(p => p.ActorsId == id)).ToList();
            return movie;
        }

        public List<MovieViewModel> GetMovieByDirectorId(long id)
        {
            var movie = _mapper.Map<IEnumerable<Movie>, IEnumerable<MovieViewModel>>(GetAll().Where(p => p.DirectorId == id)).ToList();
            return movie;
        }
    }
}
