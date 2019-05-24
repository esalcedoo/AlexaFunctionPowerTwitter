using AlexaPowerTwitter.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AlexaPowerTwitter.Services
{
    public class PowerTwitterService
    {
        private HttpClient _client { get; }
        public PowerTwitterService(HttpClient client)
        {
            _client = client;
        }

        internal async Task<string> GetMessage()
        {
            string message = string.Empty;
            IList<FavModel> favList = await GetFavouritesList();
            return string.Empty;
        }

        internal async Task<List<FavModel>> GetFavouritesList()
        {
            List<FavModel> favModelList = new List<FavModel>();
            string route = "?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=e0yAgUDrfObYj_GI_gGBjGEfDz7Cj0JlnHoH1Sh0fYs";
            var response = await _client.GetAsync(route);

            if (response.IsSuccessStatusCode)
            {
                favModelList = await response.Content
                    .ReadAsAsync<List<FavModel>>();
            }

            return favModelList;
        }
    }
}
