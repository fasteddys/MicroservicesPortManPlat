using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PMaP.Helpers;
using PMaP.Models.DBModels;
using PMaP.Models.Homes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PMaP.Services
{
    public class HomeService : IHomeService
    {
        private readonly AppSettings _appSettings;

        public HomeService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<HomeResponse> GetAll()
        {
            HomeResponse homeResponse = new HomeResponse();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.HomeUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var responseTask = await client.GetAsync("home");

                var result = responseTask;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    homeResponse = JsonConvert.DeserializeObject<HomeResponse>(readTask);
                }
            }

            return homeResponse;
        }
    }
}
