using AECI.ICM.Shared.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AECI.ICM.Application.Services
{
    public class OnlineLogger : ILogger
    {
        #region Methods

        public async Task<bool> LogAsync(
            ILogMessageType message, string src="API", 
            string url="")
        {
            var _url = url;

            //var u = new Uri("http://madev04:8080/api/exceptions");
            if (!_url.EndsWith("/api"))
                _url = $"{_url}/api/exceptions";

            var u = new Uri($"{_url}/");

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var serialised = JsonConvert.SerializeObject(message);
                    var messageContent = new StringContent(serialised, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(u, messageContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;

                        return Convert.ToInt32(result) > 0;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
               throw ex;
            }
        }

        #endregion
    }
}
