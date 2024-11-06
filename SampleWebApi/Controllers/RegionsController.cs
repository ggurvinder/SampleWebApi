using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleWebApi.Data.Repositories;
using SampleWebApi.Models.Domain;
using SampleWebApiDto.Models.DTO;
using System.Net;
using System.Text.Json;

namespace SampleWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(IRegionRepository regionRepository,IMapper mapper,ILogger<RegionsController> logger)
        {
            this._regionRepository = regionRepository;
            this._mapper = mapper;
            this._logger = logger;
        }
        [HttpGet]
        [Route("AllRegions")]
        [Authorize(Roles ="Reader")]

        public async Task<IActionResult> GetAll()
        {
               // int c = Convert.ToInt16("w");

                List<Models.Domain.Region> regions = await _regionRepository.GetAllAsync();

                _logger.LogInformation($"Finished GetAll request with data {JsonSerializer.Serialize(regions)}");

                var regionDto = _mapper.Map<List<SampleWebApiDto.Models.DTO.RegionDto>>(regions);
                return Ok(regionDto);
         
         

       
        }
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region=await _regionRepository.GetByIdAsync(id);
            if (region == null) { 
              return  NotFound();
            }
            var regionDto = _mapper.Map<SampleWebApiDto.Models.DTO.RegionDto>(region);
            return Ok(regionDto);
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> AddRegion([FromBody] SampleWebApiDto.Models.DTO.AddRegionRequestDto regionDto) {

            if (ModelState.IsValid) {
                var region = _mapper.Map<Models.Domain.Region>(regionDto);
                var reg = await _regionRepository.AddRegionAsync(region);
                return Ok(reg);
            }
            {

                return BadRequest(ModelState);
            }


        
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id ,[FromBody] SampleWebApiDto.Models.DTO.UpdateRegionRequestDto regionDto)
        {
            if (ModelState.IsValid)
            {

                var region = _mapper.Map<Models.Domain.Region>(regionDto);
                var reg = await _regionRepository.UpdateRegionAsync(region, id);
                
                if (reg == null) { 
                    return NotFound(); 
                }
                var returnRegiondto = _mapper.Map<SampleWebApiDto.Models.DTO.UpdateRegionRequestDto>(reg);
                return Ok(returnRegiondto);
            }
            else { 
             return BadRequest(ModelState);
            }

        }
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        { 
          var deletedRegion=  await _regionRepository.DeleteRegionAsynch(id);
            if (deletedRegion == null) { return NotFound(); }
            
            
            var returnRegiondto = _mapper.Map<SampleWebApiDto.Models.DTO.RegionDto>(deletedRegion);

            return Ok(returnRegiondto);

        }
    }
}
