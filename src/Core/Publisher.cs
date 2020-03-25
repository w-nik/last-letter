using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polly;

namespace Core
{
    public class Publisher
    {
        private readonly string _name;
        private readonly Uri _matchUrl;

        public Publisher(string name, string matchUrl)
        {
            _name = name;
            _matchUrl = new Uri(matchUrl);
        }

        public async Task PublishResponse(Message message)
        {
            var answer = new AnswerMessage
            {
                Match = 19,
                Player = _name,
                Status = message.Status.ToString(),
                Text = message.Text
            };

            var url = new Uri(_matchUrl, $"match/answer");
            var content = new StringContent(JsonConvert.SerializeObject(answer), Encoding.UTF8, "application/json");

            var retry = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            await retry.ExecuteAsync(async () =>
            {
                using (var client = new HttpClient())
                {
                    await client.PostAsync(url, content);
                }
            });
        }
    }
}
