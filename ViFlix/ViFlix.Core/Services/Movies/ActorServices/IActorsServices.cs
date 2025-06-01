using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.MoviesViewModel;
using ViFlix.Data.Movies;
using ViFlix.Data.Repository;

namespace ViFlix.Core.Services.Movies.ActorServices
{
    public interface IActorsServices : IGenericRepository<Actors>
    {
        IEnumerable<ActorsViewModel> GetAllActors();
        ActorsViewModel GetActorsById(long? Id);
        Task<long> AddActors(ActorsViewModel actor);
        Task EditActors(ActorsViewModel actor);
        Task DeleteActors(long id);
       
    }
}
