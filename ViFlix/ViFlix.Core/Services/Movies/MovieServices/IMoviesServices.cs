using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.MoviesViewModel;
using ViFlix.Data.Movies;
using ViFlix.Data.Repository;

namespace ViFlix.Core.Services.Movies.MovieServices
{
    public interface IMoviesServices : IGenericRepository<Movie>
    {
        IEnumerable<MovieViewModel> GetAllMovies();
        List<MovieViewModel> GetMovieByGenreId(long id);
        List<MovieViewModel> GetMovieByLanguageId(long id);
        List<MovieViewModel> GetMovieByActorsId(long id);
        List<MovieViewModel> GetMovieByDirectorId(long id);
        Task<long> AddMovies(MovieViewModel movie, IFormFile MImg);
        Task EditMovies(MovieViewModel movie, IFormFile MImg);
        Task DeleteMovies(long Id);
        Task<IEnumerable<MovieViewModel>> GetMoviesByTitleAsync(string? title);
        MovieViewModel GetMovieById(long id);
        List<MovieViewModel> GetNewMovie();
        List<MovieViewModel> GetOldMovie();
        MovieViewModel GetDownloadLinksById(long id);
    }
}
