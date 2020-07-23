using Client.Models;
using Client.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Client.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("v1/[controller]")]
    public class HistoryController : ControllerBase
    {
        private readonly IServerApiConsumer iServerApiConsumer;

        public HistoryController(IServerApiConsumer iServerApiConsumer)
        {
            this.iServerApiConsumer = iServerApiConsumer ?? throw new ArgumentNullException(nameof(iServerApiConsumer));
        }

        [HttpGet]
        public async Task<IEnumerable<ChatEntry>> GetLastMessages(int nbLastMessages)
        {
            return await iServerApiConsumer.GetHistory(nbLastMessages);
        }
    }
}
