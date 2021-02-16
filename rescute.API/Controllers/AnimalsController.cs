using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using rescute.API.Extensions;
using rescute.API.Models;
using rescute.Infrastructure.Services;
using rescute.Domain.Aggregates;
using rescute.Domain.ValueObjects;
using rescute.Infrastructure;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rescute.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private readonly string relativeAttachmentsRoot;
        //private readonly ILogger<AnimalsController> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileStorageService storageService;

        public AnimalsController(IFileStorageService storageService, IUnitOfWork unitOfWork, IConfiguration config)
        {
            //this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.storageService = storageService;
            this.relativeAttachmentsRoot = config["RelativeAttachmentsRootPath"];
        }
        [HttpPost]
        public async Task<ActionResult> PostAnimal([FromForm] AnimalPostModel animalModel)
        {
            var samaritan = new Samaritan(new Name("Test First"), new Name("Test Last"), new PhoneNumber(true, "0912121212"), DateTime.Now);
            var animal = new Animal(DateTime.Now, samaritan.Id, animalModel.Description, AnimalType.GetByName(animalModel.AnimalType));

            if (animalModel.Attachments != null)
            {
                foreach (var file in animalModel.Attachments)
                {
                    animal.AddAttachments(await storageService.Store(file, animal.Id.ToString(), AttachmentType.ProfilePicture()));
                }
            }
            unitOfWork.Samaritans.Add(samaritan);
            unitOfWork.Animals.Add(animal);

            await unitOfWork.Complete();

            return CreatedAtAction(nameof(Get), new { id = animal.Id }, animal.ToModel(relativeAttachmentsRoot));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnimalGetModel>>> Get([FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            var animals = await unitOfWork.Animals.GetAsync(a =>  true, pageSize, pageIndex);
            return Ok(animals.ToModel(relativeAttachmentsRoot));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AnimalGetModel>> Get([FromRoute] Guid id)
        {
            var animal = await unitOfWork.Animals.GetAsync(Id<Animal>.Generate(id));
            if (animal == null) return NotFound();
            return Ok(animal.ToModel(relativeAttachmentsRoot));
        }

    }
}
