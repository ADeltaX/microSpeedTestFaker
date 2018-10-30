using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace microSpeedTestFaker
{
    public class SpeedTest
    {
        const string URL = "http://www.speedtest.net/api/results.php";
        const string HASH_MONT = "{0}-{1}-{2}-817d699764d33f89c"; //ping - upload - download - key
        const string CONTENT_STRING = "serverid={0}&hash={1}&ping={2}&jitter={3}&upload={4}&download={5}"; //Server ID - hash - ping - jitter - upload - download

        private readonly HttpClient _hc;

        public SpeedTest(HttpClient hc = null) => _hc = hc ?? new HttpClient();

        public async Task<string> DoFakeTestAsync(string ping, string upload, 
                                                  string download, string jitter = "1", 
                                                  string server = "303")
        {
            var hash = GetMD5Hash(string.Format(HASH_MONT, ping, upload, download));

            var req = CreateRequestHeader(HttpMethod.Post, URL);
            req.Content = CreateStringContentForm(string.Format(CONTENT_STRING,
                                                  server,
                                                  hash,
                                                  ping,
                                                  jitter,
                                                  upload,
                                                  download));

            var result = await _hc.SendAsync(req);

            return System.Net.WebUtility.HtmlDecode(await result.Content.ReadAsStringAsync());
        }

        private string GetMD5Hash(string input)
        {
            MD5 md5 = MD5.Create();

            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("x2"));

            md5.Dispose();

            return sb.ToString();
        }

        private StringContent CreateStringContentForm(string composedString)
        {
            var strContent = new StringContent(composedString);
            strContent.Headers.ContentType.CharSet = "UTF-8";
            strContent.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";
            return strContent;
        }

        private HttpRequestMessage CreateRequestHeader(HttpMethod method, string url)
        {
            var request = new HttpRequestMessage(method, new Uri(url));
            request.Headers.Add("User-Agent", "lolwat-10");
            request.Headers.Referrer = new Uri("http://www.speedtest.net/");
            request.Headers.Add("Origin", "http://www.speedtest.net/");
            request.Headers.Connection.Add("keep-alive");
            return request;
        }
    }
}
