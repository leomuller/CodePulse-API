using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
	public interface ICategoryRespository
	{
		//CRUD

		Task<Category> CreateAsync(Category category);

		Task<IEnumerable<Category>> GetAllAsync();

		Task<Category> GetAsync(Guid id);

		Task<Category> UpdateAsync(Category category);

		Task<Category> DeleteAsync(Guid id);
	}

}
