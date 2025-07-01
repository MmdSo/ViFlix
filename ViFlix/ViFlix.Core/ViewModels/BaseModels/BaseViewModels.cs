using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViFlix.Core.ViewModels.BaseModels
{
    public class BaseViewModels
    {

        [BindNever]
        public long Id { get; set; }
        [BindNever]
        public DateTime DataCreated { get; set; } = DateTime.UtcNow;
        [BindNever]
        public DateTime DataModified { get; set; } = DateTime.UtcNow;
        [BindNever]
        public bool IsDelete { get; set; }
    }
}
