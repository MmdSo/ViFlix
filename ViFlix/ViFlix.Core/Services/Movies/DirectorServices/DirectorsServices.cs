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

namespace ViFlix.Core.Services.Movies.DirectorServices
{
    public class DirectorsServices : GenericRepository<Director>, IDirectorsServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public DirectorsServices(AppDbContext context , IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> AddDirector(DirectorViewModel director)
        {
            var dir = _mapper.Map<DirectorViewModel, Director>(director);
            await AddEntity(dir);
            _context.SaveChanges();
            return dir.Id;
        }

        public async Task DeleteDirector(long id)
        {
            DirectorViewModel dir = GetDirectorById(id);

            var director = _mapper.Map<DirectorViewModel, Director>(dir);

            director.IsDelete = true;

            EditEntity(director);
            await SaveChanges();
        }

        public async Task EditDirector(DirectorViewModel director)
        {
            var dir = _mapper.Map<DirectorViewModel, Director>(director);
            EditEntity(dir);
            await SaveChanges();
        }

        public DirectorViewModel GetDirectorById(long? Id)
        {
            var dir = _mapper.Map<Director, DirectorViewModel>(GetEntityById(Id));
            return dir;
        }

        public IEnumerable<DirectorViewModel> GetAllDirector()
        {
            var dir = _mapper.Map<IEnumerable<Director>, IEnumerable<DirectorViewModel>>(GetAll());
            return dir;
        }
    }
}
