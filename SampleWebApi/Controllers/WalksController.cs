using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleWebApi.CustomActionFilters;
using SampleWebApi.Data.Repositories;
using SampleWebApi.Models.Domain;
using SampleWebApiDto;
using SampleWebApiDto.Models.DTO;

namespace SampleWebApi.Controllers
{
    //  /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper,IWalkRepository walkRepository)
        {
            this._mapper = mapper;
            this._walkRepository = walkRepository;
        }
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateWalk([FromForm] AddWalkRequestDto addWalkRequestDto)
        {
           

                var walk = _mapper.Map<SampleWebApi.Models.Domain.Walk>(addWalkRequestDto);

                var rtn = await _walkRepository.AddWalkAsync(walk);
                // Map Domain Model to DTO
                var walkdto = _mapper.Map<WalkDto>(rtn);

                return Ok(walkdto);
         
        }

        //GET: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery]string? filterQuery, [FromQuery]string? sortBy, [FromQuery] bool? isAscending
            , [FromQuery] int pageNumber=1,[FromQuery]int pageSize=1000
            
            ) { 

            var model=await _walkRepository.GetAllAsync(filterOn, filterQuery, sortBy,isAscending??true, pageNumber,pageSize);

            var walDto=_mapper.Map<List<WalkDto>>(model);

            return Ok(walDto); 
        
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetWalkById([FromRoute]Guid id)
        {
            var model = await _walkRepository.GetByIdAsync(id);
            if (model == null)
                return NotFound();

            var walDto = _mapper.Map<WalkDto>(model);

            return Ok(walDto);


            

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWalk([FromRoute] Guid id)
        {
            var deletedWalk=await _walkRepository.DeleteAsync(id);
            if (deletedWalk == null) {  return NotFound(); }
            
            var walkDto=_mapper.Map<WalkDto>(deletedWalk);

            return Ok(walkDto);

        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateWalk([FromRoute]Guid id,[FromForm] UpdateWalkRequestDto updtWalkRequestDto)
        {
            if (ModelState.IsValid)
            {


                var walk = _mapper.Map<Walk>(updtWalkRequestDto);

                var walkDomainModel = await _walkRepository.UpdateAsync(id, walk);
                if (walkDomainModel == null)
                    return NotFound();


                var walkDto = _mapper.Map<WalkDto>(walkDomainModel);

                return Ok(walkDto);
            }
            else { 
                return BadRequest(); 
            }
        }

        }
}
