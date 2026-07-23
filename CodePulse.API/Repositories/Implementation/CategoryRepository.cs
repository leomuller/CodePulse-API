using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;

namespace CodePulse.API.Repositories.Implementation
{
	public class CategoryRepository : ICategoryRespository
	{
		public ApplicationDbContext _dbContext { get; }
	
		public CategoryRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Category> CreateAsync(Category category)
		{
			await _dbContext.Categories.AddAsync(category);
			await _dbContext.SaveChangesAsync();
			
			return category;
		}
	}
}
