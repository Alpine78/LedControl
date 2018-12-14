using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HarjoitustyoLed
{
    class LedControl
    {

        private static readonly HttpClient client = new HttpClient();
        private readonly string url;

        public LedControl()
        {
            url = "http://ilkka.freemyip.com:5000/api/pins/";
        }

        public async Task<Boolean> setStatus(Led led, int status)
        {

            string pinId = led.pinId.ToString();
            var values = new Dictionary<string, string>
            {
                { "pinId", pinId },
                { "status", status.ToString() }
            };

            try
            {
                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<int> getStatus(Led led)
        {
            string ledUrl = url + led.pinId;

            try
            {
                var result = await client.GetAsync(ledUrl);
                result.EnsureSuccessStatusCode();
                string content = await result.Content.ReadAsStringAsync();
                if (content == "Low")
                {
                    return 0;
                }
                else if (content == "High")
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task swapStatus(Led led)
        {
            if (await getStatus(led) == 0)
            {
                await setStatus(led, 1);
            }
            else if (await getStatus(led) == 1)
            {
                await setStatus(led, 0);
            }
        }

    }
}
