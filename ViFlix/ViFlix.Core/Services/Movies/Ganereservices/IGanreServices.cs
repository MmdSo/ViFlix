using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.MoviesViewModel;
using ViFlix.Data.Movies;
using ViFlix.Data.Repository;

namespace ViFlix.Core.Services.Movies.Ganereservices
{
    public interface IGanreServices : IGenericRepository<Ganres>
    {
        IEnumerable<GanresViewModel> GetAllGanres();
        GanresViewModel GetGanresById(long? Id);
        Task<long> AddGanres(GanresViewModel ganres);
        Task EditGanres(GanresViewModel ganre);
        Task DeleteGanres(long id);
    }
}
