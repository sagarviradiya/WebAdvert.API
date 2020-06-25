using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using WebAdvert.API.Services;
using WebAdvert.Models;

namespace WebAdvert.API.Controllers
{
    [ApiController]
    [Route("api/v1/adverts")]
    public class AdvertsController : ControllerBase
    {
        private readonly IAdvertStorageService advertStorageService;
        public AdvertsController(IAdvertStorageService advertStorageService)
        {
            this.advertStorageService = advertStorageService;
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
            }
            catch
            {
                throw;
            }
            return Ok();
        }
    }
}
