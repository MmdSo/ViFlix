using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.MoviesViewModel;
using ViFlix.Data.Movies;
using ViFlix.Data.Repository;

namespace ViFlix.Core.Services.Movies.SerieServices
{
    public interface ISeriesServices : IGenericRepository<Series>
    {
        IEnumerable<SeriesViewModel> GetAllSeries();
        List<SeriesViewModel> GetSeriesByGenreId(long id);
        List<SeriesViewModel> GetSeriesByLanguageId(long id);
        List<SeriesViewModel> GetSeriesBySeasonsId(long id);
        Task<long> AddSeries(SeriesViewModel series, IFormFile MImg);
        Task EditSeries(SeriesViewModel series, IFormFile MImg);
        Task DeleteSeries(long Id);
        Task<IEnumerable<SeriesViewModel>> GetSeriesByTitleAsync(string? title);
        SeriesViewModel GetSeriesById(long id);
        List<SeriesViewModel> GetNewSeries();
        List<SeriesViewModel> GetOldSeries();
    }
}
