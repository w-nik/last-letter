using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core;
using Newtonsoft.Json;
using Polly;

namespace GameOfWords
{
    public class RemotePlayer : IRemotePlayer
    {
        private TaskCompletionSource<Message> _tcs;
        private readonly Uri _remoteBotUrl;
        private readonly int _matchId = 19;

        public RemotePlayer(string name, string remoteBorUrl)
        {
            _remoteBotUrl = new Uri(remoteBorUrl);
            Name = name;
        }

        public string Name { get; private set; }
        
        public async Task<Message> NextWord(string lastWord)
        {
            _tcs = new TaskCompletionSource<Message>();

            // Publish message to player
            //await SendRequest("next", lastWord);
            await SendMessage("next", new WordMessage { Match = _matchId, Word = lastWord });
            
            // Wait for answer
            return await _tcs.Task;
        }
        
        public void HandlePlayerResponse(Message message)
        {
            if (message.Player == Name)
            {
                _tcs.TrySetResult(message);
            }
        }

        public async Task WordAccepted(string player, string word)
        {
            //await SendRequest("accept", word);
            await SendMessage("accept", new WordMessage {Match = _matchId, Word = word});
        }

        public async Task WordRejected(string player, string word)
        {
            //await SendRequest("reject", word);
            await SendMessage("reject", new WordMessage { Match = _matchId, Word = word });
        }

        public async Task EndGame(string message)
        {
            //await SendRequest("endgame", message);
            await SendMessage("endgame", new GameMessage {Match = _matchId, Message = message});
        }

        //private async Task<Message> GetPlayerAnswer(string word)
        //{
        //    Message message = null;

        //    var url = new Uri(_remoteBotUrl, $"next/{word}");

        //    var retry = Policy
        //        .Handle<Exception>()
        //        .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        //    await retry.ExecuteAsync(async () =>
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            var response = await client.GetStringAsync(url);
        //            message = JsonConvert.DeserializeObject<Message>(response);
        //        }
        //    });

        //    return message;
        //}

        //private async Task SendRequest(string action, string word)
        //{
        //    var url = new Uri(_remoteBotUrl, $"{action}/{word}");
            
        //    var retry = Policy
        //        .Handle<Exception>()
        //        .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        //    await retry.ExecuteAsync(async () =>
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            await client.GetAsync(url);
        //        }
        //    });
        //}

        private async Task SendMessage(string action, object message)
        {
            var url = new Uri(_remoteBotUrl, $"{action}");
            var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

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
