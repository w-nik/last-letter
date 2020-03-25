using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;

namespace Bot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BotController : ControllerBase
    {
        private readonly ILogger<BotController> _logger;
        private readonly IPlayerService _service;
        private readonly Publisher _publisher;

        public BotController(IPlayerService service, Publisher publisher, ILogger<BotController> logger)
        {
            _service = service;
            _logger = logger;
            _publisher = publisher;
        }

        [HttpPost("next")]
        public async Task PostNextWord([FromBody] WordMessage next)
        {
            if (_service.VerifyMatch(next.Match))
            {
                var message = _service.NextWord(next.Word);

                await _publisher.PublishResponse(message);
            }
        }

        [HttpPost("accept")]
        public Task WordAccepted([FromBody] WordMessage message)
        {
            if (_service.VerifyMatch(message.Match))
            {
                _service.WordAccepted(message.Word);
            }

            return Task.CompletedTask;
        }

        [HttpPost("reject")]
        public Task WordRejected([FromBody] WordMessage message)
        {
            if (_service.VerifyMatch(message.Match))
            {
                _service.WordRejected(message.Word);
            }

            return Task.CompletedTask;
        }

        [HttpGet("endgame")]
        public Task EndGame([FromBody] GameMessage message)
        {
            if (_service.VerifyMatch(message.Match))
            {
                _service.EndGame(message.Message);
            }

            return Task.CompletedTask;
        }
    }
}
