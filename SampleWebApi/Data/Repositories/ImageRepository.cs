using SampleWebApi.Models.Domain;

namespace SampleWebApi.Data.Repositories
{

    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }

    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly NZWalksDbContext _nZWalksDbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,IHttpContextAccessor httpContextAccessor,NZWalksDbContext nZWalksDbContext)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._httpContextAccessor = httpContextAccessor;
            this._nZWalksDbContext = nZWalksDbContext;
        }
        public async Task<Image> Upload(Image image)
        {
            var loaclFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images",$"{image.FileName}{image.FileExtension}");
            //upload image to local path
            using var stream = new FileStream(loaclFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            //find the relative path

            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            // Add the url path into DB
           await _nZWalksDbContext.Images.AddAsync(image);
           await _nZWalksDbContext.SaveChangesAsync();

            return image;
        }
    }
}
