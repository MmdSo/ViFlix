using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.GoogleRecaptcha;

namespace FirstShop.Core.Security
{
    public class GoogleReCaptchaServices
    {
        private IConfiguration _config;
        public GoogleReCaptchaServices(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> VerifyRecaptchaToken(string token)
        {
            var secret = _config.GetSection("googleReCaptcha:SecretKey").Value;

            var url = $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={token}";

            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri(url);
                var response = await client.GetAsync(url);

                if(response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return false;
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var googleResult = JsonConvert.DeserializeObject<GoogleRecaptchaResponseViewModel>(responseString);

                return googleResult.success && googleResult.score >= 0.5;

            }
        }
    }
}
