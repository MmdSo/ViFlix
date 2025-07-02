using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViFlix.Core.ViewModels.RefreshToken
{
    public class TokenResponseViewModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
