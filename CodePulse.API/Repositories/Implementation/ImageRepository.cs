using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;

namespace CodePulse.API.Repositories.Implementation
{
	public class ImageRepository : IImageRespository
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ApplicationDbContext _DbContext;

		public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, ApplicationDbContext applicationDbContext)
		{
			_webHostEnvironment = webHostEnvironment;
			_httpContextAccessor = httpContextAccessor;
			_DbContext = applicationDbContext;
		}

		public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
		{
			//1. Upload to the images folder. 
			var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "ImgLib", $"{blogImage.FileName}");

			using var stream = new FileStream(localPath, FileMode.Create);
			await file.CopyToAsync(stream);

			//2. update the DB.
			//https://localhost:5001/ImgLib/filename.jpg

			var req = _httpContextAccessor.HttpContext.Request;

			var urlPath = $"{req.Scheme}://{req.Host}{req.PathBase}/ImgLib/{blogImage.FileName}";

			blogImage.Url = urlPath;

			await _DbContext.BlogImages.AddAsync(blogImage);
			await _DbContext.SaveChangesAsync();

			return blogImage;


		}
	}
}
