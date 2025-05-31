using AutoMapper;
using ViFlix.Core.ViewModels.MoviesViewModel;
using ViFlix.Data.Context;
using ViFlix.Data.Movies;
using ViFlix.Data.Repository;

namespace ViFlix.Core.Services.Movies.Ganereservices
{
    public class GaneresServices : GenericRepository<Ganres>, IGanreServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public GaneresServices(AppDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> AddGanres(GanresViewModel ganres)
        {
            var Ganre = _mapper.Map<GanresViewModel, Ganres>(ganres);
            await AddEntity(Ganre);
            _context.SaveChanges();
            return ganres.Id;
        }

        public async Task DeleteGanres(long id)
        {
            GanresViewModel ganre = GetGanresById(id);

            var ge = _mapper.Map<GanresViewModel, Ganres>(ganre);

            ge.IsDelete = true;

            EditEntity(ge);
            await SaveChanges();
        }


        public async Task EditGanres(GanresViewModel ganre)
        {
            var genre = _mapper.Map<GanresViewModel, Ganres>(ganre);
            EditEntity(genre);
            await SaveChanges();
        }

        public IEnumerable<GanresViewModel> GetAllGanres()
        {
            var ganres = _mapper.Map<IEnumerable<Ganres>, IEnumerable<GanresViewModel>>(GetAll());
            return ganres;
        }

        public GanresViewModel GetGanresById(long? Id)
        {
            var genre = _mapper.Map<Ganres , GanresViewModel>(GetEntityById(Id));
            return genre;
        }
    }  
 }  

