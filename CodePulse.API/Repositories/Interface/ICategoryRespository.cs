using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
	public interface ICategoryRespository
	{
		Task<Category> CreateAsync(Category category);

	}
}
