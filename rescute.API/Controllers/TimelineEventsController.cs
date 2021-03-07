using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using rescute.API.Extensions;
using rescute.API.Models;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineEvents;
using rescute.Domain.ValueObjects;
using rescute.Infrastructure;
using rescute.Infrastructure.Services;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace rescute.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimelineEventsController : ControllerBase
    {
        private readonly string relativeAttachmentsRoot;
        //private readonly ILogger<AnimalsController> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileStorageService storageService;

        public TimelineEventsController(IFileStorageService storageService, IUnitOfWork unitOfWork, IConfiguration config)
        {
            //this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.storageService = storageService;
            this.relativeAttachmentsRoot = config["RelativeAttachmentsRootPath"];
        }
        
        [HttpPost("api/[controller]/StatusReports")]
        public async Task<ActionResult> PostStatus([FromForm] StatusReportedPostModel statusModel)
        {
            var samaritan = Samaritan.RandomTestSamaritan();
            unitOfWork.Samaritans.Add(samaritan);


            var animal = await unitOfWork.Animals.GetAsync(Id<Animal>.Generate(Guid.Parse(statusModel.AnimalId)));
            if (animal == null) return NotFound(statusModel.AnimalId);

            var statusReported = new StatusReported(DateTime.Now,
                                                    samaritan.Id,
                                                    animal.Id,
                                                    new MapPoint(statusModel.Lattitude, statusModel.Longitude),
                                                    statusModel.Description);

            if (statusModel.Attachments != null)
            {
                foreach (var file in statusModel.Attachments)
                {
                    statusReported.AddAttachments(await storageService.Store(file, statusReported.Id.ToString(), file.ContentType.StartsWith("image") == true ? AttachmentType.Image() : AttachmentType.Video()));
                }
            }

            unitOfWork.TimelineEvents.Add(statusReported);

            await unitOfWork.Complete();

            return CreatedAtAction(nameof(GetStatus), new { id = statusReported.Id }, statusReported.ToModel(relativeAttachmentsRoot));
        }
        [HttpGet("api/[controller]/StatusReports")]
        public async Task<ActionResult<IEnumerable<StatusReportedGetModel>>> GetStatus([FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            if (pageSize < 1 || pageIndex < 0) return BadRequest();

            var events = await unitOfWork.TimelineEvents.GetAsync<StatusReported>(a => true, pageSize, pageIndex);
            return Ok(events.ToModel(relativeAttachmentsRoot));
        }
        [HttpGet("api/[controller]/StatusReports/{id}")]
        public async Task<ActionResult<StatusReportedGetModel>> GetStatus([FromRoute] Guid id)
        {
            
            var statusEvent = await unitOfWork.TimelineEvents.GetAsync(Id<TimelineEvent>.Generate(id));
            if (statusEvent == null) return NotFound();
            return Ok(((StatusReported)statusEvent).ToModel(relativeAttachmentsRoot));
        }

        [HttpPost("api/[controller]/TransportRequests")]
        public async Task<ActionResult> PostTransportRequest([FromForm] TransportRequestedPostModel transportModel)
        {
            var samaritan = Samaritan.RandomTestSamaritan();
            unitOfWork.Samaritans.Add(samaritan);


            var animal = await unitOfWork.Animals.GetAsync(Id<Animal>.Generate(Guid.Parse(transportModel.AnimalId)));
            if (animal == null) return NotFound(transportModel.AnimalId);

            var transportRequested = new TransportRequested(DateTime.Now,
                                                    samaritan.Id,
                                                    animal.Id,
                                                    new MapPoint(transportModel.Lattitude, transportModel.Longitude),
                                                    new MapPoint(transportModel.ToLattitude, transportModel.ToLongitude),
                                                    transportModel.Description);


            unitOfWork.TimelineEvents.Add(transportRequested);

            await unitOfWork.Complete();

            return CreatedAtAction(nameof(GetTransportRequest), new { id = transportRequested.Id }, transportRequested.ToModel(relativeAttachmentsRoot));
        }
        [HttpGet("api/[controller]/TransportRequests")]
        public async Task<ActionResult<IEnumerable<TransportRequestedGetModel>>> GetTransportRequests([FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            if (pageSize < 1 || pageIndex < 0) return BadRequest();

            var events = await unitOfWork.TimelineEvents.GetAsync<TransportRequested>(a => true, pageSize, pageIndex);
            return Ok(events.ToModel(relativeAttachmentsRoot));
        }

        [HttpGet("api/[controller]/TransportRequests/{id}")]
        public async Task<ActionResult<TransportRequestedGetModel>> GetTransportRequest([FromRoute] Guid id)
        {

            var statusEvent = await unitOfWork.TimelineEvents.GetAsync(Id<TimelineEvent>.Generate(id));
            if (statusEvent == null) return NotFound();
            return Ok(((TransportRequested)statusEvent).ToModel(relativeAttachmentsRoot));
        }

    }
}
