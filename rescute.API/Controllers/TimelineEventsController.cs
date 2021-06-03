using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using rescute.API.Extensions;
using rescute.API.Models;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;
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
    public class TimelineItemsController : ControllerBase
    {
        private readonly string relativeAttachmentsRoot;
        //private readonly ILogger<AnimalsController> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileStorageService storageService;

        public TimelineItemsController(IFileStorageService storageService, IUnitOfWork unitOfWork, IConfiguration config)
        {
            //this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.storageService = storageService;
            this.relativeAttachmentsRoot = config["RelativeAttachmentsRootPath"];
        }

        [HttpPost("api/[controller]/StatusReports")]
        public async Task<ActionResult> PostStatus([FromForm] StatusReportPostModel statusModel)
        {
            var samaritan = Samaritan.RandomTestSamaritan();
            unitOfWork.Samaritans.Add(samaritan);


            var animal = await unitOfWork.Animals.GetAsync(Id<Animal>.Generate(Guid.Parse(statusModel.AnimalId)));
            if (animal == null) return NotFound(statusModel.AnimalId);

            var statusReport = new StatusReport(DateTime.Now,
                                                    samaritan.Id,
                                                    animal.Id,
                                                    new MapPoint(statusModel.Lattitude, statusModel.Longitude),
                                                    statusModel.Description);

            if (statusModel.Attachments != null)
            {
                foreach (var file in statusModel.Attachments)
                {
                    statusReport.AddAttachments(await storageService.Store(file, statusReport.Id.ToString()));
                }
            }

            unitOfWork.TimelineItems.Add(statusReport);

            await unitOfWork.Complete();

            return CreatedAtAction(nameof(GetStatus), new { id = statusReport.Id }, statusReport.ToModel(relativeAttachmentsRoot));
        }
        [HttpGet("api/[controller]/StatusReports")]
        public async Task<ActionResult<IEnumerable<StatusReportGetModel>>> GetStatus([FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            if (pageSize < 1 || pageIndex < 0) return BadRequest();

            var events = await unitOfWork.TimelineItems.GetAsync<StatusReport>(a => true, pageSize, pageIndex);
            return Ok(events.ToModel(relativeAttachmentsRoot));
        }
        [HttpGet("api/[controller]/StatusReports/{id}")]
        public async Task<ActionResult<StatusReportGetModel>> GetStatus([FromRoute] Guid id)
        {

            var statusEvent = await unitOfWork.TimelineItems.GetAsync(Id<TimelineItem>.Generate(id));
            if (statusEvent == null) return NotFound();
            return Ok(((StatusReport)statusEvent).ToModel(relativeAttachmentsRoot));
        }

        [HttpPost("api/[controller]/TransportRequests")]
        public async Task<ActionResult> PostTransportRequest([FromForm] TransportRequestPostModel transportModel)
        {
            var samaritan = Samaritan.RandomTestSamaritan();
            unitOfWork.Samaritans.Add(samaritan);


            var animal = await unitOfWork.Animals.GetAsync(Id<Animal>.Generate(Guid.Parse(transportModel.AnimalId)));
            if (animal == null) return NotFound(transportModel.AnimalId);

            var transportRequested = new TransportRequest(DateTime.Now,
                                                    samaritan.Id,
                                                    animal.Id,
                                                    new MapPoint(transportModel.Lattitude, transportModel.Longitude),
                                                    new MapPoint(transportModel.ToLattitude, transportModel.ToLongitude),
                                                    transportModel.Description);


            unitOfWork.TimelineItems.Add(transportRequested);

            await unitOfWork.Complete();

            return CreatedAtAction(nameof(GetTransportRequest), new { id = transportRequested.Id }, transportRequested.ToModel());
        }
        [HttpGet("api/[controller]/TransportRequests")]
        public async Task<ActionResult<IEnumerable<TransportRequestGetModel>>> GetTransportRequests([FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            if (pageSize < 1 || pageIndex < 0) return BadRequest();

            var events = await unitOfWork.TimelineItems.GetAsync<TransportRequest>(a => true, pageSize, pageIndex);
            return Ok(events.ToModel());
        }

        [HttpGet("api/[controller]/TransportRequests/{id}")]
        public async Task<ActionResult<TransportRequestGetModel>> GetTransportRequest([FromRoute] Guid id)
        {

            var statusEvent = await unitOfWork.TimelineItems.GetAsync(Id<TimelineItem>.Generate(id));
            if (statusEvent == null) return NotFound();
            return Ok(((TransportRequest)statusEvent).ToModel());
        }

        //[HttpPost("api/[controller]/Bills")]
        //public async Task<ActionResult> PostBill([FromForm] BillPostModel billModel)
        //{
        //    var samaritan = Samaritan.RandomTestSamaritan();
        //    unitOfWork.Samaritans.Add(samaritan);

        //    var animal = await unitOfWork.Animals.GetAsync(Id<Animal>.Generate(Guid.Parse(billModel.AnimalId)));
        //    if (animal == null) return NotFound(billModel.AnimalId);
        //    //unitOfWork.TimelineItems.GetAsync()
        //    //billModel.
        //    var id = Id<TimelineItem>.Generate();
        //    var attachments = new List<Attachment>();
        //    if (billModel.Attachments != null)
        //    {
        //        foreach (var file in billModel.Attachments)
        //        {
        //            attachments.Add(await storageService.Store(file, id.ToString()));
        //        }
        //    }

        //    var bill = new Bill(id, DateTime.Now,
        //                        samaritan.Id,
        //                        animal.Id,
        //                        billModel.Description,
        //                        billModel.Total,
        //                        billModel.IncludesLabResults,
        //                        billModel.IncludesPrescription,
        //                        billModel.IncludesVetFee
        //                        );


        //    unitOfWork.TimelineItems.Add(statusReported);

        //    await unitOfWork.Complete();

        //    return CreatedAtAction(nameof(GetStatus), new { id = statusReported.Id }, statusReported.ToModel(relativeAttachmentsRoot));
        //}


    }
}
