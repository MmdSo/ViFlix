using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.MoviesViewModel;
using ViFlix.Data.Movies;
using ViFlix.Data.Repository;

namespace ViFlix.Core.Services.Movies.DirectorServices
{
    public interface IDirectorsServices : IGenericRepository<Director>
    {
        IEnumerable<DirectorViewModel> GetAllDirector();
        DirectorViewModel GetDirectorById(long? Id);
        Task<long> AddDirector(DirectorViewModel director);
        Task EditDirector(DirectorViewModel director);
        Task DeleteDirector(long id);
    }
}
