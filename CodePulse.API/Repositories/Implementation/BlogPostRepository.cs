using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
	public class BlogPostRepository : IBlogPostRepository
	{

		public ApplicationDbContext _dbContext { get; }

		public BlogPostRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<BlogPost> CreateAsync(BlogPost blogpost)
		{
			await _dbContext.BlogPosts.AddAsync(blogpost);
			await _dbContext.SaveChangesAsync();

			return blogpost;
		}

		public async Task<BlogPost?> DeleteByIdAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<BlogPost>> GetAllAsync()
		{
			return await _dbContext.BlogPosts.ToListAsync();
		}

		public async Task<BlogPost?> GetByIdAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task<BlogPost?> UpdateAsync(BlogPost blogpost)
		{
			throw new NotImplementedException();
		}
	}
}
