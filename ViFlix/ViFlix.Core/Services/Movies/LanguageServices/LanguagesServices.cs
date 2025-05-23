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

namespace ViFlix.Core.Services.Movies.LanguageServices
{
    public class LanguagesServices : GenericRepository<Language>, ILanguagesServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public LanguagesServices(AppDbContext context , IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> AddLanguage(LanguageViewModel language)
        {
            var lang = _mapper.Map<LanguageViewModel, Language>(language);
            await AddEntity(lang);
            _context.SaveChanges();
            return lang.Id;
        }

        public async Task DeleteLanguage(long Id)
        {
            LanguageViewModel brand = GetLanguageById(Id);

            var ba = _mapper.Map<LanguageViewModel, Language>(brand);

            ba.IsDelete = true;

            EditEntity(ba);
            await SaveChanges();
        }

        public async Task EditLanguages(LanguageViewModel language)
        {
            var Ba = _mapper.Map<LanguageViewModel, Language>(language);
            EditEntity(Ba);
            await SaveChanges();
        }

        public IEnumerable<LanguageViewModel> GetAllLanguages()
        {
            var Brands = _mapper.Map<IEnumerable<Language>, IEnumerable<LanguageViewModel>>(GetAll());
            return Brands;
        }

        public LanguageViewModel GetLanguageById(long? Id)
        {
            var Brands = _mapper.Map<Language, LanguageViewModel>(GetEntityById(Id));
            return Brands;
        }
    }
}
