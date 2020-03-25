using Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;

namespace User
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly Publisher _publisher;
        private readonly IPlayerService _service;

        public PlayerController(IPlayerService service, Publisher publisher, ILogger<PlayerController> logger)
        {
            _logger = logger;
            _service = service;
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
