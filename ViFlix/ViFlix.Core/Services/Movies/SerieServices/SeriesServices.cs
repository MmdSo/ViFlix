using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.Services.Movies.Ganereservices;
using ViFlix.Core.Services.Movies.LanguageServices;
using ViFlix.Core.Services.Movies.SeassonServices;
using ViFlix.Core.Tools;
using ViFlix.Core.ViewModels.MoviesViewModel;
using ViFlix.Data.Context;
using ViFlix.Data.Movies;
using ViFlix.Data.Repository;

namespace ViFlix.Core.Services.Movies.SerieServices
{
    public class SeriesServices : GenericRepository<Series>, ISeriesServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILanguagesServices _language;
        private readonly IGanreServices _ganres;
        private readonly ISeasonServices _seasons;
        public SeriesServices(AppDbContext context , IMapper mapper , ILanguagesServices language ,
            IGanreServices ganres , ISeasonServices seasons) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _language = language;
            _ganres = ganres;
            _seasons = seasons;
        }

        public async Task<long> AddSeries(SeriesViewModel series, IFormFile MImg)
        {
            if (MImg != null)
            {
                series.Poster = NameGenerator.GenerateUniqCode() + Path.GetExtension(MImg.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Posters", series.Poster);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    MImg.CopyTo(stream);
                }
                Tools.ImageConverter ImgResizer = new Tools.ImageConverter();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Posters", MImg.FileName);
            }
            var se = _mapper.Map<SeriesViewModel, Series>(series);
            await AddEntity(se);
            _context.SaveChanges();
            return series.Id;
        }

        public async Task DeleteSeries(long Id)
        {
            SeriesViewModel series = GetSeriesById(Id);

            var ser = _mapper.Map<SeriesViewModel, Series>(series);

            ser.IsDelete = true;

            EditEntity(ser);
            await SaveChanges();
        }

        public async Task EditSeries(SeriesViewModel series, IFormFile MImg)
        {
            if (MImg != null)
            {
                series.Poster = NameGenerator.GenerateUniqCode() + Path.GetExtension(MImg.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Posters", series.Poster);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    MImg.CopyTo(stream);
                }
                Tools.ImageConverter ImgResizer = new Tools.ImageConverter();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Posters", MImg.FileName);
            }
            var pO = _mapper.Map<SeriesViewModel, Series>(series);
            EditEntity(pO);
            await SaveChanges();
        }

        public IEnumerable<SeriesViewModel> GetAllSeries()
        {
            var serie = _mapper.Map<IEnumerable<Series>, IEnumerable<SeriesViewModel>>(GetAll());
            return serie;
        }

        public List<SeriesViewModel> GetNewSeries()
        {
            var movie = _mapper.Map<IEnumerable<Series>, IEnumerable<SeriesViewModel>>(GetAll().OrderBy(p => p.DateCreated)).ToList();
            return movie;
        }

        public List<SeriesViewModel> GetOldSeries()
        {
            var movie = _mapper.Map<IEnumerable<Series>, IEnumerable<SeriesViewModel>>(GetAll().OrderBy(p => p.DateCreated)).ToList();
            return movie;
        }

        public List<SeriesViewModel> GetSeriesByGenreId(long id)
        {
            var movie = _mapper.Map<IEnumerable<Series>, IEnumerable<SeriesViewModel>>(GetAll().Where(p => p.GanreId == id)).ToList();
            return movie; 
        }

        public SeriesViewModel GetSeriesById(long id)
        {
            var movie = _mapper.Map<Series, SeriesViewModel>(GetEntityById(id));

            movie.GanreTitle = _ganres.GetGanresById(movie.GanreId).Title;
            movie.LanguageTitle = _language.GetLanguageById(movie.LanguageId).Title;

            return movie;
        }

        public List<SeriesViewModel> GetSeriesByLanguageId(long id)
        {
            var movie = _mapper.Map<IEnumerable<Series>, IEnumerable<SeriesViewModel>>(GetAll().Where(p => p.LanguageId == id)).ToList();
            return movie;
        }

        public List<SeriesViewModel> GetSeasonsBySeriesId(long SeriesId)
        {
            var movie = _mapper.Map<IEnumerable<Series>, IEnumerable<SeriesViewModel>>(GetAll().Where(p => p.Id == SeriesId)).ToList();
            return movie;
        }

        public async Task<IEnumerable<SeriesViewModel>> GetSeriesByTitleAsync(string? title)
        {
            var mo = GetAllSeries().Where(p => p.Title.ToLower().Contains(title.ToLower()));

            foreach (var item in mo)
            {
                item.GanreTitle = _ganres.GetGanresById(item.GanreId).Title;
                item.LanguageTitle = _language.GetLanguageById(item.LanguageId).Title;
            }
            return mo;
        }
    }
}
