﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Context;

namespace ViFlix.Data.Movies
{
    public class Language : BaseEntitiies
    {
        public string Title { get; set; }

        #region Relations
        public List<Movie> movie { get; set; }
        #endregion
    }
}
