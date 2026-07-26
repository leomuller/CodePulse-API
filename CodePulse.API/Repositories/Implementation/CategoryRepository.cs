using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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

		public async Task<IEnumerable<Category>> GetAllAsync()
		{
			return await _dbContext.Categories.ToListAsync();
		}

		public async Task<Category> GetByIdAsync(Guid id)
		{
			// returns null if not found
			return await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id ==	id);
		}

		public async Task<Category?> UpdateAsync(Category category)
		{
			var existing = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
			if (existing == null)
			{
				return null;
			}

			// copy/update properties as needed, or use Update() if you replace the whole entity
			_dbContext.Entry(existing).CurrentValues.SetValues(category);
			await _dbContext.SaveChangesAsync();

			return category;
		}

		//public async Task<Category> DeleteAsync(Guid id)
		//{
		//	var existing = await _dbContext.Categories.FindAsync(id);
		//	if (existing == null)
		//	{
		//		return null;
		//	}

		//	_dbContext.Categories.Remove(existing);
		//	await _dbContext.SaveChangesAsync();

		//	return existing;
		//}

		public async Task<Category> DeleteByIdAsync(Guid id)
		{
			var existing = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id ==	id);
			if (existing == null)
			{
				return null;
			}

			_dbContext.Categories.Remove(existing);
			await _dbContext.SaveChangesAsync();

			return existing;
		}
	}
}
