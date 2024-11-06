using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleWebApi.Data.Repositories;
using SampleWebApi.Models.Domain;
using SampleWebApiDto;

namespace SampleWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this._imageRepository = imageRepository;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto requestDto)
        {
            ValidateFileUpload(requestDto);

            if (ModelState.IsValid) {

                var imgDomainModel = new Image
                {
                    File=requestDto.File,
                    FileExtension=Path.GetExtension(requestDto.File.FileName),
                    FileDescription=requestDto.FileDescription,
                    FileSizeInBytes=requestDto.File.Length,
                    FileName=requestDto.FileName,  
                };

                // User repository to upload image
                await _imageRepository.Upload(imgDomainModel);
                return Ok(imgDomainModel);
            }
            

            return BadRequest(ModelState);

        }


        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExt = new string[] {".jpg",".jpeg",".png" };
            if (!allowedExt.Contains(Path.GetExtension(request.File.FileName))) {
                ModelState.AddModelError("file","unsupported file extension.");
            }

            if (request.File.Length > 10500000) {
                ModelState.AddModelError("file", "file size more than 10MB, please upload smaller size of file.");
            }

        }


    }
}
