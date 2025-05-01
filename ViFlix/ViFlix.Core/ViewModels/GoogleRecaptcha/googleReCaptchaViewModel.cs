using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViFlix.Core.ViewModels.GoogleRecaptcha
{
    public class googleReCaptchaViewModel
    {
        [Required]
        public string captchaToken { get; set; }
    }
}
