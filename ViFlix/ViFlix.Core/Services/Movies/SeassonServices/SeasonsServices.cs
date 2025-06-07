using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.MoviesViewModel;
using ViFlix.Data.Context;
using ViFlix.Data.Movies;
using ViFlix.Data.Repository;

namespace ViFlix.Core.Services.Movies.SeassonServices
{
    public class SeasonsServices : GenericRepository<Seasons>, ISeasonServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public SeasonsServices(AppDbContext context , IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteSeason(long id)
        {
            SeasonsViewModel series = GetSeasonById(id);

            var ser = _mapper.Map<SeasonsViewModel, Seasons>(series);

            ser.IsDelete = true;

            EditEntity(ser);
            await SaveChanges();
        }

        public async Task EditSeason(SeasonsViewModel season)
        {
            var se = _mapper.Map<SeasonsViewModel, Seasons>(season);
            EditEntity(se);
            await SaveChanges();
        }

        public IEnumerable<SeasonsViewModel> GetAllSeasons()
        {
            var se = _mapper.Map<IEnumerable<Seasons>, IEnumerable<SeasonsViewModel>>(GetAll());
            return se;
        }

        public SeasonsViewModel GetSeasonById(long id)
        {
            var movie = _mapper.Map<Seasons, SeasonsViewModel>(GetEntityById(id));
            return movie;   
        }

        public List<SeasonsViewModel> GetSeasonsBySeriesId(long seriesId)
        {
            var movie = _mapper.Map<IEnumerable<Seasons>, IEnumerable<SeasonsViewModel>>(GetAll().Where(p => p.SeriesId == seriesId)).ToList();
            return movie;
        }
    }
}
