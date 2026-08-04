using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
	public interface IImageRespository
	{

		Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);


	}
}
