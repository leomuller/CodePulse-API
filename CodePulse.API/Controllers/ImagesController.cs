using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ImagesController : Controller
	{
		private readonly IImageRespository _imageRepository;

		public ImagesController(IImageRespository imageRepository)
		{
			_imageRepository = imageRepository;
		}

		//Get {apibaseurl}/api/images
		[HttpGet]
		public async Task<IActionResult> GetAllImages()
		{
			var imageList = await _imageRepository.GetAll();

			//convert to DTO
			var response = new List<BlogImageDTO>();
			foreach (var blogImage in imageList)
			{
				BlogImageDTO responseImage = new BlogImageDTO{
					Id = blogImage.Id,
					FileName = blogImage.FileName,
					FileExtension = blogImage.FileExtension,
					Title = blogImage.Title,
					Url = blogImage.Url,
					DateCreated = blogImage.DateCreated
				};

				response.Add(responseImage);
			}

			return Ok(response);

		}



		//POST {apibaseurl}/api/images
		[HttpPost]
		[Consumes("multipart/form-data")]
		[Route("UploadImage")]
		public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] string fileName, [FromForm] string title) {

			ValidateFileUpload(file);

			if(ModelState.IsValid)
			{
				//file upload.
				var blogImage = new Models.Domain.BlogImage
				{
					FileName = fileName,
					FileExtension = Path.GetExtension(file.FileName).ToLower(),
					Title = title,
					DateCreated = DateTime.UtcNow
				};

				blogImage = await _imageRepository.Upload(file, blogImage);

				//convert to DTO:
				var response = new BlogImageDTO
				{
					Id = blogImage.Id,
					FileName = blogImage.FileName,
					FileExtension = blogImage.FileExtension,
					Title = blogImage.Title,
					Url = blogImage.Url,
					DateCreated = blogImage.DateCreated
				};

				return Ok(response);
			}

			return BadRequest(ModelState);

		}

		private void ValidateFileUpload(IFormFile file) { 
		
			var allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif" };

			if (allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) == false) {
				// File is invalid
				ModelState.AddModelError("file", "Invalid file type. Only JPG, JPEG, PNG, and GIF files are allowed.");
			}

			if (file.Length > 2000000)
			{
				// File is invalid
				ModelState.AddModelError("file", "Max filesize exceeded.");
			}
		}
		
	}
}
