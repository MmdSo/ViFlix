using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViFlix.Data.Context
{
    public class BaseEntitiies
    {
        public long Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsDelete { get; set; }
    }
}
