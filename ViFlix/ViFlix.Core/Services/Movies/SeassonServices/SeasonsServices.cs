using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    }
}
