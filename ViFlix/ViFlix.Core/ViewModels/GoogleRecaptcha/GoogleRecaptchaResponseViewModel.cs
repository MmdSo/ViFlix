using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViFlix.Core.ViewModels.GoogleRecaptcha
{
    public class GoogleRecaptchaResponseViewModel
    {
        public bool success { get; set; }
        public string hostname { get; set; }
        public double score { get; set; }
    }
}
