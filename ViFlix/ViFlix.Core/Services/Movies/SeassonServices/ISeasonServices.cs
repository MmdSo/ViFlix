using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.MoviesViewModel;
using ViFlix.Data.Movies;
using ViFlix.Data.Repository;

namespace ViFlix.Core.Services.Movies.SeassonServices
{
    public interface ISeasonServices : IGenericRepository<Seasons>
    {
        IEnumerable<SeasonsViewModel> GetAllSeasons();
        List<SeasonsViewModel> GetSeasonsBySeriesId(long seriesId);
        Task<long> AddSeason(SeasonsViewModel season);
        Task EditSeason(SeasonsViewModel season);
        Task DeleteSeason(long id);
        SeasonsViewModel GetSeasonById(long id);
    }
}
