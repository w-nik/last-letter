using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOfWords
{
    [ApiController]
    [Route("[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly MatchService _service;

        public MatchController(MatchService service)
        {
            _service = service;
        }

        // GET match/answer/answerMessage
        [HttpGet("answer/{answerMessage}")]
        public Task Answer(string message)
        {
            var response = JsonConvert.DeserializeObject<Message>(message);

            _service.Matches[response.Match].ResponseFromPlayer(response);

            return Task.CompletedTask;
        }

        // POST api/bot/next
        [HttpPost("answer")]
        public Task Answer([FromBody] AnswerMessage answerMessage)
        {
            var m = new Message
            {
                Match = answerMessage.Match,
                Player = answerMessage.Player,
                Status = Enum.Parse<Status>(answerMessage.Status, true),
                Text = answerMessage.Text
            };

            _service.Matches[answerMessage.Match].ResponseFromPlayer(m);

            return Task.CompletedTask;
        }
    }

    public class MatchService
    {
        public IDictionary<int, Match> Matches { get; set; } = new Dictionary<int, Match>();
    }
}
