using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.MoviesViewModel;
using ViFlix.Data.Context;
using ViFlix.Data.Movies;
using ViFlix.Data.Repository;

namespace ViFlix.Core.Services.Movies.ActorServices
{
    public class ActorsServices : GenericRepository<Actors> , IActorsServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ActorsServices(AppDbContext context , IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> AddActors(ActorsViewModel actor)
        {
            var act = _mapper.Map<ActorsViewModel, Actors>(actor);
            await AddEntity(act);
            _context.SaveChanges();
            return act.Id;
        }

        public async Task DeleteActors(long id)
        {
            ActorsViewModel act = GetActorsById(id);

            var ac = _mapper.Map<ActorsViewModel, Actors>(act);

            ac.IsDelete = true;

            EditEntity(ac);
            await SaveChanges();
        }

        public async Task EditActors(ActorsViewModel actor)
        {
            var act = _mapper.Map<ActorsViewModel, Actors>(actor);
            EditEntity(act);
            await SaveChanges();
        }

        public IEnumerable<ActorsViewModel> GetAllActors()
        {
            var act = _mapper.Map<IEnumerable<Actors>, IEnumerable<ActorsViewModel>>(GetAll());
            return act;
        }

        public ActorsViewModel GetActorsById(long? Id)
        {
            var act = _mapper.Map<Actors, ActorsViewModel>(GetEntityById(Id));
            return act;
        }
    }
}
