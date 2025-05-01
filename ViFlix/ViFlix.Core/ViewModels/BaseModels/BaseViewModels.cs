using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViFlix.Core.ViewModels.BaseModels
{
    public class BaseViewModels
    {
        public long Id { get; set; }
        public DateTime DataCreated { get; set; }
        public DateTime DataModified { get; set; }
        public bool IsDelete { get; set; }
    }
}
