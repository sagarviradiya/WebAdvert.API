using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using WebAdvert.API.Services;
using WebAdvert.Models;
using WebAdvert.Models.Messages;

namespace WebAdvert.API.Controllers
{
    [ApiController]
    [Route("api/v1/adverts")]
    public class AdvertsController : ControllerBase
    {
        private readonly IAdvertStorageService advertStorageService;
        private readonly IConfiguration _configuration;
        public AdvertsController(IAdvertStorageService advertStorageService, IConfiguration configuration)
        {
            this.advertStorageService = advertStorageService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("test")]
        public async Task<ActionResult> TestSite()
        {
            return Content((2 + 3).ToString());
        }
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> Create(AdvertModel model)
        {

            try
            {
                var recordId = await advertStorageService.Add(model);
                return CreatedAtAction("Create", new { Id = recordId });

            }
            catch
            {
                return NotFound();

            }
        }
        [HttpPut]
        [Route("confirm")]
        public async Task<ActionResult> Confirm(ConfirmAdvertModel model)
        {
            try
            {
                await advertStorageService.Confirm(model);
                await RaiseAdvertConfirmedMessage(model);
            }
            catch
            {
                throw;
            }
            return Ok();
        }

        private async Task RaiseAdvertConfirmedMessage(ConfirmAdvertModel model)
        {
            //get topic arn from config
            var topicArn = _configuration.GetValue<string>("SNSTopicArn");
            var advertModel = await advertStorageService.GetByIdAsync(model.Id);
            using (var client = new AmazonSimpleNotificationServiceClient())
            {
                var message = new AdvertConfirmedMessage() { Id = model.Id, Title = advertModel.Title };
                var messageJson = JsonSerializer.Serialize(message);
                await client.PublishAsync(topicArn, messageJson);
            }
        }
    }
}
